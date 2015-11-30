using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {
    public Animator _Playre;
    float timer;
    bool Battack = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Battack)
        {
            timer += Time.deltaTime;
            if (timer > 0.05f)
            {
                _Playre.ResetTrigger("bAttack_01");
                timer = 0f;
                Battack = false;
            }
        }
	}

    public void Attack_()
    {
        _Playre.SetTrigger("bAttack_01");
        Battack = true;
    }
}
