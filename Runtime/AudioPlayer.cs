using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

namespace Boomguy
{
    public class AudioPlayer
    {
        private AudioMixer _audioMixer;
        private ObjectPool<AudioEmitter> _pool;
        private float _maxDistance = 100f;

        public AudioPlayer(AudioMixer mixer)
        {
            _audioMixer = mixer;
        }

        public void Initialize(int defaultCapacity, int maxCapacity, float maxDistance = 100f)
        {
            if (_pool != null)
                _pool.Dispose();
            _maxDistance = maxDistance;

            _pool = new ObjectPool<AudioEmitter>(CreatePoolItem, OnTakeFromPool, OnReturnToPool, OnDestroyPoolItem,
                false, defaultCapacity, maxCapacity);
        }

        public AudioEmitter Play(AudioPreset audioPreset, bool loop = false)
        {
            AudioEmitter audio = _pool.Get();
            audio.Play(audioPreset, loop);
            return audio;
        }

        public void PlayOnce(AudioPreset audioPreset)
        {
            AudioEmitter audio = _pool.Get();
            audio.PlayAndRelease(audioPreset);
        }

        public AudioEmitter GetaudioPlayer()
        {
            return _pool.Get();
        }

        public void SetVolume(string groupName, float value01)
        {
            _audioMixer.SetFloat(groupName, SoundUtils.GetDecibel(value01));
        }

        private AudioEmitter CreatePoolItem()
        {
            AudioEmitter audio = new GameObject().AddComponent<AudioEmitter>();
            audio.gameObject.name = $"Audio{audio.gameObject.GetEntityId()}";
            audio.Initialize(_pool, 0.5f, _maxDistance);
            return audio;
        }

        private void OnTakeFromPool(AudioEmitter audio)
        {
            audio.gameObject.SetActive(true);
        }
        
        private void OnReturnToPool(AudioEmitter audio)
        {
            audio.gameObject.SetActive(false);
        }
        
        private void OnDestroyPoolItem(AudioEmitter audio)
        {
            GameObject.Destroy(audio.gameObject);
        }
    }
}
