using UnityEngine;
using System.Collections;

public class AttackChk : MonoBehaviour {

    public GameObject Enemy;    //이승환//보스 오브잭트
    public GameObject Player;   //이승환//플레이어 오브잭트
 
    float timer=0;

    void Awake()
    {
           
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
            if (timer > 0.45f)
            {
                Enemy.GetComponent<Eenemymove>().test.fillAmount -= 10f * 0.01f;
                Enemy.GetComponent<Eenemymove>().Hp -= 10f;
                timer = 0;
            }
        } 
     }
}

