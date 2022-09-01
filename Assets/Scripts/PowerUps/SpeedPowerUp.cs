using System.Collections;
using UnityEngine;

namespace PowerUps
{
    public class SpeedPowerUp : MonoBehaviour
    {
        private SpeedPowerUpEffect _effect;

        private void Awake()
        {
            _effect = Resources.Load<SpeedPowerUpEffect>("PowerUps/SpeedPowerUpEffect");
        }

        public  void Activate(Player player)
        {
            StartCoroutine(Using(player));
        }

        private IEnumerator Using(Player player)
        {
            var t = 0.0f;
            while (t < _effect.duration)
            {                                           
                t += Time.deltaTime;
                player.RunningSpeed = Mathf.Sin(t * Mathf.PI / _effect.duration) * _effect.speedBoost + player.defaultRunningSpeed;
                yield return null;
            }
            
            yield return new WaitForSeconds(_effect.duration);
            Deactivate(player);
        }


        public  void Deactivate(Player player)
        {
            player.RunningSpeed = player.defaultRunningSpeed;
        }
    }
}