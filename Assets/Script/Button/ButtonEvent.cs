using UnityEngine;
using System.Collections;

//노승현, 각각의 버튼이 눌렸을때 실행되는 스크립트

public class ButtonEvent : MonoBehaviour {
    public GameObject roundPanel;
    public GameObject loadingBear, loadingRabbit, loadingPanda, loadingDog; // 각각의 라운드별 로딩창
    public enum boss { bear = 1, dog = 2, rabbit = 3, panda = 4};
    public static int BOSS;

	
    void Awake()
    {
	
	}
	void Update () 
    {
	
	}
    public void GameStart(GameObject g)
    {
        roundPanel.SetActive(true);
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
