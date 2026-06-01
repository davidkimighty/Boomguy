using UnityEngine;
using UnityEngine.UI;
using Boomguy;
using UnityEngine.Audio;

public class SimpleAudioPlayer : MonoBehaviour
{
    public AudioMixer AudioMixer;
    public AnimationCurve SpacialCurve;
    public AudioPreset BackgroundMusic;
    public AudioPreset[] KeyboardSounds;

    public Slider VolumeSlider;

    private AudioPlayer _audioPlayer;
    private AudioEmitter _backgroundAudio;

    private void Start()
    {
        _audioPlayer = new AudioPlayer(AudioMixer);
        _audioPlayer.Initialize(10, 30);
        _audioPlayer.SetVolume("MasterVolume", VolumeSlider.value);
        
        _backgroundAudio = _audioPlayer.Play(BackgroundMusic, true);
    }

    public void PlayPauseBackground()
    {
        _backgroundAudio.SetPlayMode(_backgroundAudio.IsPlaying ? SoundActions.Pause : SoundActions.Play);
    }

    public void StopBackground()
    {
        _backgroundAudio.SetPlayMode(SoundActions.Stop);
    }

    public void ChangeVolume(float volume)
    {
        _audioPlayer.SetVolume("MasterVolume", volume);
    }
}
