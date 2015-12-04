using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {
    public GameObject bear, rabbit, panda, dog;
    

    void Awake () 
    {
        CreateBoss();
	}
	
	void Update () 
    {

	}

    public void CreateBoss()
    {
        switch (ButtonEvent.BOSS)
        {
            case 1:
                bear.SetActive(true);
                break;
            case 2:
                dog.SetActive(true);
                break;
            case 3:
                rabbit.SetActive(true);
                break;
            case 4:
                panda.SetActive(true);
                break;
        }     
    }
}
