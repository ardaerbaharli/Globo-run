using Enums;
using UnityEngine;

namespace PowerUps
{
    public class PowerUps : MonoBehaviour
    {
        [SerializeField] public GameObject shieldEffect;
        [SerializeField] public GameObject slowEffect;
        [SerializeField] public GameObject speedEffect;

        public void Activate(PowerUpType type)
        {
            print(type);
            switch (type)
            {
                case PowerUpType.Shield:
                    shieldEffect.SetActive(true);
                    break;
                case PowerUpType.Slow:
                    slowEffect.SetActive(true);
                    break;
                case PowerUpType.Speed:
                    speedEffect.SetActive(true);
                    break;
            }
        }

        public void Deactivate(PowerUpType type)
        {
            switch (type)
            {
                case PowerUpType.Shield:
                    shieldEffect.SetActive(false);
                    break;
                case PowerUpType.Slow:
                    slowEffect.SetActive(false);
                    break;
                case PowerUpType.Speed:
                    speedEffect.SetActive(false);
                    break;
            }
        }
    }
}