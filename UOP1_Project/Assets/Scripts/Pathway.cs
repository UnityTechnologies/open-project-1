using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathway : MonoBehaviour
{
	[HideInInspector]
	public List<Vector3> wayPoints;
	[SerializeField]
	private float _cubeSize = 3;
	[SerializeField]
	private Color _cubeColor = Color.red;
	[SerializeField]
	private Color _lineColor = Color.black;
	[SerializeField]
	private Color _selectedObjectColor = Color.white;

	public Color CubeColor
	{
		get => _cubeColor;
	}

	public Color LineColor
	{
		get => _lineColor;
	}

	public Color SelectedObjectColor
	{
		get => _selectedObjectColor;
	}

	public float Size
	{
		get => _cubeSize;
	}

	private void OnEnable()
	{
		wayPoints = new List<Vector3>();
	}
}
