using UnityEngine;
using System.Collections;

//노승현, 각각의 버튼이 눌렸을때 실행되는 스크립트

public class ButtonEvent : MonoBehaviour {

    public GameObject roundPanel,loadingPanel;
    public GameObject yesOrno; // 예아니요 팝업
    public UILabel yesOrnoMessage;
    public GameObject loadingBear, loadingRabbit, loadingPanda, loadingDog; // 각각의 라운드별 로딩창
    public enum boss { bear = 1, dog, rabbit, panda};
    public static int BOSS;

    public void GameStart(GameObject g)
    {
        roundPanel.SetActive(true);
        if (CharacterChoice.PLAYER_CHOICE == 0)
        {
            yesOrno.SetActive(true);
            yesOrnoMessage.text = "캐릭터 메뉴에서 캐릭터를 선택하세요.";
            return;
        }
        loadingPanel.SetActive(true);
        switch(g.name)
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
}
