using UnityEngine;
using System.Collections;
//이승환//캐릭터를 따라다니는 채력바
public class HP_bar : MonoBehaviour {

    public GameObject target;
	
	void Update () {
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 0.2f, target.transform.position.z);
	}
}
