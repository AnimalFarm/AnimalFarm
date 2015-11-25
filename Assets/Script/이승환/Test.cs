using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.Euler(0f, 0f, -90f);
	}
}
