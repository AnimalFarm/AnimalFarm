using UnityEngine;
using System.Collections;

public class CameraSmooth : MonoBehaviour
{
    public Transform target = null;

    public float distance = 10.0f;
    public float minHeight = 5.0f;
    public float height = 5.0f;
    public float maxHeight = 30.0f;
    public float heightDamping = 2.0f;

    public float rotation = 0;
    public float rotationDamping = 3.0f;

    public float rotationFactor = 1;
    public float zoomFactor = 1;

    private Transform tm;

    // Use this for initialization
    void Start()
    {
        // lcoal caching for the performance.
        this.tm = this.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            // adjust height.
            if (this.height < this.minHeight)
                this.height = this.minHeight;
            else if (this.height > this.maxHeight)
                this.height = this.maxHeight;

            float wantedHeight = target.position.y + this.height;
            float currentHeight = transform.position.y ;
            if (this.heightDamping != 0)
            {
                currentHeight = Mathf.Lerp(currentHeight, wantedHeight, this.heightDamping * Time.deltaTime);
            }
            else
                currentHeight = wantedHeight;

            // position.
            this.tm.position = target.position;
            this.tm.position -= Vector3.forward * distance;

            Vector3 pos = this.tm.position;
            pos.y = currentHeight;
            this.tm.position = pos;

            // rotation.
            float wantedRotation = this.rotation;
            if (this.rotationDamping != 0)
            {
                wantedRotation = Mathf.LerpAngle(tm.eulerAngles.y, this.rotation, this.rotationDamping * Time.deltaTime);
            }
            this.tm.RotateAround(target.position, Vector3.up, wantedRotation);
            this.tm.LookAt(target);
        }
    }
}
