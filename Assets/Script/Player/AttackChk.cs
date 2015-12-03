using UnityEngine;
using System.Collections;

public class AttackChk : MonoBehaviour {

    public GameObject Enemy;    //오브잭트
    public GameObject Player;   //오브잭트
    float timer=0;

    void Awake()
    {
           
    }
    void Update()
    {
        timer += Time.deltaTime;
    }
     void OnTriggerEnter(Collider order)//이승환//오브젝트와 콜리더가 처음 충돌했을 때 한번 한번호출
     {
         if (order.tag == "Enemy" && order.tag != "Player" && Player.GetComponent<PlayerAttack>().bAttack == true)
        {
            if (timer > 0.5f)
            {
                Enemy.GetComponent<Eenemymove>().test.fillAmount -= 10f * 0.01f;
                Enemy.GetComponent<Eenemymove>().Hp -= 10f;
                timer = 0;
            }
        } 
     }
}

