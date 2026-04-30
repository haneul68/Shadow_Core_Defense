using System;
using System.Collections.Generic;

public class Inventory_Logic
{
    private Inventory_Data inventory_Data;

    public event Action<Item_Scriptable> On_Item_Changed;
    public event Action<Item_Scriptable> On_Equip_Changed;


    public Inventory_Logic(Inventory_Data data)
    {
        inventory_Data = data;
    }
    public bool Get_Item(Item_Scriptable item, int amount)
    {
        bool success = inventory_Data.Add_Item(item, amount);
        if (success) On_Item_Changed?.Invoke(item);
        return success;
    }

    public bool Use_Item(Item_Scriptable item, int amount)
    {
        bool success = inventory_Data.Use_Item(item, amount);

        if (!success) return false;

        Character target = Base_Manager.Character_Mng.current_Character.GetComponent<Character>();

        item.Use(target);

        On_Equip_Changed?.Invoke(item);

        On_Item_Changed?.Invoke(item);
        return true;
    }
    public bool Consume_Item(Item_Scriptable item, int amount)
    {
        bool success = inventory_Data.Use_Item(item, amount);

        if (!success) return false;

        On_Equip_Changed?.Invoke(item);
        On_Item_Changed?.Invoke(item);
        return true;
    }

    public int Get_Item_Count(string item_ID)
    {
        return inventory_Data.Get_Quantity(item_ID);
    }


    public bool Try_Equip_Item(Item_Scriptable item)
    {
        bool success = inventory_Data.Try_Equip_Item(item);

        if (!success) return false;

        On_Equip_Changed?.Invoke(item);
        return true;
    }


    public bool Try_UnEquip_Item(Item_Scriptable item)
    {
        bool success = inventory_Data.UnEquip_Item(item);

        if (!success) return false;

        On_Equip_Changed?.Invoke(item);
        return true;
    }

    public bool Is_Equipped(Item_Scriptable item)
    {
        return inventory_Data.Is_Equipped(item);
    }

    public int Get_Addable_Amount(Item_Scriptable item)
    {
        return inventory_Data.Get_Addable_Amount(item);
    }
    public List<Inventory_Slot> Clone_Inventory_Slots(Item_Type type)
    {
        return inventory_Data.Clone_Inventory_Slots(type);
    }
}
