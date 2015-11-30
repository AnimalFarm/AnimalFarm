using UnityEngine;
using System.Collections;
//조이스틱 위치로 캐릭터 방향 설정
public class CharacterRotate : MonoBehaviour
{
    public UIJoystick stick; //조이스틱

    // Update is called once per frame
    void Update()
    {
        //조이스틱범위가 20 이상이면 움직임 현재 40
        if (stick.position.x > stick.radius - 20 || stick.position.x < -stick.radius + 20 || stick.position.y > stick.radius - 20 || stick.position.y < -stick.radius + 20)
        {
            transform.forward = (new Vector3((stick.position.x * Time.deltaTime)/2, 0f,(stick.position.y * Time.deltaTime)/2));
        }
    }
}