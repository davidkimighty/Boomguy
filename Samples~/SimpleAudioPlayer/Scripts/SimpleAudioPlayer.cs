using UnityEngine;
using UnityEngine.UI;
using Boomguy;

public class SimpleAudioPlayer : MonoBehaviour
{
    public AudioController SoundController;
    public AudioPreset BackgroundMusic;
    public AudioPreset[] KeyboardSounds;

    public Slider VolumeSlider;

    private AudioPlayer _backgroundPlayer;

    private void Start()
    {
        SoundController.SetVolume("MasterVolume", VolumeSlider.value);
        
        _backgroundPlayer = SoundController.Play(BackgroundMusic, true);
    }

    public void PlayPauseBackground()
    {
        _backgroundPlayer.SetPlayMode(_backgroundPlayer.IsPlaying ? SoundActions.Pause : SoundActions.Play);
    }

    public void StopBackground()
    {
        _backgroundPlayer.SetPlayMode(SoundActions.Stop);
    }

    public void ChangeVolume(float volume)
    {
        SoundController.SetVolume("MasterVolume", volume);
    }
}
