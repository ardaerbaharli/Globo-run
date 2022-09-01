using System;

namespace Enums
{
    [Flags]
    public enum PowerUpType
    {
        None = 0,
        Speed = 1 << 0,
        Slow = 1 << 1,
        Shield = 1 << 2,
    }
}