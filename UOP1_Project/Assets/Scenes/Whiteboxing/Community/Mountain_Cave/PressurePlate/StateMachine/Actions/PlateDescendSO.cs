using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "PlateDescend", menuName = "State Machines/Actions/Plate Descend")]
public class PlateDescendSO : StateActionSO
{
	protected override StateAction CreateAction() => new PlateDescend();
}

public class PlateDescend : StateAction
{
	Transform _plateTransform;
    private PressurePlate _plate;
    //public float _maxDescent = -.15f;
    float _minY;
	public override void Awake(StateMachine stateMachine)
	{
        _plate = stateMachine.GetComponent<PressurePlate>();
		_plateTransform = _plate.transform;
        _minY = _plateTransform.position.y + _plate._maxDescent;
	}

	public override void OnUpdate()
	{
        if (_plateTransform.position.y > _minY)
        {
            Debug.Log("Going down...");
            _plateTransform.position = _plateTransform.position + new Vector3(0, -.2f, 0);
        }
	}

	public override void OnStateEnter()
	{
		Debug.Log("GoingDown state entered");
	}
}
