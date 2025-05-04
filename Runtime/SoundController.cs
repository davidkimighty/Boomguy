using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

namespace USHER
{
    public class SoundController : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private int _defaultCapacity = 10;
        [SerializeField] private int _maxCapacity = 100;
        
        private IObjectPool<SoundPlayer> _pool;

        private void Awake()
        {
            _pool = new ObjectPool<SoundPlayer>(CreatePoolItem, OnTakeFromPool, OnReturnToPool, OnDestroyPoolItem,
                false, _defaultCapacity, _maxCapacity);
        }

        public SoundPlayer Play(SoundPreset soundPreset, bool loop = false)
        {
            SoundPlayer soundPlayer = _pool.Get();
            soundPlayer.Play(soundPreset, loop);
            return soundPlayer;
        }

        public SoundPlayer PlayRandom(SoundPreset[] soundPresets, bool loop = false)
        {
            SoundPlayer soundPlayer = _pool.Get();
            SoundPreset randomPick = soundPresets[Random.Range(0, soundPresets.Length - 1)];
            soundPlayer.Play(randomPick, loop);
            return soundPlayer;
        }

        private SoundPlayer CreatePoolItem()
        {
            GameObject empty = new GameObject("SoundPlayer");
            empty.transform.SetParent(transform);
            SoundPlayer soundPlayer = empty.AddComponent<SoundPlayer>();
            soundPlayer.Initialize(_pool);
            return soundPlayer;
        }

        private void OnTakeFromPool(SoundPlayer soundPlayer)
        {
            soundPlayer.gameObject.SetActive(true);
        }
        
        private void OnReturnToPool(SoundPlayer soundPlayer)
        {
            soundPlayer.gameObject.SetActive(false);
        }
        
        private void OnDestroyPoolItem(SoundPlayer soundPlayer)
        {
            Destroy(soundPlayer.gameObject);
        }
    }
}
