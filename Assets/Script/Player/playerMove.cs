using UnityEngine;
using System.Collections;
//이승환//조이스틱으로 플래이어 이동
public class playerMove : MonoBehaviour
{
    public UIJoystick stick;    //이승환//조이스텍스크립트를 가지고있는 플렌
    public Animator _Player;    //이승환//player 에니메이션
    public float speed = 0f;    //이승환//player 스피드

	void Update ()
    {
        transform.Translate((stick.position.x * Time.deltaTime * speed), 0f, (stick.position.y * Time.deltaTime * speed));//이승환//이동
        //이승환//이동하면 에니메이션 실행
        if (stick.position.x > stick.radius - 20 || stick.position.x < -stick.radius + 20 || stick.position.y > stick.radius - 20 || stick.position.y < -stick.radius + 20)
        {
            if (_Player.name == "animal_ch_rabbit_01")
            {
                if (speed < 0.1f)
                {
                    _Player.SetBool("bWalk", true);
                }
                if (speed < 0.2f)
                {
                    speed += 0.2f * Time.deltaTime;
                }
            }
            else
            {
                if (speed < 0.1f)
                {
                    _Player.SetBool("bWalk", true);
                }
                if (speed < 0.13f)
                {
                    speed += 0.05f * Time.deltaTime;
                }
            }
            
        }
        else
        {
            speed = 0f;
            _Player.SetBool("bWalk", false);
            _Player.SetBool("bRun", false);
        }
        if (_Player.name == "animal_ch_rabbit_01")
        {
            if (speed >= 0.1f && speed < 0.2f)
            {
                _Player.SetBool("bRun", true);
                _Player.SetBool("bWalk", false);
                speed += 0.05f * Time.deltaTime;
            }
        }
        else
        {
            if (speed >= 0.1f && speed < 0.13f)
            {
                _Player.SetBool("bRun", true);
                _Player.SetBool("bWalk", false);
                speed += 0.05f * Time.deltaTime;
            }
        }
	}
}
