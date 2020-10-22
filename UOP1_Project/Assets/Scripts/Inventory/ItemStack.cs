using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStack
{
    private Item _item;
    private int _amount;

    public ItemStack(Item item, int amount)
    {
        _item = item;
        _amount = amount;
    }
}
