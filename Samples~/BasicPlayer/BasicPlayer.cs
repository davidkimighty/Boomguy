using UnityEngine;
using USHER;

public class BasicPlayer : MonoBehaviour
{
    public SoundController SoundController;
    public SoundPreset[] KeyboardSounds;

    [ContextMenu("PlayKeySound")]
    public void PlayKeySound()
    {
        SoundController.PlayRandom(KeyboardSounds, false);
    }
}
