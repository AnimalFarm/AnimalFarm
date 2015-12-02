using UnityEngine;
using System.Collections;
//이승환//조이스틱으로 플래이어 이동
public class playerMove : MonoBehaviour
{
    public UIJoystick stick;    //이승환//조이스텍스크립트를 가지고있는 플렌
    public Animator _rabbit;    //이승환//player 에니메이션
    public float speed = 0f;    //이승환//player 스피드

	void Update ()
    {
        transform.Translate((stick.position.x * Time.deltaTime * speed), 0f, (stick.position.y * Time.deltaTime * speed));//이승환//이동
        //이승환//이동하면 에니메이션 실행
        if (stick.position.x > stick.radius - 20 || stick.position.x < -stick.radius + 20 || stick.position.y > stick.radius - 20 || stick.position.y < -stick.radius + 20)
        {
            if (speed < 0.1f) { 
                _rabbit.SetBool("bWalk", true); 
            }
            if (speed < 0.13f) {
                speed += 0.05f * Time.deltaTime; 
            }
        }
        else
        {
            speed = 0f;
            _rabbit.SetBool("bWalk", false);
            _rabbit.SetBool("bRun", false);
        }

        if (speed >= 0.1f && speed < 0.13f )
        {
            _rabbit.SetBool("bRun", true);
            _rabbit.SetBool("bWalk", false);
            speed += 0.05f * Time.deltaTime;
        }
	}
}
