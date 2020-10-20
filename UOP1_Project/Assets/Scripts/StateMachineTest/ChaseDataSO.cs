using UnityEngine;

[CreateAssetMenu(fileName = "ChaseData", menuName = "State Machines/Tests/Data/Chase Data")]
public class ChaseDataSO : ScriptableObject
{
	[SerializeField] private string _targetName = "Player";
	[SerializeField] private float _speed = 10f;
	public float Speed => _speed;
	public string TargetName => _targetName;
}
