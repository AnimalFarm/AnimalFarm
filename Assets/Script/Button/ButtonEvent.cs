using UnityEngine;
using System.Collections;

//노승현, 각각의 버튼이 눌렸을때 실행되는 스크립트

public class ButtonEvent : MonoBehaviour {

    public GameObject loadingPanel, characterPopUp, gameStartPopUp; // 각 버튼의 팝업;
    public GameObject yesOrno; // 예아니요 팝업
    public UILabel yesOrnoMessage;
    public GameObject loadingBear, loadingRabbit, loadingPanda, loadingDog; // 각각의 라운드별 로딩창
    public enum boss { bear = 1, dog, rabbit, panda};
    public static int BOSS;
    string roundName = null;

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
                break;
            case "게임을 시작하시겠습니까?":
                RoundChoice(roundName);
                break;
        }
        obj.SetActive(false);
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
