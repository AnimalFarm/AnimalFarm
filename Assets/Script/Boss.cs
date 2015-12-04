using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {
    public GameObject Bear, Rabbit, Panda, Dog;
    

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
                Bear.SetActive(true);
                break;
            case 2:
                Dog.SetActive(true);
                break;
            case 3:
                Rabbit.SetActive(true);
                break;
            case 4:
                Panda.SetActive(true);
                break;
        }     
    }
}
