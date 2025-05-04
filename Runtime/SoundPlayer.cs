using UnityEngine;
using UnityEngine.Pool;

namespace USHER
{
    public class SoundPlayer : MonoBehaviour
    {
        private AudioSource _audioSource;
        private IObjectPool<SoundPlayer> _pool;

        public void Initialize(IObjectPool<SoundPlayer> pool)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _pool = pool;
        }

        public void Release()
        {
            _audioSource.Stop();
            _pool.Release(this);
        }

        public void Play(SoundPreset preset, bool loop)
        {
            _audioSource.clip = preset.Clip;
            _audioSource.outputAudioMixerGroup = preset.AudioMixerGroup;
            _audioSource.volume = preset.Volume;
            _audioSource.pitch = preset.Pitch;
            _audioSource.loop = loop;
            _audioSource.Play();
        }

        public void Stop()
        {
            _audioSource.Stop();
        }

        public void Pause()
        {
            _audioSource.Pause();
        }
    }
}
