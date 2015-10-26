using UnityEngine;
using System.Collections;

public class RobbyButton : MonoBehaviour {

    public GameObject roomMake, roomSearch, shop, guild, friend, character;
    public GameObject coinPlus, jewelPlus, mail, set;
    public GameObject room;
    public GameObject creatRoom, roomBorder, shopMenubor, characterShopUIPanel, setOption, Maliborder;
    public GameObject modeling;

    public bool modelChack;
	// Use this for initialization
    void Awake()
    {
        modelChack = false;
	}
	
	// Update is called once per frame
    public void RoomMake() // 오진수 방만들기
    {
        creatRoom.SetActive(true);
        StopButton();
    }
    public void RoomBorder() // 오진수 방찾기
    {
        roomBorder.SetActive(true);
        StopGameObjectButton();
    }
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
    public void OpenRoom() // 오진수 로비방 열기
    {
        creatRoom.SetActive(false);
        room.SetActive(true);
        StopGameObjectButton();
    }
    public void ModelOpen() // 오진수 모델 띄움
    {
        modelChack = true;
        StartView();
    }
    public void StopButton() // 오진수 GameObject UIbutton만 끄기
    {
        roomMake.GetComponent<UIButton>().enabled = false;
        roomSearch.GetComponent<UIButton>().enabled = false;
        shop.GetComponent<UIButton>().enabled = false;
        guild.GetComponent<UIButton>().enabled = false;
        friend.GetComponent<UIButton>().enabled = false;
        character.GetComponent<UIButton>().enabled = false;
        coinPlus.GetComponent<UIButton>().enabled = false;
        jewelPlus.GetComponent<UIButton>().enabled = false;
        mail.GetComponent<UIButton>().enabled = false;
        set.GetComponent<UIButton>().enabled = false;
        modeling.SetActive(false);
    }
    public void StopGameObjectButton() // 오진수 RobbyButton GameObject 전부 끄기
    {

        roomMake.SetActive(false);
        roomSearch.SetActive(false);
        shop.SetActive(false);
        guild.SetActive(false);
        friend.SetActive(false);
        character.SetActive(false);
        modeling.SetActive(false);
    }
    public void StartView() // 오진수 처음 시작으로 초기화
    {
        roomMake.GetComponent<UIButton>().enabled = true;
        roomSearch.GetComponent<UIButton>().enabled = true;
        shop.GetComponent<UIButton>().enabled = true;
        guild.GetComponent<UIButton>().enabled = true;
        friend.GetComponent<UIButton>().enabled = true;
        character.GetComponent<UIButton>().enabled = true;
        coinPlus.GetComponent<UIButton>().enabled = true;
        jewelPlus.GetComponent<UIButton>().enabled = true;
        mail.GetComponent<UIButton>().enabled = true;
        set.GetComponent<UIButton>().enabled = true;

        roomMake.SetActive(true);
        roomSearch.SetActive(true);
        shop.SetActive(true);
        guild.SetActive(true);
        friend.SetActive(true);
        character.SetActive(true);

        room.SetActive(false);
        Maliborder.SetActive(false);
        setOption.SetActive(false);
        shopMenubor.SetActive(false);
        characterShopUIPanel.SetActive(false);
        roomBorder.SetActive(false);
        creatRoom.SetActive(false);
        if (modelChack == true) // 오진수 캐릭터가 구입되었을때 출력됨
        {
            modeling.SetActive(true);
        }
    }
}
