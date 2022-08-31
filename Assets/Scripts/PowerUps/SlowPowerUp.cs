using System;
using System.Collections;
using UnityEngine;

namespace PowerUps
{
    public class SlowPowerUp : PowerUp
    {
        private SlowPowerUpEffect effect;

        private void Awake()
        {
            effect = Resources.Load<SlowPowerUpEffect>("PowerUps/SlowPowerUpEffect");
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
                var targetSpeed = player.defaultRunningSpeed -
                                  Mathf.Sin(t * Mathf.PI / effect.duration) * effect.slowAmount;
                if (targetSpeed > 5)
                    player.RunningSpeed = targetSpeed;
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