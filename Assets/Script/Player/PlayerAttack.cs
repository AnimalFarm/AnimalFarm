using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {

    public GameObject gameManage;
    public Animator _Playre;
    public GameObject playerLookat;
    public bool bAttack = false;
    public float damig;
    public GameObject Sword;
    public float Hp;
    public UISprite Hpbar;

    float timer;

    void Awake()
    {
        Hpbar.fillAmount = Hp;
    }
	void Update () {
        if (bAttack)
        {
            timer += Time.deltaTime;
            if (timer > 0.3f)
            {
                Sword.GetComponent<BoxCollider>().enabled = true;
                _Playre.ResetTrigger("bAttack_01");
            }
            if(timer > 0.5f)
            {
                bAttack = false;
                timer = 0f;
            }

        }
        else { Sword.GetComponent<BoxCollider>().enabled = false; bAttack = false; }
       
       
	}

    public void Attack_()
    {
        //playerLookat.transform.LookAt(gameManage.GetComponent<Boss>().bear.transform);
        Sword.GetComponent<BoxCollider>().enabled = false; 
        _Playre.SetTrigger("bAttack_01");
        bAttack = true;
    }
}
