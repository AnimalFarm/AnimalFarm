using UnityEngine;
using System.Collections;

// 노승현 코루틴 함수로 변경

public class GPGSButton : MonoBehaviour 
{
    public GameObject lobbyPanel, loadingPanel;
    public UITexture user_Photo = null;
    public UILabel userName = null;
    public string userID = null;
    public LobbyButton startButton;

    void Awake()
    {
        GPGSMng.GetInstance.InitGPGS(); //노승현, 초기화
        StartCoroutine(SetPanel());
	}
    IEnumerator SetPanel()
    {
        while(true)
        {
            if (Social.localUser.authenticated)
            {
                lobbyPanel.SetActive(true);
                loadingPanel.SetActive(false);
                StartCoroutine(startButton.SetButton());

                //if (GPGSMng.GetInstance.GetImageGPGS() != null)
                //{
                    
                //    Debug.Log("dfdfdfdfdfdfdf");
                //}
                user_Photo.mainTexture = GPGSMng.GetInstance.GetImageGPGS();
                userName.text = GPGSMng.GetInstance.GetNameGPGS();
                userID = GPGSMng.GetInstance.GetUserIDGPGS();
                yield break;
            }
            yield return null;
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
