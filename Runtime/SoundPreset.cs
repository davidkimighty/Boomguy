using UnityEngine;
using UnityEngine.Audio;

namespace USHER
{
    [CreateAssetMenu(fileName = "Sound", menuName = "USHER/Preset/Sound")]
    public class SoundPreset : ScriptableObject
    {
        public AudioClip Clip;
        public AudioMixerGroup AudioMixerGroup;
        [Range(0f, 1f)] public float Volume = 1f;
        [Range(-3f, 3f)] public float Pitch = 1f;
    }
}
