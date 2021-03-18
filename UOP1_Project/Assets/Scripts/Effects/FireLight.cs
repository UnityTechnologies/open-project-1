using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLight : MonoBehaviour
{
	public AnimationCurve lightCurve;
	public float fireSpeed = 1f;

	private Light _lightComp;
	private float _initialIntensity;

	private void Awake()
	{
		_lightComp = GetComponent<Light>();
		_initialIntensity = _lightComp.intensity;
	}

	void Update()
	{
		_lightComp.intensity = _initialIntensity * lightCurve.Evaluate(Time.time * fireSpeed);
	}
}
