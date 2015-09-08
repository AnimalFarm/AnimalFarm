using UnityEngine;
using System.Collections;

public class PhonManager : MonoBehaviour{

	void Start () 
    {
        Screen.orientation = ScreenOrientation.Landscape; //노승현, 게임 실행시 가로로 시작하는 설정
        Screen.sleepTimeout = SleepTimeout.NeverSleep; //노승현, 핸드폰이 계속 켜져있게끔 하는 설정
	}
	void Update () 
    {
	
	}
}
