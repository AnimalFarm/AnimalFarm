using UnityEngine;
using System.Collections;

public class GPGSButton : MonoBehaviour 
{
    public GameObject robbyPanel, loadingPanel;

    void Awake()
    {
        GPGSMng.GetInstance.InitGPGS(); //노승현, 초기화

	}
    void Update()
    {
        if (Social.localUser.authenticated)
        {
            robbyPanel.SetActive(true);
            loadingPanel.SetActive(false);
        }
    }
    public void ClickEvent()
    {
        if (GPGSMng.GetInstance.bLogin == false)
        {
            GPGSMng.GetInstance.LoginGPGS(); //노승현, 로그인
        }
        //else
        //{
        //    GPGSMng.GetInstance.LogoutGPGS(); //노승현, 로그아웃
        //}
    }
}
