using UnityEngine;
using System.Collections;

public class BossAttack : MonoBehaviour {
    public PlayerCharacter GameObject;    //이승환//게임매니저 오브잭트
    public GameObject Boss;   //이승환//플레이어 오브잭트

    GameObject Player;


    float timer = 0;
    float PlayerHp, BossDamage;

    // Use this for initialization
    void Awake() {
        BossDamage = Boss.GetComponent<Eenemymove>().Damage;
        Player = GameObject.user_rabbit;
        PlayerHp = GameObject.user_rabbit.GetComponent<PlayerAttack>().Hp;
        switch (CharacterChoice.PLAYER_CHOICE)
        {
            case 1:
                Player = GameObject.user_Bear;
                PlayerHp = GameObject.user_Bear.GetComponent<PlayerAttack>().Hp;
                break;
            case 2:
                Player = GameObject.user_dog;
                PlayerHp = GameObject.user_dog.GetComponent<PlayerAttack>().Hp;
                break;
            case 3:
                Player = GameObject.user_panda;
                PlayerHp = GameObject.user_panda.GetComponent<PlayerAttack>().Hp;
                break;
            case 4:
                Player = GameObject.user_penguin;
                PlayerHp = GameObject.user_penguin.GetComponent<PlayerAttack>().Hp;
                break;
            case 5:
                Player = GameObject.user_rabbit;
                PlayerHp = GameObject.user_rabbit.GetComponent<PlayerAttack>().Hp;
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        // Player.GetComponent<BoxCollider>().enabled = false;
    }
    void OnTriggerEnter(Collider order)//이승환//오브젝트와 콜리더가 처음 충돌했을 때 한번 한번호출
    {
        if (order.tag == "Player" && order.tag != "Enemy")
        {
            if (timer > 0.5f)
            {
                Player.GetComponent<PlayerAttack>().Hpbar.fillAmount -= BossDamage / PlayerHp;
                Player.GetComponent<PlayerAttack>().Hp -= BossDamage;
                timer = 0;
                Debug.Log(BossDamage);
            }
        }
    }

}

