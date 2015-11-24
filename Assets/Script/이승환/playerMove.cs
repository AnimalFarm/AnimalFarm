using UnityEngine;
using System.Collections;
//이승환//조이스틱으로 플래이어 이동
public class playerMove : MonoBehaviour
{
    public UIJoystick stick;    //이승환//조이스텍스크립트를 가지고있는 플렌
    public Animator _rabbit;    //이승환//player 에니메이션

    public float speed = 0.01f;    //이승환//player 스피드

    float sX,sZ;
    
    void Awake ()
    {
        sX = -(stick.position.x/2 * Time.deltaTime * speed);
        sZ = -(stick.position.y/2 * Time.deltaTime * speed);
    }
	void Update () {

        transform.Translate(-(stick.position.x * Time.deltaTime * speed), 0f, -(stick.position.y * Time.deltaTime * speed));//이승환//이동

        //이승환//이동하면 에니메이션 실행
        if (stick.position.x > stick.radius - 4 || stick.position.x < -stick.radius + 4 || stick.position.y > stick.radius - 4 || stick.position.y < -stick.radius + 4)
        {
          _rabbit.SetBool("runChk", true);
        }
	}
}
