using UnityEngine;
using System.Collections;

// 노승현 코루틴 함수 변경

public class TitleScreen : MonoBehaviour
{

    public float delayTime = 0;
    private float timer;
    public GameObject login;
    public UISprite bg;

	void Awake ()
    {
        timer = delayTime;
        StartCoroutine(Loading());
	}
    IEnumerator Loading()
    {
        while(true)
        {
            timer += Time.deltaTime; //  시간 증가
            if (timer >= 0 && timer < 2) //  로그인 배경 알파값 증가
            {
                bg.color = new Color(1f, 1f, 1f, timer * 0.5f);
            }
            if (timer >= 2 && timer < 4) //  로그인 배경 알파값 감소
            {
                bg.color = new Color(1f, 1f, 1f, (4 - timer) * 0.5f);
            }
            if (timer >= 4 && timer < 6) //  로그인 배경 알파값 증가 , 이미지 체인지
            {
                bg.spriteName = "startImage";
                bg.color = new Color(1f, 1f, 1f, (timer - 4) * 0.5f);
            }
            if (timer >= 6 && timer < 7) // 로그인 버튼 나옴
            {
                login.SetActive(true);
                yield break;
            }
            yield return null;
        }
    }
}
