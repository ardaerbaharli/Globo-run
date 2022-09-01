using System;
using System.Collections;
using Enums;
using UnityEngine;

namespace PowerUps
{
    public class PowerUp : MonoBehaviour
    {
        private Material _material;

        public Action OnCollected;

        private PowerUpType _powerUpType;
        private PowerUpEffect _effect;
        public PooledObject pooledObject;
        private bool didCollect;


        private void Awake()
        {
            _material = GetComponent<Renderer>().material;
        }

        public void SetPowerUpType(PowerUpType powerUpType)
        {
            _powerUpType = powerUpType;
            _effect = Resources.Load<PowerUpEffect>($"PowerUps/{powerUpType}PowerUpEffect");

            if (_effect.changeMaterial) _material = _effect.tileMaterial;
            else if (_effect.changeColor) _material.color = _effect.tileColor;
        }

        public void Activate(Player player)
        {
            didCollect = true;
            PowerUpSpawner.Instance.Spawn();
            print("Activate");
            StartCoroutine($"Using{_powerUpType}", player);
        }

        private IEnumerator UsingSpeed(Player player)
        {
            print("using speed");
            var t = 0.0f;
            var speedBoost = ((SpeedPowerUpEffect) _effect).speedBoost;
            var startSpeed = player.RunningSpeed;
            if (startSpeed + speedBoost > player.maxSpeed) speedBoost = player.maxSpeed - startSpeed;
            
            while (t < _effect.duration)
            {
                t += Time.deltaTime;
                player.RunningSpeed = Mathf.Sin(t * Mathf.PI / 2 / _effect.duration) * speedBoost + startSpeed;
                yield return null;
            }

            Deactivate(player);
        }

        private IEnumerator UsingSlow(Player player)
        {
            print("using slow");
            var t = 0.0f;
            var slowAmount = ((SlowPowerUpEffect) _effect).slowAmount;
            var startSpeed = player.RunningSpeed;
            if (startSpeed - slowAmount < player.minSpeed) slowAmount = startSpeed - player.minSpeed;

            while (t < _effect.duration)
            {
                t += Time.deltaTime;
                player.RunningSpeed = startSpeed - Mathf.Sin(t * Mathf.PI / 2 / _effect.duration) * slowAmount;
                yield return null;
            }

            Deactivate(player);
        }

        private IEnumerator UsingShield(Player player)
        {
            yield return new WaitForSeconds(_effect.duration);
            Deactivate(player);
        }


        private void Deactivate(Player player)
        {
            didCollect = false;
            // player.RunningSpeed = player.defaultRunningSpeed;
            ObjectPool.Instance.TakeBack(pooledObject);
        }

        public void Missed()
        {
            if (didCollect) return;
            print("missed");
            PowerUpSpawner.Instance.Spawn();
            ObjectPool.Instance.TakeBack(pooledObject);
        }
    }
}