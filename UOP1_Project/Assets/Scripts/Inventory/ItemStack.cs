public class ItemStack
{
	public Item Item { get; }

	public int Amount { get; set; }

	public ItemStack(Item item, int amount)
	{
		Item = item;
		Amount = amount;
	}
}
