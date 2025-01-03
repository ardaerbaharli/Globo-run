using UnityEngine;

namespace Utilities
{
    public class Vibration
    {
        public static void Light()
        {
            if (!Config.IsVibrationOn) return;
#if UNITY_IOS
            VibrateIos(iOSHapticFeedback.iOSFeedbackType.ImpactLight);
#elif UNITY_ANDROID
        Native.Vibrate(1);
#endif
        }

        public static void Medium()
        {
            if (!Config.IsVibrationOn) return;
#if UNITY_IOS
            VibrateIos(iOSHapticFeedback.iOSFeedbackType.ImpactMedium);
#elif UNITY_ANDROID
        Native.Vibrate(2);
#endif
        }

        public static void Heavy()
        {
            if (!Config.IsVibrationOn) return;
#if UNITY_IOS
            VibrateIos(iOSHapticFeedback.iOSFeedbackType.ImpactHeavy);
#elif UNITY_ANDROID
        Native.Vibrate(3);
#endif
        }

        private static void VibrateIos(iOSHapticFeedback.iOSFeedbackType impact)
        {
            if (iOSHapticFeedback.Instance.IsSupported())
            {
                iOSHapticFeedback.Instance.Trigger(impact);
            }
            else
            {
                Handheld.Vibrate();
            }
        }

        public static void Vibrate(int level)
        {
#if UNITY_IOS
            VibrateIos(level);
#elif UNITY_ANDROID
        VibrateAnd(level);
#endif
        }

        private static void VibrateAnd(int level)
        {
            Native.Vibrate(level);
        }

        private static void VibrateIos(int level)
        {
            if (iOSHapticFeedback.Instance.IsSupported())
            {
                iOSHapticFeedback.Instance.Trigger((iOSHapticFeedback.iOSFeedbackType) level);
            }
            else
            {
                Handheld.Vibrate();
            }
        }

        public static bool IsHapticSupported()
        {
#if UNITY_IOS
            return iOSHapticFeedback.Instance.IsSupported();
#elif UNITY_ANDROID
        return Native.getSDKInt() >= 26;
#endif
            return false;
        }
    }
}