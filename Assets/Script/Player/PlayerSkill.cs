using UnityEngine;
using System.Collections;

public class PlayerSkill : MonoBehaviour
{
    public GameObject boss;
    public GameObject buffparticle;
    public GameObject rabbitMace;
    public GameObject rabbitskillparticle;
    float timer;
    public bool buff, rabbitskill;

    void Awake()
    {

    }

    void Update()
    {

        if (buff == true)
        {
            buffparticle.transform.position = transform.position;
            buffparticle.transform.Translate(0f, 1f, 0f);
            timer += Time.deltaTime;
            if (timer > 3f)
            {
                timer = 0;
                buff = false;
                buffparticle.SetActive(buff);
            }
        }

        if (rabbitskill == true)
        {
            rabbitskillparticle.transform.position = boss.transform.position;
            rabbitskillparticle.transform.Translate(0f, 0f, -9f);
            timer += Time.deltaTime;
            if (timer >= 2f)
            {
                timer = 0;
                rabbitskill = false;
                //rabbitMace.SetActive(rabbitskill);
                rabbitskillparticle.SetActive(rabbitskill);
            }
        }
    }
    public void Onbuffskill()
    {
        buff = true;
        buffparticle.SetActive(buff);
    }
    public void Onrabbitskill()
    {
        if (boss.GetComponent<Eenemymove>().moveOn == true && rabbitskill != true)
        {
            rabbitskill = true;
            //rabbitMace.SetActive(rabbitskill);
            rabbitskillparticle.SetActive(rabbitskill);
        }
    }
}
