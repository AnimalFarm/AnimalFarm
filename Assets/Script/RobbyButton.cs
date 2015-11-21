using UnityEngine;
using System.Collections;

public class RobbyButton : MonoBehaviour {

    public GameObject roomSearch, shop, profile, character;
    public GameObject coinPlus, jewelPlus, seting;
    public GameObject shopMenubor, characterShopUIPanel, setOption, Maliborder;
    public GameObject modeling;

    public bool modelChack;
	// Use this for initialization
    void Awake()
    {
        modelChack = false;
	}
	
	// Update is called once per fram
    public void Shop() // 오진수 상점 열기
    {
        shopMenubor.SetActive(true);
        characterShopUIPanel.SetActive(true);
        StopGameObjectButton();
    }
    public void MailButton() // 오진수 우편함 열기
    {
        Maliborder.SetActive(true);
        StopButton();
    }
    public void SetOpion() // 오진수 옵션 열기
    {
        setOption.SetActive(true);
        StopButton();
    }
    public void ModelOpen() // 오진수 모델 띄움
    {
        modelChack = true;
        StartView();
    }
    public void StopButton() // 오진수 GameObject UIbutton만 끄기
    {
        roomSearch.GetComponent<UIButton>().enabled = false;
        shop.GetComponent<UIButton>().enabled = false;
        character.GetComponent<UIButton>().enabled = false;
        coinPlus.GetComponent<UIButton>().enabled = false;
        jewelPlus.GetComponent<UIButton>().enabled = false;
        modeling.SetActive(false);
    }
    public void StopGameObjectButton() // 오진수 RobbyButton GameObject 전부 끄기
    {
        roomSearch.SetActive(false);
        shop.SetActive(false);
        profile.SetActive(false);
        character.SetActive(false);
        modeling.SetActive(false);
    }
    public void StartView() // 오진수 처음 시작으로 초기화
    {
        roomSearch.GetComponent<UIButton>().enabled = true;
        shop.GetComponent<UIButton>().enabled = true;
        character.GetComponent<UIButton>().enabled = true;
        coinPlus.GetComponent<UIButton>().enabled = true;
        jewelPlus.GetComponent<UIButton>().enabled = true;

        roomSearch.SetActive(true);
        shop.SetActive(true);
        profile.SetActive(true);
        character.SetActive(true);

        Maliborder.SetActive(false);
        setOption.SetActive(false);
        shopMenubor.SetActive(false);
        characterShopUIPanel.SetActive(false);
        if (modelChack == true) // 오진수 캐릭터가 구입되었을때 출력됨
        {
            modeling.SetActive(true);
        }
    }
}
