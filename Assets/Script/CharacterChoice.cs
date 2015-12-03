using UnityEngine;
using System.Collections;

public class CharacterChoice : MonoBehaviour {

    public GameObject bear, rabbit, panda, dog, penguin;
    public UIGrid CharacterGrid;
    public bool bearChoice = false, rabbitChoice = false, pandaChoice = false, dogChoice = false, penguinChoice = false;

    void Awake()
    {
	
	}
	void Update () 
    {
	
	}
    public void ChoiceCharacter(GameObject obj)
    {
        switch (obj.name)
        {
            case "CharacterItem": // 곰
                bear.SetActive(true);
                break;
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
}
