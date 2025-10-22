using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

namespace Boomguy
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private int _defaultCapacity = 10;
        [SerializeField] private int _maxCapacity = 100;
        
        private IObjectPool<AudioPlayer> _pool;

        private void Awake()
        {
            _pool = new ObjectPool<AudioPlayer>(CreatePoolItem, OnTakeFromPool, OnReturnToPool, OnDestroyPoolItem,
                false, _defaultCapacity, _maxCapacity);
        }

        public AudioPlayer Play(AudioPreset audioPreset, bool loop = false)
        {
            AudioPlayer audioPlayer = _pool.Get();
            audioPlayer.Play(audioPreset, loop);
            return audioPlayer;
        }

        public void PlayOnce(AudioPreset audioPreset)
        {
            AudioPlayer audioPlayer = _pool.Get();
            audioPlayer.PlayAndRelease(audioPreset);
        }

        public AudioPlayer GetaudioPlayer()
        {
            return _pool.Get();
        }

        public void SetVolume(string groupName, float value01)
        {
            _audioMixer.SetFloat(groupName, SoundUtils.GetDecibel(value01));
        }

        private AudioPlayer CreatePoolItem()
        {
            GameObject empty = new GameObject("AudioPlayer");
            empty.transform.SetParent(transform);
            AudioPlayer audioPlayer = empty.AddComponent<AudioPlayer>();
            audioPlayer.Initialize(_pool);
            return audioPlayer;
        }

        private void OnTakeFromPool(AudioPlayer audioPlayer)
        {
            audioPlayer.gameObject.SetActive(true);
        }
        
        private void OnReturnToPool(AudioPlayer audioPlayer)
        {
            audioPlayer.gameObject.SetActive(false);
        }
        
        private void OnDestroyPoolItem(AudioPlayer audioPlayer)
        {
            Destroy(audioPlayer.gameObject);
        }
    }
}
