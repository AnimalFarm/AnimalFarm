using UnityEngine;
using System.Collections;

//노승현, 각각의 버튼이 눌렸을때 실행되는 스크립트

public class ButtonEvent : MonoBehaviour {
    public GameObject roundPanel;
    public GameObject loadingBear, loadingRabbit, loadingPanda, loadingDog; // 각각의 라운드별 로딩창

	
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
                break;
            case "Round (1)":
                loadingDog.SetActive(true);
                break;
            case "Round (2)":
                loadingRabbit.SetActive(true);
                break;
            case "Round (3)":
                loadingPanda.SetActive(true);
                break;
        }
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(2);
        Application.LoadLevel("Map");
    }
}
