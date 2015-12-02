using UnityEngine;
using System.Collections;

// 노승현 코루틴 함수로 변경

public class GPGSButton : MonoBehaviour 
{
    void Awake()
    {
        GPGSMng.GetInstance.InitGPGS(); //노승현, 초기화
	}
    public void ClickEvent()
    {
        if (GPGSMng.GetInstance.bLogin == false)
        {
            GPGSMng.GetInstance.LoginGPGS(); //노승현, 로그인
        }
    }
}
