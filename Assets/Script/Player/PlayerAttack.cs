using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {
    public Animator _Playre;
    public bool bAttack = false;
    public float damig =10;
    public GameObject Sword;
    float timer;
    void Awake()
    {
        //Sword.GetComponent<BoxCollider>().enabled = false;
    }
	void Update () {
        if (bAttack)
        {
            timer += Time.deltaTime;
            if (timer > 0.25f)
            {
                Sword.GetComponent<BoxCollider>().enabled = true;
                _Playre.ResetTrigger("bAttack_01");
                timer = 0f;
                bAttack = false;
            }
           
        }
       
	}

    public void Attack_()
    {
        Sword.GetComponent<BoxCollider>().enabled = false; 
        _Playre.SetTrigger("bAttack_01");
        bAttack = true;
    }
}
