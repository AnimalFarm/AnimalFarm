﻿using UnityEngine;
using System.Collections;

// 노승현, 로비 버튼 이벤트 스크립트
public class LobbyButton : MonoBehaviour
{
    public UIPanel lobbyPanel; // 로비 패널
    public UIButton ui_gameStart, ui_character, ui_shop; // 하단 버튼 게임시작,캐릭터,상점,랭킹
    public GameObject optionPopUp, finishPopUp, shopPopUp, characterPopUp, gameStartPopUp; // 각 버튼의 팝업
    public UIScrollView ui_ShopPanel, ui_CharacterPanel;// 상점 토글 패널
    public UIToggle shop_Character, shop_Gem, shop_Coin; // 상점을 열면 우선 보여주는 화면

    
    void Update()
    {
        if (!lobbyPanel.isActiveAndEnabled)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (shopPopUp.activeSelf)
            {
                shopPopUp.SetActive(false);
            }
            else if (optionPopUp.activeSelf)
            {
                optionPopUp.SetActive(false);
            }
            else if (finishPopUp.activeSelf)
            {
                finishPopUp.SetActive(false);
            }
            else if (characterPopUp.activeSelf)
            {
                characterPopUp.SetActive(false);
            }
            else if (gameStartPopUp.activeSelf)
            {
                gameStartPopUp.SetActive(false);
            }
            else
            {
                finishPopUp.SetActive(true);
            }
        }
        if (shopPopUp.activeSelf || characterPopUp.activeSelf)
        {
            ui_gameStart.enabled = false;
            ui_character.enabled = false;
            ui_shop.enabled = false;
        }
        else
        {
            ui_gameStart.enabled = true;
            ui_character.enabled = true;
            ui_shop.enabled = true;
        }
    }
    public void OnOffButton(GameObject g)
    {
        shopPopUp.SetActive(false);
        optionPopUp.SetActive(false);
        finishPopUp.SetActive(false);
        characterPopUp.SetActive(false);
        gameStartPopUp.SetActive(false);

        switch (g.name)
        {
            case "Coinplus":
                shopPopUp.SetActive(true);
                ui_ShopPanel.SetDragAmount(0, 0, false);
                shop_Coin.value = true;
                break;
            case "Gemplus":
                shopPopUp.SetActive(true);
                ui_ShopPanel.SetDragAmount(0, 0, false);
                shop_Gem.value = true;
                break;
            case "Shop":
                shopPopUp.SetActive(true);
                ui_ShopPanel.SetDragAmount(0, 0, false);
                shop_Character.value = true;
                break;
            case "Option":
                optionPopUp.SetActive(true);
                break;
            case "Close":
                finishPopUp.SetActive(true);
                break;
            case "Character":
                characterPopUp.SetActive(true);
                ui_CharacterPanel.SetDragAmount(0, 0, false);
                break;
            case "Start":
                gameStartPopUp.SetActive(true);
                break;
        }
    }

    public void OnOffPopUp(GameObject g)
    {
        g.SetActive(false);
    }
    public void SetToggle()
    {
        ui_ShopPanel.SetDragAmount(0, 0, false);
    }
    public void EndGame()
    {
        Application.Quit();
    }
    void OnApplicationQuit()
    {
        GPGSMng.GetInstance.LogoutGPGS(); //노승현, 로그아웃
    }
}
