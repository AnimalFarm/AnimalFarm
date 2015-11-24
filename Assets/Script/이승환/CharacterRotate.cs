using UnityEngine;
using System.Collections;
//조이스틱 위치로 캐릭터 방향 설정
public class CharacterRotate : MonoBehaviour
{
    public UIJoystick stick; //조이스틱

    // Update is called once per frame
    void Update()
    {
        if (stick.position.x > stick.radius - 4 || stick.position.x < -stick.radius + 4 || stick.position.y > stick.radius - 4 || stick.position.y < -stick.radius + 4)
        {
            transform.forward = (new Vector3(-1 * stick.position.x * Time.deltaTime * 1f, 0, -1 * stick.position.y * Time.deltaTime * 1f));
        }
    }
}
