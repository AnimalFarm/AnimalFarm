using UnityEngine;
using System.Collections;
// 노승현, 3D 카메라 디바이스 해상도에 맞게 크기 조절하는 스크립트

public class CameraSize : MonoBehaviour {

	
    void Awake()
    {
        gameObject.GetComponent<Camera>().orthographicSize = (Screen.height * 1280f / 720f) / Screen.width; 
	}
}
