using UnityEngine;
using System.Collections;

public class StartImange : MonoBehaviour {
    public float delayTime = 0;
    private float timer;
    public GameObject backGround;
    public GameObject login;
    public GameObject faceBook;
    public GameObject google;
    public GameObject loginBorder;

	void Awake () {
        timer = delayTime;
	}
	 
	void Update () {
        timer += Time.deltaTime; // 오진수 시간 증가
        if(timer >= 0 && timer <2) // 오진수 로그인 배경 이미지 증가
        {
            backGround.GetComponent<UISprite>().color = new Color(1f, 1f, 1f, timer * 0.5f);
        }
        if (timer >= 2 && timer < 4) // 오진수 로그인 배경 이미지 감소
        {
            backGround.GetComponent<UISprite>().color = new Color(1f, 1f, 1f, (4 - timer) * 0.5f);
        }
        if (timer >= 4 && timer < 6) // 오진수 로그인 배경 이미지 증가 , 이미지 체인지
        {
            backGround.GetComponent<UISprite>().spriteName = "startImage";
            backGround.GetComponent<UISprite>().color = new Color(1f, 1f, 1f, (timer - 4) * 0.5f);
        }
        if (timer >= 6 && timer < 7) //오진수 로그인 버튼 나옴
        {
            login.SetActive(true);
        }
        
	}
    public void OpenLoginBorder() // 오진수 구글 페이스북 연동 버튼 나옴
    {
        loginBorder.SetActive(true);
        login.SetActive(false);
    }
    public void StartView() // 오진수 처음 화면을 보여줌(로그인버튼)
    {
        loginBorder.SetActive(false);
        login.SetActive(true);
    }
    public void nextScene() // 오진수 다음씬으로 넘어감
    {
        Application.LoadLevel("Lobby");
    }
}
