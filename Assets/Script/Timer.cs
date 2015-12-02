using UnityEngine;
using System.Collections;

//타이머 스크립트

public class Timer : MonoBehaviour {
    private float _timerForText;    //이승환//진행시간 저장
    private int _secText;           //이승환//초
    private int _minText;           //이승환//분
    public UILabel time;            //이승환//시간을 표시해줄 UI라밸

	void Update () {

        _timerForText += Time.deltaTime;

        if (_timerForText > 1.0f)
        {
            ++_secText;
            if (_secText > 60)
            {
                ++_minText;
                _secText = 0;
            }
            time.text = string.Format("{0:D2}", _minText) + ":" + string.Format("{0:D2}", _secText);
            _timerForText = 0;
        }

	
	}
}
