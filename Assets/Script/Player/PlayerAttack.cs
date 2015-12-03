using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {
    public Animator _Playre;
    public bool bAttack = false;
    public float damig =10;

    float timer;
	void Update () {
        if (bAttack)
        {
            timer += Time.deltaTime;
            if (timer > 0.1f)
            {
                _Playre.ResetTrigger("bAttack_01");
                timer = 0f;
                bAttack = false;
            }
        }
	}

    public void Attack_()
    {

        _Playre.SetTrigger("bAttack_01");
        bAttack = true;
    }
}
