using UnityEngine;
using System.Collections;

public interface IMovementActionStrategy
{
	void ApplyMovementOnStateEnter();
	void ApplyMovementOnUpdate();
	void ApplyMovementOnStateExit();
}
