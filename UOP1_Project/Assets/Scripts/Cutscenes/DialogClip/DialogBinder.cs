
/// <summary>
/// Inherit this class to create a target for the timelines dialog
/// </summary>
public abstract class DialogBinder : UnityEngine.Component
{
    public abstract void SetDialog(string dialogLineID);
}