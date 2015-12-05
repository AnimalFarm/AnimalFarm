using UnityEngine;
using System.Collections;

public class CameraSmooth : MonoBehaviour
{
    public Transform Bear, rabbit, panda, dog, penguin;
    
    Transform target = null;


    public float distance = 10.0f;
    public float minHeight = 5.0f;
    public float height = 5.0f;
    public float maxHeight = 30.0f;
    public float heightDamping = 2.0f;
    public float rotation = 0;
    public float rotationDamping = 3.0f;
    public float rotationFactor = 1;
    public float zoomFactor = 1;

    void Awake()
    {
        switch (CharacterChoice.PLAYER_CHOICE)
        {
            case 1:
                target = Bear;
                break;
            case 2:
                target = dog;
                break;
            case 3:
                target = panda;
                break;
            case 4:
                target = penguin;
                break;
            case 5:
                target = rabbit;
                break;
        }
    }

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
            this.transform.position = target.position;
            this.transform.position -= Vector3.forward * distance;

            Vector3 pos = this.transform.position;
            pos.y = currentHeight;
            this.transform.position = pos;

            // rotation.
            float wantedRotation = this.rotation;
            if (this.rotationDamping != 0)
            {
                wantedRotation = Mathf.LerpAngle(transform.eulerAngles.y, this.rotation, this.rotationDamping * Time.deltaTime);
            }
            this.transform.RotateAround(target.position, Vector3.up, wantedRotation);
            this.transform.LookAt(target);
        }
    }
}
