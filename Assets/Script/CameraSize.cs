using UnityEngine;
using System.Collections;

public class CameraSize : MonoBehaviour {

	
    void Awake()
    {
        gameObject.GetComponent<Camera>().orthographicSize = (Screen.height * 1280f / 720f) / Screen.width; 
	}
}
