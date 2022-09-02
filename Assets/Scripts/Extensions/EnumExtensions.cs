using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
    public static class EnumExtensions
    {
        public static IEnumerable<Enum> GetUniqueFlags(this Enum flags)
        {
            ulong flag = 1;
            foreach (var value in Enum.GetValues(flags.GetType()).Cast<Enum>())
            {
                ulong bits = Convert.ToUInt64(value);
                while (flag < bits)
                {
                    flag <<= 1;
                }

                if (flag == bits && flags.HasFlag(value))
                {
                    yield return value;
                }
            }
        }

        public static Enum GetRandom(this Enum value)
        {
            var enumerable = Enum.GetValues(value.GetType()).Cast<Enum>().ToArray();
            return enumerable.ElementAt(new Random().Next(0, enumerable.Length));
        }

        public static Enum GetRandom(this IEnumerable<Enum> value)
        {
            var enumerable = Enum.GetValues(value.GetType()).Cast<Enum>().ToArray();
            return enumerable.ElementAt(new Random().Next(0, enumerable.Length));
        }
    }
}