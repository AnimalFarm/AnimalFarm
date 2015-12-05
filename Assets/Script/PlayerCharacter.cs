using UnityEngine;
using System.Collections;

public class PlayerCharacter : MonoBehaviour {

    public GameObject user_Bear, user_rabbit, user_panda, user_dog, user_penguin;

    
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
                break;
            case 2:
                user_dog.SetActive(true);
                break;
            case 3:
                user_panda.SetActive(true);
                break;
            case 4:
                user_penguin.SetActive(true);
                break;
            case 5:
                user_rabbit.SetActive(true);
                break;
        }
    }
}
