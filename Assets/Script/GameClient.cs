using UnityEngine;
using System.Collections;
using Nettention.Proud;

public class GameClient : MonoBehaviour{

    NetClient m_netClient = new NetClient();
    SocialC2S.Proxy m_C2SProxy = new SocialC2S.Proxy();
    SocialS2C.Stub m_S2CStub = new SocialS2C.Stub();
    string m_serverAddr = "172.30.98.223";
    ushort m_serverPort = 5345;
    string m_villeName = "NSH";
    bool m_requestNewVille = false;
    string m_loginButtonText = "Connect!";
    string m_failMessage = "";
    enum State { Standby, Connecting, LoggingOn, InVille, Failed }
    State m_state;
    
    void Awake()
    {
        m_netClient.AttachProxy(m_C2SProxy);
        m_netClient.AttachStub(m_S2CStub);
        m_S2CStub.ReplyLogon = (Nettention.Proud.HostID remote, Nettention.Proud.RmiContext rmiContext, int result, System.String comment) =>
            {
                if (result == 0)
                {
                    m_state = State.InVille;
                }
                else
                {
                    m_state = State.Failed;
                    m_failMessage = "Logon failed: " + result.ToString() + " " + comment;
                }
                return true;
            };
    }
	void Update () 
    {
        m_netClient.FrameMove(); //노승현, (단일 스레드)모든 이벤트 통지를 담당
	}
    void OnGUI()
    {
        switch(m_state)
        {
            case State.Standby:
            case State.Connecting:
            case State.LoggingOn:
                OnGUI_Logon();
                break;
            case State.InVille:
                break;
            case State.Failed:
                GUI.Label(new Rect(10, 30, 200, 80), m_failMessage);
                if(GUI.Button(new Rect(10,100,180,30), "Quit"))
                {
                    Application.Quit();
                }
                break;
        }
        OnGUI_Logon();
    }
    void OnGUI_Logon()
    {
        GUI.Label(new Rect(10, 10, 300, 70), "ProudNet example");
        GUI.Label(new Rect(10, 60, 180, 30), "Server address");
        m_serverAddr = GUI.TextField(new Rect(10, 80, 180, 30), m_serverAddr);
        GUI.Label(new Rect(10, 110, 180, 30), "Ville name");
        m_villeName = GUI.TextField(new Rect(10, 130, 180, 30), m_villeName);
        m_requestNewVille = GUI.Toggle(new Rect(10, 160, 180, 20), m_requestNewVille, "");
        if (GUI.Button(new Rect(10, 190, 100, 30), m_loginButtonText))
        {
            if (m_state == State.Standby)
            {
                m_state = State.Connecting;
                m_loginButtonText = "Connecting...";
                IssueConnect();
            }
        }
    }
    void IssueConnect()
    {
        m_netClient.JoinServerCompleteHandler = (ErrorInfo info, ByteArray replyFromServer) =>
        {
            if (info.errorType.Equals(ErrorType.ErrorType_Ok)) //노승현, 서버 접속 성공
            {
                if (info.errorType.Equals(ErrorType.ErrorType_Ok))
                {
                    m_state = State.LoggingOn;
                    m_loginButtonText = "접속 성공";
                    m_C2SProxy.RequestLogon(HostID.HostID_Server, RmiContext.ReliableSend, m_villeName, m_requestNewVille);
                }
            }
            else //노승현, 실패
            {
                m_loginButtonText = "접속 실패";
                m_failMessage = info.ToString();
            }
        };
        m_netClient.LeaveServerHandler = (ErrorInfo errorinfo) => //노승현, 비정상 접속해제
        {
            m_state = State.Failed;
            m_failMessage = "Server connection lost";
        };
        m_netClient.ErrorHandler = (ErrorInfo errorinfo) =>
        {
            m_loginButtonText = "에러 발생";
        };

        NetConnectionParam cp = new NetConnectionParam();
        cp.serverIP = m_serverAddr;
        cp.serverPort = m_serverPort;
        cp.protocolVersion = new Guid("{0xc26fa5c2,0x723d,0x4a45,{0xba,0x9a,0xb6,0x2,0xa,0xc,0xf0,0xbc}}");

        m_netClient.Connect(cp);


    }
    void OnDestroy()
    {
        m_netClient.Dispose();
    }
}
