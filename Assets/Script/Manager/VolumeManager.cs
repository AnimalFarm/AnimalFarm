using UnityEngine;
using System.Collections;

// 노승현 사운드 컨트롤 스크립트

public class VolumeManager : MonoBehaviour {

    public AudioSource volumeControl; // 로비 배경음
    public AudioSource loginSound; // 로그인 사운드
    public UIPanel lobbyPanel, loadingPlanel; // 로비패널
    public UISlider bgSoundBar;
    public UILabel bgSizeFont;
    public bool soundCheck = true;
	
    void Update()
    {
        if (!loadingPlanel.isActiveAndEnabled) loginSound.enabled = false;
        if(lobbyPanel.isActiveAndEnabled && soundCheck) volumeControl.enabled = true;
        else volumeControl.enabled = false;
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
        if (soundCheck)
        {
            volumeControl.volume = bgSoundBar.value;
        }
        else
        {
            volumeControl.enabled = false;
        }
    }

}
