using UnityEngine;
using System.Collections;

public class SetProfile : MonoBehaviour
{
    public UITexture user_Photo = null; 
    public UILabel userName = null;
    public string userID = null;  
    void Awake()
    {

	}
    void Update()
    {
        if (GPGSMng.GetInstance.GetImageGPGS() != null)
        {
            user_Photo.mainTexture = GPGSMng.GetInstance.GetImageGPGS();
        }
        userName.text = GPGSMng.GetInstance.GetNameGPGS();
        userID = GPGSMng.GetInstance.GetUserIDGPGS();
    }
}
