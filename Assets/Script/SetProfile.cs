using UnityEngine;
using System.Collections;

public class SetProfile : MonoBehaviour
{
    public GameObject lobbyPanel, loadingPanel;
    public UITexture user_Photo = null; 
    public UILabel userName = null;
    public string userID = null;  

    void Update()
    {
        if (Social.localUser.authenticated)
        {
            lobbyPanel.SetActive(true);
            loadingPanel.SetActive(false);
        }
        if (GPGSMng.GetInstance.GetImageGPGS() != null)
        {
            user_Photo.mainTexture = GPGSMng.GetInstance.GetImageGPGS();
        }
        userName.text = GPGSMng.GetInstance.GetNameGPGS();
        userID = GPGSMng.GetInstance.GetUserIDGPGS();
    }
}
