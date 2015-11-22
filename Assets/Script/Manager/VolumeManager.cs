using UnityEngine;
using System.Collections;

// 노승현 사운드 컨트롤 스크립트

public class VolumeManager : MonoBehaviour {

    public AudioSource volumeControl;
    public UISlider bgSoundBar;
    public UILabel bgSizeFont;
    public bool soundCheck = true;

	
	void Awake () 
    {
        volumeControl.volume = 1.0f;
	}
    
    public void BGsoundSize() // 사운드 소리 크기
    {
        int size = (int)(bgSoundBar.value * 100);
        bgSizeFont.text = size.ToString();

        if (!soundCheck) return;
        volumeControl.volume = bgSoundBar.value;
    }
    public void SoundOnOff()
    {
        soundCheck = !soundCheck;
        if (soundCheck == true)
        {
            volumeControl.volume = bgSoundBar.value;
        }
        else
            volumeControl.volume = 0;
    }

}
