using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "TeleportIfFallingOutOfBounds", menuName = "State Machines/Actions/Teleport If Falling Out Of Bounds")]
public class TeleportIfFallingOutOfBoundsSO : StateActionSO<TeleportIfFallingOutOfBounds>
{
	[Tooltip("Minimum fall time before checking if the player is falling out of bounds")]
	public float minFallTime = 5f;
	
	public TransformEventChannelSO playerTeleportedChannel = default;
}

public class TeleportIfFallingOutOfBounds : StateAction
{
	private Protagonist _protagonistScript;

	private float _fallTimer;
	
	private TeleportIfFallingOutOfBoundsSO _originSO => (TeleportIfFallingOutOfBoundsSO)base.OriginSO;

	public override void Awake(StateMachine stateMachine)
	{
		_protagonistScript = stateMachine.GetComponent<Protagonist>();
	}
	
	public override void OnStateEnter()
	{
		_fallTimer = _originSO.minFallTime;
	}
	
	public override void OnUpdate()
	{
		_fallTimer -= Time.deltaTime;
		
		if (_fallTimer <= 0f)
		{
			if (!HasGroundBelow())
			{
				TeleportToLastValidPos();
			}
			else
			{
				_fallTimer = _originSO.minFallTime;
			}
		}
	}

	private bool HasGroundBelow()
	{
		return Physics.Raycast(_protagonistScript.transform.position, Vector3.down,999f);
	}
	
	private void TeleportToLastValidPos()
	{
		_protagonistScript.transform.position = _protagonistScript.lastValidPos;
		
		//Force update positions to make sure the character controller does not override movement
		//an alternative to that would be to disable the controller and reenable it again after setting the position
		Physics.SyncTransforms();
		
		//Fire an event that tells the camera to instantly warp to the player
		_originSO.playerTeleportedChannel.RaiseEvent(_protagonistScript.transform);
	}
}
