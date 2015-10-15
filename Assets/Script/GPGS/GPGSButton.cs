using UnityEngine;
using System.Collections;

public class GPGSButton : MonoBehaviour 
{
    public UILabel Login_Label = null;
    public UILabel User_Label = null;
    public UITexture User_Texture = null;
    public static string userID = null;

    void Awake()
    {
        GPGSMng.GetInstance.InitGPGS(); //노승현, 초기화
	}
	
	void Update () 
    {
	    if(GPGSMng.GetInstance.bLogin == false)
        {
         
        }
        else
        {
            SettingUser();
        }
	}

    public void ClickEvent()
    {
        if(GPGSMng.GetInstance.bLogin == false)
        {
            GPGSMng.GetInstance.LoginGPGS(); //노승현, 로그인
            SettingUser();
            
        }
        else
        {
            GPGSMng.GetInstance.LogoutGPGS(); //노승현, 로그아웃
        }
    }

    void SettingUser()
    {
        if (User_Texture.mainTexture != null)
            return;
        User_Label.enabled = true;
        User_Texture.enabled = true;

        User_Label.text = GPGSMng.GetInstance.GetNameGPGS();
        User_Texture.mainTexture = GPGSMng.GetInstance.GetImageGPGS();
        Login_Label.text = GPGSMng.GetInstance.GetUserIDGPGS();

        userID = GPGSMng.GetInstance.GetUserIDGPGS();
        Debug.Log(userID);
        Debug.Log(GPGSMng.GetInstance.GetImageGPGS());
    }
}
