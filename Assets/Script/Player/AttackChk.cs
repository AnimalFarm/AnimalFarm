using UnityEngine;
using System.Collections;

public class AttackChk : MonoBehaviour {

    public Boss GameObject;    //이승환//게임매니저 오브잭트
    public GameObject Player;   //이승환//플레이어 오브잭트
    public GameObject AttackParticle;

    GameObject Enemy;
    float timer=0;
    float BossHp,PlayerDamage;

    void Awake()
    {
        PlayerDamage = Player.GetComponent<PlayerAttack>().damig;
        Enemy = GameObject.bear;
        BossHp = GameObject.bear.GetComponent<Eenemymove>().Hp;
        switch (ButtonEvent.BOSS)
        {
            case 1:
                Enemy = GameObject.bear;
                BossHp = GameObject.bear.GetComponent<Eenemymove>().Hp;
                break;
            case 2:
                Enemy = GameObject.dog;
                BossHp = GameObject.dog.GetComponent<Eenemymove>().Hp;
                break;
            case 3:
                Enemy = GameObject.rabbit;
                BossHp = GameObject.rabbit.GetComponent<Eenemymove>().Hp;
                break;
            case 4:
                Enemy = GameObject.panda;
                BossHp = GameObject.panda.GetComponent<Eenemymove>().Hp;
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
            AttackParticle.SetActive(true);
            AttackParticle.transform.position = order.transform.position;
            if (timer > 0.5f)
            {
                Enemy.GetComponent<Eenemymove>().Hpbar.fillAmount -= PlayerDamage / BossHp;
                Enemy.GetComponent<Eenemymove>().Hp -= PlayerDamage;
                timer = 0;
                //AttackParticle.particleSystem.animation
            }
        } 
     }
    //void OnCollisionEnter(Collision order)
    // {
    //     if (other.transform.tag == "Enemy")
    //     {
    //         AttackParticle.SetActive(true);
    //         AttackParticle.transform.position = other.transform.position;
    //     }
    // }
}

