using UnityEngine;
using System.Collections;

// 노승현 사운드 컨트롤 스크립트

public class VolumeManager : MonoBehaviour {

    public GameObject soundBar;
    public bool soundCheck = true;
    public AudioSource volumeControl;
    public UISlider bgSoundSize;
    public UILabel bgSizeFont;

	
	void Awake () 
    {
        volumeControl = GetComponent<AudioSource>();
        bgSoundSize = soundBar.GetComponent<UISlider>();
        volumeControl.volume = 1.0f;
	}
    
    public void BGsoundSize() // 사운드 소리 크기
    {
        int size = (int)(bgSoundSize.value * 100);
        bgSizeFont.text = size.ToString();

        if (!soundCheck) return;
        volumeControl.volume = bgSoundSize.value;
    }
    public void SoundOnOff()
    {
        soundCheck = !soundCheck;
        if (soundCheck == true)
        {
            volumeControl.volume = bgSoundSize.value;
        }
        else
            volumeControl.volume = 0;
    }

}
