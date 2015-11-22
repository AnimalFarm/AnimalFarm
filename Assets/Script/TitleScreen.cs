using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour
{

    public float delayTime = 0;
    private float timer;
    public GameObject backGround,login;
    public UISprite bg;

	void Awake ()
    {
        timer = delayTime;
        bg = backGround.GetComponent<UISprite>();
	}
	 
	void Update ()
    {
        timer += Time.deltaTime; // 오진수 시간 증가
        if(timer >= 0 && timer <2) // 오진수 로그인 배경 알파값 증가
        {
            bg.color = new Color(1f, 1f, 1f, timer * 0.5f);
        }
        if (timer >= 2 && timer < 4) // 오진수 로그인 배경 알파값 감소
        {
            bg.color = new Color(1f, 1f, 1f, (4 - timer) * 0.5f);
        }
        if (timer >= 4 && timer < 6) // 오진수 로그인 배경 알파값 증가 , 이미지 체인지
        {
            bg.spriteName = "startImage";
            bg.color = new Color(1f, 1f, 1f, (timer - 4) * 0.5f);
        }
        if (timer >= 6 && timer < 7) //오진수 로그인 버튼 나옴
        {
            login.SetActive(true);
        }
	}
}
