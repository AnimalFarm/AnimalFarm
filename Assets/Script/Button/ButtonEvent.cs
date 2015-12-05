using UnityEngine;
using System.Collections;

//노승현, 각각의 버튼이 눌렸을때 실행되는 스크립트

public class ButtonEvent : MonoBehaviour {

    public UIToggle shop_Character, shop_Gem, shop_Coin; // 상점을 열면 우선 보여주는 화면
    public GameObject loadingPanel, characterPopUp, gameStartPopUp; // 각 버튼의 팝업;
    public GameObject yesOrno; // 예아니요 팝업
    public UILabel yesOrnoMessage;
    public GameObject loadingBear, loadingRabbit, loadingPanda, loadingDog; // 각각의 라운드별 로딩창
    public enum boss { bear = 1, dog, rabbit, panda};
    public static int BOSS;
    string roundName = null, getShop = null, getGem = null, getPay = null;
    GameObject shopCharacter;
    public SetShop setshop;

    public void GameStart(GameObject round)
    {
        yesOrno.SetActive(true);
        if (CharacterChoice.PLAYER_CHOICE == 0)
        {
            yesOrnoMessage.text = "캐릭터 메뉴에서 캐릭터를 선택하세요.";
        }
        else
        {
            roundName = round.name;
            yesOrnoMessage.text = "게임을 시작하시겠습니까?";
        }
    }
    public void GetCharacter(UILabel label, GameObject character)
    {
        getShop = label.text;
        shopCharacter = character;
        yesOrno.SetActive(true);
        yesOrnoMessage.text = "캐릭터를 구매 하시겠습니까?";
    }
    public void GetGold(UILabel label, UILabel pay)
    {
        getGem = label.text;
        getPay = pay.text;
        yesOrno.SetActive(true);
        yesOrnoMessage.text = "골드를 구매 하시겠습니까?";
    }
    public void Identity(UILabel message, GameObject obj)
    {
        switch (message.text)
        {
            case "게임을 정말 종료 하시겠습니까?":
                Application.Quit();
                break;
            case "캐릭터 메뉴에서 캐릭터를 선택하세요.":
                gameStartPopUp.SetActive(false);
                characterPopUp.SetActive(true);
                obj.SetActive(false);
                break;
            case "게임을 시작하시겠습니까?":
                RoundChoice(roundName);
                obj.SetActive(false);
                break;
            case "캐릭터를 구매 하시겠습니까?":
                int gold = int.Parse(getShop);
                if (gold <= SetShop.gold)
                {
                    setshop.BuyCharacter(getShop, shopCharacter);
                    obj.SetActive(false);
                }
                else
                {
                    message.text = "골드가 부족합니다.";
                }
                break;
            case "골드를 구매 하시겠습니까?":
                int gem = int.Parse(getGem);
                if (gem <= SetShop.gem)
                {
                    setshop.BuyGold(getGem, getPay);
                    obj.SetActive(false);
                }
                else
                {
                    message.text = "보석이 부족합니다.";
                }
                break;
            case "골드가 부족합니다.":
                shop_Coin.value = true;
                obj.SetActive(false);
                break;
            case "보석이 부족합니다.":
                shop_Gem.value = true;
                obj.SetActive(false);
                break;
        }
    }
    public void RoundChoice(string round)
    {
        loadingPanel.SetActive(true);
        switch (round)
        {
            case "Round":
                loadingBear.SetActive(true);
                BOSS = (int)boss.bear;
                break;
            case "Round (1)":
                loadingDog.SetActive(true);
                BOSS = (int)boss.dog;
                break;
            case "Round (2)":
                loadingRabbit.SetActive(true);
                BOSS = (int)boss.rabbit;
                break;
            case "Round (3)":
                loadingPanda.SetActive(true);
                BOSS = (int)boss.panda;
                break;
        }
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(2);
        Application.LoadLevel("Field");
    }
    void OnApplicationQuit()
    {
        GPGSMng.GetInstance.LogoutGPGS(); //노승현, 로그아웃
    }
}
