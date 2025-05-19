using UnityEngine;
using UnityEngine.UI;
using USHER;

public class SimpleAudioPlayer : MonoBehaviour
{
    public SoundController SoundController;
    public SoundPreset BackgroundMusic;
    public SoundPreset[] KeyboardSounds;

    public Slider VolumeSlider;

    private SoundPlayer _backgroundPlayer;

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
