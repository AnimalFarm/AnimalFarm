using UnityEngine;
using System.Collections;

// 노승현, 로비 버튼 이벤트 스크립트
public class RobbyButton : MonoBehaviour
{

    public UIButton ui_gameStart, ui_character, ui_shop; // 하단 버튼 게임시작,캐릭터,상점,랭킹
    public GameObject optionPopUp, finishPopUp, shopPopUp, characterPopUp, gameStartPopUp; // 각 버튼의 팝업
    public GameObject shop_Character, shop_Gem, shop_Coin; // 상점을 열면 우선 보여주는 화면
    public GameObject shop_Panel, round_Panel; // 상점 , 게임시작 페널
    public bool stateCheck = false;
    public bool popUpCheck = false;
    int count = 0;

    public void OnOffButton(GameObject g)
    {
        count++;
        stateCheck = !stateCheck;
        popUpCheck = !popUpCheck;

        if (stateCheck == true)
        {
            ui_gameStart.enabled = false;
        }
        else
        {
            ui_gameStart.enabled = true;
        }

        shopPopUp.SetActive(false);
        optionPopUp.SetActive(false);
        finishPopUp.SetActive(false);
        characterPopUp.SetActive(false);
        gameStartPopUp.SetActive(false);
        shop_Panel.SetActive(false);
        round_Panel.SetActive(false);

        if (count % 2 == 0) popUpCheck = true;

        switch (g.name)
        {
            case "Coinplus":
                shopPopUp.SetActive(popUpCheck);
                shop_Panel.SetActive(popUpCheck);
                shop_Coin.GetComponentInChildren<UIToggle>().value = true;
                break;
            case "Gemplus":
                shopPopUp.SetActive(popUpCheck);
                shop_Panel.SetActive(popUpCheck);
                shop_Gem.GetComponentInChildren<UIToggle>().value = true;
                break;
            case "Option":
                optionPopUp.SetActive(popUpCheck);
                break;
            case "Close":
                finishPopUp.SetActive(popUpCheck);
                break;
            case "Shop":
                shopPopUp.SetActive(popUpCheck);
                shop_Panel.SetActive(popUpCheck);
                shop_Character.GetComponentInChildren<UIToggle>().value = true;
                break;
            case "Character":
                characterPopUp.SetActive(popUpCheck);
                break;
            case "Start":
                gameStartPopUp.SetActive(popUpCheck);
                round_Panel.SetActive(popUpCheck);
                break;
        }
        if (count % 2 == 0)
        {
            popUpCheck = false;
            count = 0;
        }
    }
}
