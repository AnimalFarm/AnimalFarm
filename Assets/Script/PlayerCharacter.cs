using UnityEngine;
using System.Collections;

public class PlayerCharacter : MonoBehaviour {

    public GameObject user_Bear, user_rabbit, user_panda, user_dog, user_penguin;
    public UISprite PlayerHpBar;
    
    void Awake()
    {
        CreatePlayer();
    }
	void Update () 
    {
	}
    public void CreatePlayer()
    {
        switch(CharacterChoice.PLAYER_CHOICE)
        {
            case 1:
                user_Bear.SetActive(true);
                PlayerHpBar.spriteName = "BearCondition";
                break;
            case 2:
                user_dog.SetActive(true);
                PlayerHpBar.spriteName = "DogCondition";
                break;
            case 3:
                user_panda.SetActive(true);
                PlayerHpBar.spriteName = "PandaCondition";
                break;
            case 4:
                user_penguin.SetActive(true);
                PlayerHpBar.spriteName = "PenguinCondition";
                break;
            case 5:
                user_rabbit.SetActive(true);
                PlayerHpBar.spriteName = "RabbitCondition";
                break;
        }
    }
}
