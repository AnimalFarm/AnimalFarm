using UnityEngine;
using System.Collections;

public class CharacterChoice : MonoBehaviour {

    public GameObject bear, rabbit, panda, dog, penguin;
    public UIGrid CharacterGrid;
    public bool bearChoice = false, rabbitChoice = false, pandaChoice = false, dogChoice = false, penguinChoice = false;
    public enum player { user_bear = 1, user_dog, user_panda, user_penguin, user_rabbit};
    public static int PLAYER_CHOICE;

    void Awake()
    {
	
	}
	void Update () 
    {
	
	}
    public void Character(GameObject obj)
    {
        switch (obj.name)
        {
            case "CharacterItem (1)": // 개
                dog.SetActive(true);
                break;
            case "CharacterItem (2)": // 판다
                panda.SetActive(true);
                break;
            case "CharacterItem (3)": // 펭귄
                penguin.SetActive(true);
                break;
            case "CharacterItem (4)": // 토끼
                rabbit.SetActive(true);
                break;
        }
        CharacterGrid.Reposition();
    }
    public void ChoiceCharacter(GameObject obj)
    {
        switch(obj.name)
        {
            case "Character": // 곰
                PLAYER_CHOICE = (int)player.user_bear;
                break;
            case "Character (1)": // 개
                PLAYER_CHOICE = (int)player.user_dog;
                break;
            case "Character (2)": // 판다
                PLAYER_CHOICE = (int)player.user_panda;
                break;
            case "Character (3)": // 펭귄
                PLAYER_CHOICE = (int)player.user_penguin;
                break;
            case "Character (4)": // 토끼
                PLAYER_CHOICE = (int)player.user_rabbit;
                break;
        }
    }
}
