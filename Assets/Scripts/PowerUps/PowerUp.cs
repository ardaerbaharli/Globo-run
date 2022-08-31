using System;
using Enums;
using UnityEngine;

namespace PowerUps
{
    public abstract class PowerUp : MonoBehaviour
    {
        public PowerUpType PowerUpType;

        public abstract void Activate(Player player);
        public abstract void Deactivate(Player player);
    }
}