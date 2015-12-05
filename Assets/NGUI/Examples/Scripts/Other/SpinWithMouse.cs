using UnityEngine;

[AddComponentMenu("NGUI/Examples/Spin With Mouse")]
public class SpinWithMouse : MonoBehaviour
{
	public Transform target;
	public float speed = 1f;
    int number = 0;
    public Lines lines;
    
	void OnDrag (Vector2 delta)
	{
        target.localRotation = Quaternion.Euler(0f, -0.5f * delta.x * speed, 0f) * target.localRotation;   
	}
    void OnClick()
    {
        ++number;
        if (number == 1) lines.LinesText(number);
        if (number == 2) lines.LinesText(number);
        if (number == 3) lines.LinesText(number);
        if (number == 4) lines.LinesText(number);
        if (number == 5)
        {
            lines.LinesText(number);
            number = 0;
        }    
    }
}