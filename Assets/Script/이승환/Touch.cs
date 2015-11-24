using UnityEngine;
using System.Collections;
//이승환//터치이밴트 스크립트
public class Touch : MonoBehaviour {

	void Update () {
        if (Input.touchCount > 0)
        {
            Vector2 pos = Input.GetTouch(0).position;                    // 터치한 위치
            Vector3 theTouch = new Vector3(pos.x, pos.y, 0.0f);          // 변환 안하고 바로 Vector3로 받아도 되겠지.

            Ray ray = Camera.main.ScreenPointToRay(theTouch);            // 터치한 좌표 레이로 바꾸엉
            RaycastHit hit;    // 정보 저장할 구조체 만들고
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))           // 레이저를 끝까지 쏴블자. 충돌 한넘이 있으면 return true다.
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)         // 처음 터치 할때 발생한다
                {
                  
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Moved)    // 터치하고 움직이믄 발생한다.
                {
                    
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Ended)    // 터치 떼면 발생한다.
                {
                    
                }
            }
        }
	}
}
