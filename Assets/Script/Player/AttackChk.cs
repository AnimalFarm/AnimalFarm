using UnityEngine;
using System.Collections;

public class AttackChk : MonoBehaviour {

    public GameObject GameObject;    //이승환//보스 오브잭트
    public GameObject Player;   //이승환//플레이어 오브잭트
    
    GameObject Enemy;
    float timer=0;
    float BossHp,PlayerDamage;

    void Awake()
    {
        switch (ButtonEvent.BOSS)
        {
            case 1:
                Enemy = GameObject.GetComponent<Boss>().bear;
                BossHp = GameObject.GetComponent<Boss>().bear.GetComponent<Eenemymove>().Hp;
                PlayerDamage = Player.GetComponent<PlayerAttack>().damig;
                break;
            case 2:
                Enemy = GameObject.GetComponent<Boss>().dog;
                BossHp = GameObject.GetComponent<Boss>().dog.GetComponent<Eenemymove>().Hp;
                PlayerDamage = Player.GetComponent<PlayerAttack>().damig;
                break;
            case 3:
                Enemy = GameObject.GetComponent<Boss>().rabbit;
                BossHp = GameObject.GetComponent<Boss>().rabbit.GetComponent<Eenemymove>().Hp;
                PlayerDamage = Player.GetComponent<PlayerAttack>().damig;
                break;
            case 4:
                Enemy = GameObject.GetComponent<Boss>().panda;
                BossHp = GameObject.GetComponent<Boss>().panda.GetComponent<Eenemymove>().Hp;
                PlayerDamage = Player.GetComponent<PlayerAttack>().damig;
                break;
        }     
    }
    void Update()
    {
        timer += Time.deltaTime;
       // Player.GetComponent<BoxCollider>().enabled = false;
    }
     void OnTriggerEnter(Collider order)//이승환//오브젝트와 콜리더가 처음 충돌했을 때 한번 한번호출
     {
         if (order.tag == "Enemy" && order.tag != "Player")
        {
            if (timer > 0.5f)
            {
                Enemy.GetComponent<Eenemymove>().test.fillAmount -= PlayerDamage / BossHp;
                Enemy.GetComponent<Eenemymove>().Hp -= PlayerDamage;
                timer = 0;
            }
        } 
     }
}

