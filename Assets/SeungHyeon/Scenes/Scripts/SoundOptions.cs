using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundOptions : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider BgmSlider;
    public Slider SfxSlider;
    public Slider GeneralSlider;
    public Slider QualitySlider;

    public void Update()
    {
        int qualityLevel = (int)(QualitySlider.value * QualitySettings.names.Length);
        QualitySettings.SetQualityLevel(qualityLevel, true);
            }

    public void SetBgmVolume()
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(BgmSlider.value) * 20);
    }

    public void SetSFXVolume()
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(SfxSlider.value) * 20);
    }
    public void SetGeneralVolume()
    {
        audioMixer.SetFloat("Master", Mathf.Log10(SfxSlider.value) * 20);
    }





}
