[System.Serializable]
public class SerializedItemStack
{
	public string itemGuid;
	public int amount;

	public SerializedItemStack(string itemGuid, int amount)
	{
		this.itemGuid = itemGuid;
		this.amount = amount;
	}
}
