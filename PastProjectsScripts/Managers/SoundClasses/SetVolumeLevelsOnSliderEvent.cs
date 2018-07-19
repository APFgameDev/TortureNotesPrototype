using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetVolumeLevelsOnSliderEvent : MonoBehaviour
{
    [SerializeField]
    Slider m_musicSlider;

    [SerializeField]
    Slider m_soundEffectSlider;

    [SerializeField]
    Slider m_dialogueSlider;


    public void OnMusicSliderValueChange()
    {
        float volume = m_musicSlider.value;
        if (volume <= -40)
            volume = -80;

        NS_Sound.SoundController.SetVolumeLevel(NS_Sound.MixerGroups.MUSIC, volume);
    }

    public void OnSoundEffectSliderValueChange()
    {
        float volume = m_soundEffectSlider.value;
        if (volume <= -40)
            volume = -80;

        NS_Sound.SoundController.SetVolumeLevel(NS_Sound.MixerGroups.SOUNDEFFECT, volume);
    }

    public void OnDialogueSliderValueChange()
    {
        float volume = m_dialogueSlider.value;
        if (volume <= -40)
            volume = -80;

        NS_Sound.SoundController.SetVolumeLevel(NS_Sound.MixerGroups.DIALOG, volume);
    }
}
