using UnityEngine;

namespace USHER
{
    public static class SoundUtils
    {
        public static float GetDecibelNormalized(float dB)
        {
            return Mathf.Pow(10f, dB / 20f);
        }

        public static float GetDecibel(float value01)
        {
            return Mathf.Log10(Mathf.Clamp(value01, 0.0001f, 1f)) * 20f;
        }
    }
}