using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {


    private float _timerForText;
    private int _secText;
    private int _minText;
    public UILabel time;

	// Use this for initialization
    void Awake()
    {

        time = gameObject.GetComponent<UILabel>();
	}
	
	// Update is called once per frame
	void Update () {

        _timerForText += Time.deltaTime;

        if (_timerForText > 1.0f)
        {
            _secText += 1;
            if (_secText > 60)
            {
                _minText += 1;
                _secText = 0;
            }
            time.text = string.Format("{0:D2}", _minText) + ":" + string.Format("{0:D2}", _secText);
            _timerForText = 0;
        }

	
	}
}
