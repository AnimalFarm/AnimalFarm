using UnityEngine;
using System.Collections;
using Nettention.Proud;

public class GameClient : MonoBehaviour{


    string m_serverAddr = "172.30.98.223";
    string m_villeName = "NSH";
    bool m_requestNewVille = false;
    string m_loginButtonText = "Connect!";
    enum State { Standby, Connecting, LoggingOn, InVille, }
    State m_state;
    NetClient m_netClient = new NetClient();

    
    void Awake()
    {
    }
	void Update () 
    {
        m_netClient.FrameMove(); //노승현, (단일 스레드)모든 이벤트 통지를 담당
	}
    void OnGUI()
    {
        GUI.Label(new Rect(10,10,300,70), "ProudNet example");
        GUI.Label(new Rect(10,60,180,30), "Server address");
        m_serverAddr = GUI.TextField(new Rect(10, 80, 180, 30), m_serverAddr);
        GUI.Label(new Rect(10,110,180,30),"Ville name");
        m_villeName = GUI.TextField(new Rect(10, 130, 180, 30), m_villeName);
        m_requestNewVille = GUI.Toggle(new Rect(10, 160, 180, 20), m_requestNewVille,"");
        if (GUI.Button(new Rect(10, 190, 100, 30), m_loginButtonText))
        {
            if(m_state == State.Standby)
            {
                m_state = State.Connecting;
                m_loginButtonText = "Connecting...";
                IssueConnect();
            }
        }
    }
    private void IssueConnect()
    {
        m_netClient.JoinServerCompleteHandler = (ErrorInfo info, ByteArray replyFromServer) =>
            {
                if (info.errorType.Equals(ErrorType.ErrorType_Ok)) //노승현, 서버 접속 성공
                {
                    if(info.errorType == ErrorType.ErrorType_Ok)
                    {
                        m_loginButtonText = "Connected!";
                    }
                }
                else //노승현, 실패
                {
                    m_loginButtonText = "Failed!";
                }
            };
        m_netClient.LeaveServerHandler = (ErrorInfo info) => //노승현, 비정상 접속해제
        {
            m_loginButtonText = "LEFT!!!";
        };
        NetConnectionParam cp = new NetConnectionParam();
        cp.serverIP = m_serverAddr;
        cp.serverPort = 5345;
        cp.protocolVersion = new Guid("{0xc26fa5c2,0x723d,0x4a45,{0xba,0x9a,0xb6,0x2,0xa,0xc,0xf0,0xbc}}");

        m_netClient.Connect(cp);
       
    }

    void OnDestroy()
    {
        m_netClient.Dispose();
    }
}
