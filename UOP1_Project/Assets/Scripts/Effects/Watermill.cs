using UnityEngine;

public class Watermill : MonoBehaviour
{
	public AnimationCurve rotationRhythm;
	public Transform wheel;
	public float speed;
    
    void Update()
    {
		wheel.Rotate(0f, 0f, rotationRhythm.Evaluate(Time.time) * speed * Time.deltaTime);
    }
}
