using UnityEngine;
using System.Collections;

public class PlayerHpText : MonoBehaviour {

    public UILabel HpText;
    public PlayerCharacter Player;
    float PlayerHp;

	// Use this for initialization
    void Awake()
    {
       
	}
	
	void Update () {
        HpText.text = string.Format("{0:0}", Player.user_rabbit.GetComponent<PlayerAttack>().Hp);
        switch (CharacterChoice.PLAYER_CHOICE)
        {
            case 1:
                HpText.text = string.Format("{0:0}", Player.user_Bear.GetComponent<PlayerAttack>().Hp);
                break;
            case 2:
                HpText.text = string.Format("{0:0}", Player.user_dog.GetComponent<PlayerAttack>().Hp);
                break;
            case 3:
                HpText.text = string.Format("{0:0}", Player.user_panda.GetComponent<PlayerAttack>().Hp);
                break;
            case 4:
                HpText.text = string.Format("{0:0}", Player.user_penguin.GetComponent<PlayerAttack>().Hp);
                break;
            case 5:
                HpText.text = string.Format("{0:0}", Player.user_rabbit.GetComponent<PlayerAttack>().Hp);
                break;
        }
        
	}
}
