using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace Boomguy
{
    public enum SoundActions { Play, Pause, Stop }
    
    public class AudioEmitter : MonoBehaviour
    {
        private AudioSource _audioSource;
        private IObjectPool<AudioEmitter> _pool;
        private bool _isPlaying;
        private IEnumerator _waitCoroutine;

        public bool IsPlaying => _isPlaying;

        public void Initialize(IObjectPool<AudioEmitter> pool, float minDistance, float maxDistance)
        {
            _pool = pool;
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.rolloffMode = AudioRolloffMode.Custom;
            _audioSource.minDistance = minDistance;
            _audioSource.maxDistance = maxDistance;
        }

        public void Release()
        {
            _audioSource.Stop();
            _isPlaying = false;
            _waitCoroutine = null;
            _pool.Release(this);
        }
        
        public void Play(AudioPreset preset, bool loop)
        {
            if (_waitCoroutine != null)
                StopCoroutine(_waitCoroutine);
            
            if (_audioSource.isPlaying)
                _audioSource.Stop();
            
            SetupClip(preset, loop);
            _audioSource.Play();
            _isPlaying = true;
        }
        
        public void PlayAndRelease(AudioPreset preset)
        {
            if (_waitCoroutine != null)
                StopCoroutine(_waitCoroutine);
            
            if (_audioSource.isPlaying)
                _audioSource.Stop();
            
            SetupClip(preset);
            _audioSource.Play();
            _isPlaying = true;
            
            _waitCoroutine = WaitForPlaying(Release);
            StartCoroutine(_waitCoroutine);
        }
        
        public void PlayOneShot(AudioPreset preset)
        {
            _audioSource.PlayOneShot(preset.Clip, preset.Volume);
        }

        public void SetupClip(AudioPreset preset, bool loop = false)
        {
            _audioSource.clip = preset.Clip;
            _audioSource.outputAudioMixerGroup = preset.AudioMixerGroup;
            _audioSource.volume = preset.Volume;
            _audioSource.pitch = preset.Pitch;
            _audioSource.spatialBlend = preset.SpatialBlend;
            _audioSource.loop = loop;
            if (preset.SpacialCurve.length > 1)
                _audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, preset.SpacialCurve);
            else
                _audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        }

        public void SetPlayMode(SoundActions mode)
        {
            if (_audioSource.clip == null) return;

            switch (mode)
            {
                case SoundActions.Play: _audioSource.Play(); break;
                case SoundActions.Pause: _audioSource.Pause(); break;
                case SoundActions.Stop: _audioSource.Stop(); break;
            }
            _isPlaying = (int)mode == 0;
        }
        
        public void Mute(bool state)
        {
            if (_audioSource.clip == null) return;
            
            _audioSource.mute = state;
        }

        private IEnumerator WaitForPlaying(Action done)
        {
            while (_audioSource.isPlaying)
                yield return null;
            done?.Invoke();
        }
    }
}
