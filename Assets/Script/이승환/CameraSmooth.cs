using UnityEngine;
using System.Collections;

public class CameraSmooth : MonoBehaviour
{

    public GameObject _target;
    public Vector3 _iniPos;
    public Vector3 _nowPos;
    public Camera m_pCam;
    // Use this for initialization
    void Start()
    {
        _iniPos = transform.position;
        UIRoot root = gameObject.GetComponent<UIRoot>();
     //   root.automatic = true;        // �ڵ� �����ǰ� �ϰ�.

        root.manualHeight = 480;
        root.minimumHeight = 480;
        root.maximumHeight = 1280;        
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = _iniPos + _target.transform.position;
        float perx = 1024.0f / Screen.width;        // �̹��� ������ �ִ´�.
        float pery = 768.0f / Screen.height;        // ��͵� �̹��� ������
        float v = (perx > pery) ? perx : pery;


        m_pCam.GetComponent<Camera>().orthographicSize = v;

    }
}
