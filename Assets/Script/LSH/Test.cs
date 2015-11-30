using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {
   public UISprite test;
   int HP = 100;
	// Use this for initialization
	void Start () {
        test.fillAmount = HP * 0.01f;
	}
	
	// Update is called once per frame
	void Update () {
        
       // test.fillAmount -= Time.deltaTime * 0.1f;
	}
}
