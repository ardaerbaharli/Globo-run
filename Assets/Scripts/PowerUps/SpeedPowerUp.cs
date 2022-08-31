using System;
using System.Collections;
using UnityEngine;

namespace PowerUps
{
    public class SpeedPowerUp : PowerUp
    {
        private SpeedPowerUpEffect effect;

        private void Awake()
        {
            effect = Resources.Load<SpeedPowerUpEffect>("PowerUps/SpeedPowerUpEffect");
        }

        public override void Activate(Player player)
        {
            StartCoroutine(Using(player));
        }

        private IEnumerator Using(Player player)
        {
            var t = 0.0f;
            while (t < effect.duration)
            {
                t += Time.deltaTime;
                player.RunningSpeed = Mathf.Sin(t * Mathf.PI / effect.duration) * effect.speedBoost + player.defaultRunningSpeed;
                yield return null;
            }
            
            yield return new WaitForSeconds(effect.duration);
            Deactivate(player);
        }


        public override void Deactivate(Player player)
        {
            player.RunningSpeed = player.defaultRunningSpeed;
        }
    }
}