using UnityEngine;
using System.Collections;
//이승환//EnemyCheking에서 Player위치 값을 받아와서 몬스터 이동
public class Eenemymove : MonoBehaviour
{

    public CharacterController cc;  //Enemy의 컨트롤러를 받아 이동
    public Animation any;           //Enemy의 에니메이션
    public float speed;             //Enemy의 스피드
    public Vector3 Vdir;            //EnemyCheking에서 Player의 위치값을 받아온다
    public float Hp;
    public float Damage;
    public UISprite Hpbar;

    float distancedir;              //Enemy와 player의 거리, 작아질수록 가까워진다

    Vector3 v1;                     //Enemy를 이동하기위한 백터
    Vector3 v2;                     //Enemy를 오브젝터의 축
    Quaternion dir;                 //Enemy가 보는방향

    public Animator animator;

    public bool attackOn = false, moveOn = false;// 어택과 무브 온오프

    void Awake()
    {
        Hpbar.fillAmount = Hp;
        
        
       // test.fillAmount -= Time.deltaTime * 0.1f;
    }

    void Update()
    {
        if (attackOn)//이승환//어택 에니매이션 실행 함수
        {
            distancedir = Vector3.Distance(Vdir, transform.position);
            animator.SetBool("bAttack_01", true);
            animator.SetBool("bWalk", false);
            animator.SetBool("bRun", false);
            // any.CrossFade("1_attack", 0.25f);
            if (distancedir > 1f)//이승환//거리가 멀어지면 이동
            {
                attackOn = false;
                moveOn = true;
            }
        }
        else if (moveOn)//이승환//Enemy이동과 에니메이션
        {
            //transform.position = Vdir * Time.deltaTime * speed;
            v1 = (Vdir - transform.position).normalized;
            distancedir = Vector3.Distance(Vdir, transform.position);

            if (distancedir > 4f)//이승환//멀어지면 빨라짐
            {
                animator.SetBool("bRun", true);
                animator.SetBool("bWalk", false);
                animator.SetBool("bAttack_01", false);
                // any.CrossFade("2_run", 0.25f);
                speed = 2f; 
            }
            else if (distancedir >2f)//이승환//가까운 속도
            {
                animator.SetBool("bWalk", true);
                animator.SetBool("bRun", false);
                animator.SetBool("bAttack_01", false);
                // any.CrossFade("2_run", 0.25f);
                if (speed < 0.13f)
                speed = 1f; 
            }
            else//이승환//위의 조건보다 작으면 공격하고 if문을 나가서 이동을 못하게한다
            {
                if (moveOn)
                    moveOn = false;

                attackOn = true;
                return;
            }
            //이승환//이동과 방향
            cc.Move(v1 * speed * Time.deltaTime);
            cc.Move(new Vector3(0, -10f, 0));

            dir = Quaternion.LookRotation(v1);
            v2.y = dir.eulerAngles.y;
            dir.eulerAngles = v2;

            transform.rotation = Quaternion.Slerp(transform.rotation, dir, 5f * Time.deltaTime);
            ////////////////////
        }
        else//이승환//정지 에니메이션
        {
            animator.SetBool("bRun", false);
            animator.SetBool("bWalk", false);
            animator.SetBool("bAttack_01", false);
            // any.CrossFade("0_idle", 0.25f);
            moveOn = false;
            attackOn = false;
        }
    }
}
