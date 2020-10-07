
namespace UOP1.Cutscenes
{
	/// <summary>
	/// Inherit this class to create a target for the timelines dialogue
	/// </summary>
	public abstract class DialogueBinder : UnityEngine.Component
	{
		public abstract void SetDialog(string dialogueLineID);
	}
}
