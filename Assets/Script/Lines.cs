using UnityEngine;
using System.Collections;

public class Lines : MonoBehaviour {
    public GameObject linesPopUp;
    public UILabel lines;

    public void LinesText(int number)
    {
        linesPopUp.SetActive(true);
        if (number == 1) lines.text = "안녕 ~";
        if (number == 2) lines.text = "건들지마 !";
        if (number == 3) lines.text = "하지마 !";
        if (number == 4) lines.text = "혼난다 !";
        if (number == 5) lines.text = "우이쒸 !";
        StartCoroutine(TimeOut(3.5f));
    }
    IEnumerator TimeOut(float time)
    {
        yield return new WaitForSeconds(time);
        linesPopUp.SetActive(false);
    }
}
