using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Data
{
    private Dictionary<Item_Type, List<Inventory_Slot>> inventory_Slots = new Dictionary<Item_Type, List<Inventory_Slot>>();
    private List<Equipped_Slot> equipped_Slots = new List<Equipped_Slot>();

    public IReadOnlyDictionary<Item_Type, List<Inventory_Slot>> Inventory_Slots => inventory_Slots;

    public IReadOnlyList<Equipped_Slot> Equipped_Slots => equipped_Slots;

    public readonly int default_Slot_Size = 11;
    public readonly int default_Equipped_Slot_Size = 5;

    public void Init()
    {
        equipped_Slots.Clear();

        foreach (Item_Type type in Enum.GetValues(typeof(Item_Type)))
        {
            if (type == Item_Type.None) continue;
            inventory_Slots[type] = new List<Inventory_Slot>();
            for (int i = 0; i < default_Slot_Size; i++)
                inventory_Slots[type].Add(new Inventory_Slot());
        }

        for (int i = 0; i < default_Equipped_Slot_Size; i++)
        {
            equipped_Slots.Add(new Equipped_Slot());
        }
    }

    public void Get_Inventory_Size(Item_Type type, out int size)
    {
        size = inventory_Slots[type].Count;
    }

    public bool Can_Add_Item(Item_Scriptable item, int amount)
    {
        if (!inventory_Slots.ContainsKey(item.item_Type)) return false;

        var slots = inventory_Slots[item.item_Type];
        int remaining = amount;

        foreach (var slot in slots)
        {
            if (slot.item == item && slot.quantity < item.max_Stack)
            {
                int canAdd = item.max_Stack - slot.quantity;
                remaining -= canAdd;
                if (remaining <= 0) return true;
            }
        }

        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
            {
                int canAdd = item.max_Stack;
                remaining -= canAdd;
                if (remaining <= 0) return true;
            }
        }

        return false;
    }

    public bool Add_Item(Item_Scriptable item, int amount)
    {
        if (!inventory_Slots.ContainsKey(item.item_Type)) return false;

        var slots = inventory_Slots[item.item_Type];
        int remaining = amount;

        // 기존 슬롯에 채우기
        foreach (var slot in slots)
        {
            if (slot.item == item && slot.quantity < item.max_Stack)
            {
                int canAdd = Math.Min(item.max_Stack - slot.quantity, remaining);
                slot.quantity += canAdd;
                remaining -= canAdd;
                if (remaining <= 0) return true;
            }
        }

        // 빈 슬롯에 새로 추가
        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
            {
                int toAdd = Math.Min(item.max_Stack, remaining);
                slot.item = item;
                slot.quantity = toAdd;
                remaining -= toAdd;
                if (remaining <= 0) return true;
            }
        }

        // 모든 슬롯이 다 차면 실패
        return false;
    }

    // 슬롯 단위 사용
    public bool Use_Item(Item_Scriptable item, int amount)
    {
        if (!inventory_Slots.ContainsKey(item.item_Type)) return false;

        var slots = inventory_Slots[item.item_Type];
        int remaining = amount;

        foreach (var slot in slots)
        {
            if (slot.item != item) continue;

            if (slot.quantity >= remaining)
            {
                slot.quantity -= remaining;
                if (slot.quantity == 0) slot.item = null;

                Remove_Equipped_If_Needed(item);
                return true;
            }
            else
            {
                remaining -= slot.quantity;
                slot.quantity = 0;
                slot.item = null;
            }
        }

        Remove_Equipped_If_Needed(item);
        return false; // 사용할 슬롯 부족
    }

    public bool Use_Slot(Item_Type type, int slot_Index, int amount)
    {
        if (!inventory_Slots.ContainsKey(type)) return false;

        var slot = inventory_Slots[type][slot_Index];

        if (slot.IsEmpty || slot.quantity < amount) return false;

        Item_Scriptable used_Item = slot.item;

        slot.quantity -= amount;
        if (slot.quantity == 0) slot.item = null;

        Remove_Equipped_If_Needed(used_Item);

        return true;
    }

    public int Get_Quantity(Item_Scriptable item)
    {
        int total = 0;
        foreach (var slot in inventory_Slots[item.item_Type])
        {
            if (slot.item == item)
                total += slot.quantity;
        }
        return total;
    }
    public int Get_Quantity(string itemID)
    {
        int total = 0;

        foreach (var slotList in inventory_Slots.Values)
        {
            foreach (var slot in slotList)
            {
                if (slot.item != null && slot.item.item_ID == itemID)
                {
                    total += slot.quantity;
                }
            }
        }
        return total;
    }

    #region Equipped
    // [ADD] 해당 아이템이 장착 중인지
    public bool Is_Equipped(Item_Scriptable item)
    {
        if (item == null) return false;

        for (int i = 0; i < equipped_Slots.Count; i++)
        {
            if (equipped_Slots[i].item == item)
                return true;
        }

        return false;
    }

    // [ADD] 장착 시도
    public bool Try_Equip_Item(Item_Scriptable item)
    {
        if (item == null) return false;

        // 인벤토리에 실제로 있어야 함
        if (Get_Quantity(item) <= 0) return false;

        // 이미 장착 중이면 중복 장착 막기
        if (Is_Equipped(item)) return false;

        for (int i = 0; i < equipped_Slots.Count; i++)
        {
            if (equipped_Slots[i].IsEmpty)
            {
                equipped_Slots[i].item = item;
                return true;
            }
        }

        // 빈 장착 슬롯 없음
        return false;
    }

    // [ADD] 장착 해제
    public bool UnEquip_Item(Item_Scriptable item)
    {
        if (item == null) return false;

        for (int i = 0; i < equipped_Slots.Count; i++)
        {
            if (equipped_Slots[i].item == item)
            {
                equipped_Slots[i].item = null;
                return true;
            }
        }

        return false;
    }

    // [ADD] 아이템이 인벤토리에서 완전히 사라졌으면 장착 해제
    private void Remove_Equipped_If_Needed(Item_Scriptable item)
    {
        if (item == null) return;

        if (Get_Quantity(item) > 0) return;

        UnEquip_Item(item);
    }
    #endregion


    #region End_Game_Item
    public int Get_Addable_Amount(Item_Scriptable item)
    {
        if (item == null) return 0;
        if (!inventory_Slots.ContainsKey(item.item_Type)) return 0;

        var slots = inventory_Slots[item.item_Type];
        int total_Addable = 0;

        foreach (var slot in slots)
        {
            if (slot.item == item)
            {
                total_Addable += Mathf.Max(0, item.max_Stack - slot.quantity);
            }
            else if (slot.IsEmpty)
            {
                total_Addable += item.max_Stack;
            }
        }

        return total_Addable;
    }
    public List<Inventory_Slot> Clone_Inventory_Slots(Item_Type type)
    {
        List<Inventory_Slot> cloned = new List<Inventory_Slot>();

        if (!inventory_Slots.ContainsKey(type))
            return cloned;

        var source = inventory_Slots[type];

        for (int i = 0; i < source.Count; i++)
        {
            Inventory_Slot slot = source[i];
            cloned.Add(new Inventory_Slot(slot.item, slot.quantity));
        }

        return cloned;
    }
    #endregion
}