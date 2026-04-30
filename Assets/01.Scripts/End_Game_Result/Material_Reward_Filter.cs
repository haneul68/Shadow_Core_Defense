using System.Collections.Generic;
using UnityEngine;

public static class Material_Reward_Filter
{
    public static List<Material_Reward_Data> Filter_By_Inventory_Capacity(List<Material_Reward_Data> rewards)
    {
        List<Material_Reward_Data> filtered = new List<Material_Reward_Data>();

        if (rewards == null || rewards.Count == 0)
        {
            Debug.Log("rewards == null || rewards.Count == 0");
            return filtered;
        }
        if (Base_Manager.Inventory_Mng == null || Base_Manager.Inventory_Mng.inventory_Logic == null) 
        {
            Debug.Log("Base_Manager.Inventory_Mng == null || Base_Manager.Inventory_Mng.inventory_Logic == null");
            return filtered;
        }

        List<Inventory_Slot> simulated_Slots = Base_Manager.Inventory_Mng.inventory_Logic.Clone_Inventory_Slots(Item_Type.Material);

        if (simulated_Slots == null || simulated_Slots.Count == 0)
            return filtered;

        for (int i = 0; i < rewards.Count; i++)
        {
            Material_Reward_Data reward = rewards[i];

            if (reward == null || reward.item == null) continue;
            if (reward.amount <= 0) continue;

            int addable_Amount = Simulate_Add_Item(simulated_Slots, reward.item, reward.amount);

            if (addable_Amount <= 0)
                continue;

            filtered.Add(new Material_Reward_Data(reward.item, addable_Amount));
        }

        return filtered;
    }

    private static int Simulate_Add_Item(List<Inventory_Slot> slots, Item_Scriptable item, int amount)
    {
        if (slots == null || item == null || amount <= 0)
            return 0;

        int added = 0;
        int remaining = amount;

        for (int i = 0; i < slots.Count; i++)
        {
            Inventory_Slot slot = slots[i];

            if (slot.item == item && slot.quantity < item.max_Stack)
            {
                int canAdd = Mathf.Min(item.max_Stack - slot.quantity, remaining);
                slot.quantity += canAdd;
                remaining -= canAdd;
                added += canAdd;

                if (remaining <= 0)
                    return added;
            }
        }

        for (int i = 0; i < slots.Count; i++)
        {
            Inventory_Slot slot = slots[i];

            if (slot.IsEmpty)
            {
                int toAdd = Mathf.Min(item.max_Stack, remaining);
                slot.item = item;
                slot.quantity = toAdd;
                remaining -= toAdd;
                added += toAdd;

                if (remaining <= 0)
                    return added;
            }
        }

        return added;
    }
}