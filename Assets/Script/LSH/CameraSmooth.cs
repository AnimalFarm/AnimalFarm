using UnityEngine;
using System.Collections;

public class CameraSmooth : MonoBehaviour
{
    Transform mCamera;
    // Use this for initialization
    void Start()
    {
        mCamera = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        mCamera = transform.FindChild("Camera");
        mCamera.transform.LookAt(transform.position);
        mCamera.transform.RotateAround(transform.position, Vector3.up, transform.rotation.z);
    }
}
