using UnityEngine;
using System.Collections;
//이승환//Sphere collider 충돌채크 해서Enemymove로 전달, Sphere collider에 Is Trigger 채크돼있어야함
public class EnemyCheking : MonoBehaviour {

    public GameObject Enemy;    //Enemy 오브잭트
    
    void OnTriggerEnter(Collider order)//이승환//오브젝트와 콜리더가 처음 충돌했을 때 한번 한번호출
    {
        if (order.tag == "Player")
        {
            Enemy.GetComponent<Eenemymove>().Vdir = order.transform.position;
            Enemy.GetComponent<Eenemymove>().moveOn = true;
        }
    }

    void OnTriggerStay(Collider order)//이승환//콜리더안에 오브젝트가 움직일때마다 호출
    {
        if (order.tag == "Player")
        {
            Enemy.GetComponent<Eenemymove>().Vdir = order.transform.position;
        }
    }
    void OnTriggerExit(Collider order)//이승환//오브젝트가 콜리더를 나가면 호출
    {
        if (order.tag == "Player")
        {
            Enemy.GetComponent<Eenemymove>().moveOn = false;
            Enemy.GetComponent<Eenemymove>().attackOn = false;
        }
    }
}
