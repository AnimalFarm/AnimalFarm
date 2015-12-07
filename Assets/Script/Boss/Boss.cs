using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {
    public GameObject bear, rabbit, panda, dog;
    public GameObject gameState, gameeEnd;

    void Awake () 
    {
        CreateBoss();
	}
	
	void Update () 
    {
        if(bear.GetComponent<Eenemymove>().Hp <=0)
        {
            bear.SetActive(false);
            gameState.SetActive(false);
            gameeEnd.SetActive(true);
        }
	}

    public void CreateBoss()
    {
        switch (ButtonEvent.BOSS)
        {
            case 1:
                bear.SetActive(true);
                break;
            case 2:
                dog.SetActive(true);
                break;
            case 3:
                rabbit.SetActive(true);
                break;
            case 4:
                panda.SetActive(true);
                break;
        }     
    }

    public void LobbyScene()
    {
        Application.LoadLevel("Lobby");
        SetShop.gold += 100;
    }
}
