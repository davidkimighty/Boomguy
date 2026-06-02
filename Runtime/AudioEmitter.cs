using System;
using System.Threading;
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
        private bool _suspended;
        private CancellationTokenSource _waitCts;
        private CancellationTokenSource _fadeCts;

        public bool IsPlaying => _isPlaying;
        public float Volume => _audioSource.volume;

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
            CancelWait();
            CancelFade();
            _audioSource.Stop();
            _isPlaying = false;
            _suspended = false;
            _pool.Release(this);
        }
        
        public async Awaitable Play(AudioPreset preset, bool loop = false, bool release = false)
        {
            CancelWait();

            if (_audioSource.isPlaying)
                _audioSource.Stop();

            SetupClip(preset, loop);
            _audioSource.Play();
            _isPlaying = true;
            _suspended = false;

            if (!release) return;

            _waitCts = new CancellationTokenSource();
            var token = _waitCts.Token;
            try
            {
                while (_audioSource.isPlaying || _suspended)
                    await Awaitable.NextFrameAsync(token);
            }
            catch (OperationCanceledException)
            {
                return;
            }
            Release();
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
                case SoundActions.Play: _audioSource.Play(); _suspended = false; break;
                case SoundActions.Pause: _audioSource.Pause(); _suspended = true; break;
                case SoundActions.Stop: _audioSource.Stop(); _suspended = true; break;
            }
            _isPlaying = mode == SoundActions.Play;
        }

        public void SetVolume(float volume)
        {
            _audioSource.volume = volume;
        }

        public async Awaitable FadeVolumeAync(float targetVolume, float duration, bool release = false)
        {
            CancelFade();
            var cts = new CancellationTokenSource();
            _fadeCts = cts;
            var token = cts.Token;

            targetVolume = Mathf.Clamp01(targetVolume);
            bool completed = false;
            try
            {
                if (duration <= 0f)
                {
                    _audioSource.volume = targetVolume;
                }
                else
                {
                    float startVolume = _audioSource.volume;
                    float elapsed = 0f;
                    while (elapsed < duration)
                    {
                        elapsed += Time.deltaTime;
                        float t = Mathf.Clamp01(elapsed / duration);

                        _audioSource.volume = Mathf.Lerp(startVolume, targetVolume, t);
                        await Awaitable.NextFrameAsync(token);
                    }
                    _audioSource.volume = targetVolume;
                }
                completed = true;
            }
            catch (OperationCanceledException) { }
            finally
            {
                if (ReferenceEquals(_fadeCts, cts))
                {
                    cts.Dispose();
                    _fadeCts = null;
                }
            }

            if (release && completed)
                Release();
        }

        public void Mute(bool state)
        {
            if (_audioSource.clip == null) return;
            
            _audioSource.mute = state;
        }

        private void CancelWait()
        {
            if (_waitCts == null) return;

            _waitCts.Cancel();
            _waitCts.Dispose();
            _waitCts = null;
        }

        private void CancelFade()
        {
            if (_fadeCts == null) return;

            _fadeCts.Cancel();
            _fadeCts.Dispose();
            _fadeCts = null;
        }
    }
}
