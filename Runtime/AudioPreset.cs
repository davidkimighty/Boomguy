using UnityEngine;
using UnityEngine.Audio;

namespace Boomguy
{
    [CreateAssetMenu(fileName = "Audio", menuName = "Boomguy/AudioPreset")]
    public class AudioPreset : ScriptableObject
    {
        public AudioClip Clip;
        public AudioMixerGroup AudioMixerGroup;
        [Range(0f, 1f)] public float Volume = 1f;
        [Range(-3f, 3f)] public float Pitch = 1f;
        [Range(0f, 1f)] public float SpatialBlend = 1f;
    }
}
