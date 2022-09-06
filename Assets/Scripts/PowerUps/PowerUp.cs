using System;
using System.Collections;
using Enums;
using Managers;
using UnityEngine;

namespace PowerUps
{
    public class PowerUp : Collectable
    {
        public PowerUpType powerUpType;
        private PowerUpEffect _effect;
        private bool _didCollect;

        protected override void Collect()
        {
            Activate(Player.Instance);
        }

        private void Awake()
        {
            _effect = Resources.Load<PowerUpEffect>($"PowerUps/{powerUpType}PowerUpEffect");
        }


        public void Activate(Player player)
        {
            if (powerUpType == PowerUpType.Shield)
                player._shieldPowerUp = this;

            _didCollect = true;
            CollectableSpawner.Instance.Spawn();
            print("Activate");
            StartCoroutine($"Using{powerUpType}", player);
            player.PowerUps.Activate(powerUpType);
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
            player.hasShield = true;
            yield return new WaitForSeconds(_effect.duration);
            Deactivate(player);
        }

        private IEnumerator UsingHeart(Player player)
        {
            GameManager.Instance.GainedLife();
            yield return new WaitForSeconds(_effect.duration);
            Deactivate(player);
        }


        public void Deactivate(Player player)
        {
            player.PowerUps.Deactivate(powerUpType);
            if (powerUpType == PowerUpType.Shield)
                player.hasShield = false;

            _didCollect = false;
            ObjectPool.Instance.TakeBack(pooledObject);
        }

        public override void Missed()
        {
            if (_didCollect) return;
            print("missed");
            CollectableSpawner.Instance.Spawn();
            ObjectPool.Instance.TakeBack(pooledObject);
        }
    }
}