using UnityEngine;
using System.Collections;

// 노승현, 로비 버튼 이벤트 스크립트
public class RobbyButton : MonoBehaviour
{

    public UIButton ui_gameStart, ui_character, ui_shop; // 하단 버튼 게임시작,캐릭터,상점,랭킹
    public GameObject optionPopUp, finishPopUp, shopPopUp, characterPopUp, gameStartPopUp; // 각 버튼의 팝업
    public UIToggle shop_Character, shop_Gem, shop_Coin; // 상점을 열면 우선 보여주는 화면
    public bool stateCheck = false;
    public bool popUpCheck = false;
    int count = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (shopPopUp.activeSelf == true)
            {
                shopPopUp.SetActive(false);
            }
            else if (optionPopUp.activeSelf == true)
            {
                optionPopUp.SetActive(false);
            }
            else if (finishPopUp.activeSelf == true)
            {
                finishPopUp.SetActive(false);
            }
            else if (characterPopUp.activeSelf == true)
            {
                characterPopUp.SetActive(false);
            }
            else if (gameStartPopUp.activeSelf == true)
            {
                gameStartPopUp.SetActive(false);
            }
            else
            {
                finishPopUp.SetActive(true);
            }
        }
    }

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

        if (count % 2 == 0) popUpCheck = true;

        switch (g.name)
        {
            case "Coinplus":
                shopPopUp.SetActive(popUpCheck);
                shop_Coin.value = true;
                break;
            case "Gemplus":
                shopPopUp.SetActive(popUpCheck);
                shop_Gem.value = true;
                break;
            case "Option":
                optionPopUp.SetActive(popUpCheck);
                break;
            case "Close":
                finishPopUp.SetActive(popUpCheck);
                break;
            case "Shop":
                shopPopUp.SetActive(popUpCheck);
                shop_Character.value = true;
                break;
            case "Character":
                characterPopUp.SetActive(popUpCheck);
                break;
            case "Start":
                gameStartPopUp.SetActive(popUpCheck);
                break;
        }
        if (count % 2 == 0)
        {
            popUpCheck = false;
            count = 0;
        }
    }

    public void OnOffPopUp(GameObject g)
    {
        g.SetActive(false);
    }
    public void EndGame()
    {
        Application.Quit();
    }


}
