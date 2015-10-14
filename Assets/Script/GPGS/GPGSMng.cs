using UnityEngine;
using System.Collections;
using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.BasicApi;

public class GPGSMng : Singleton<GPGSMng> 
{
    //노승현, 현재 로그인 중인지 체크
    public bool bLogin 
    {
        get;
        set;
    }
    //노승현, GPGS 초기화
    public void InitGPGS()
    {
        bLogin = false;
        PlayGamesPlatform.Activate();
    }
    //노승현, GPGS 로그인
    public void LoginGPGS()
    {
        //노승현, 로그인상태가 아니라면
        if(!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate(LoginCallBackGPGS);
        }
    }
    public void LoginCallBackGPGS(bool result)
    {
        bLogin = result;
    }
    //노승현, 로그아웃
    public void LogoutGPGS()
    {
        //노승현, 로그인 상태라면
        if(Social.localUser.authenticated)
        {
            ((GooglePlayGames.PlayGamesPlatform)Social.Active).SignOut();
            bLogin = false;
        }
    }
    //노승현, 자신의 프로필 이미지를 가져온다.
    public Texture2D GetImageGPGS()
    {
        if (Social.localUser.authenticated)
        {
            return Social.localUser.image;
        }
        else
            return null;
    }
    //노승현, 사용자의 이름을 가져옵니다.
    public string GetNameGPGS()
    {
        if(Social.localUser.authenticated)
        {
            return Social.localUser.userName;
        }
        else
        {
            return null;
        }
    }
}
