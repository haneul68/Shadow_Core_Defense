using System;
using System.Collections.Generic;
using UnityEngine;

public class Rune_Manager
{
    private Dictionary<string, Rune_Holder> p_Rune_Holder = new Dictionary<string, Rune_Holder>();

    private readonly List<string> equipped_Rune_Id_List = new List<string>();

    private Rune_System_Settings rune_System_Settings;

    public IReadOnlyDictionary<string, Rune_Holder> P_Rune_Holder => p_Rune_Holder;
    public IReadOnlyList<string> Equipped_Rune_Id_List => equipped_Rune_Id_List;

    public event Action<string> On_Rune_Changed;
    public event Action On_Rune_Equipped_Changed;

    public void Init()
    {
        p_Rune_Holder.Clear();
        equipped_Rune_Id_List.Clear();

        rune_System_Settings = Resources.Load<Rune_System_Settings>("Scriptable/Rune/Rune_System_Settings");

        for (int i = 0; i < 4; i++)
        {
            equipped_Rune_Id_List.Add(string.Empty);
        }

        if (Base_Manager.Data_Mng == null)
        {
            Debug.LogWarning("Base_Manager.Data_Mng == null");
            return;
        }

        foreach (var pair in Base_Manager.Data_Mng.Rune_Data)
        {
            if (pair.Value == null) continue;

            Rune_Scriptable rune_Data = pair.Value as Rune_Scriptable;
            if (rune_Data == null) continue;

            if (!p_Rune_Holder.ContainsKey(rune_Data.item_ID))
            {
                Rune_Holder rune_Holder = new Rune_Holder();
                rune_Holder.data = rune_Data;
                rune_Holder.is_Owned = false;
                rune_Holder.level = 0;

                p_Rune_Holder.Add(rune_Data.item_ID, rune_Holder);
            }
        }
    }

    public Rune_Holder Get_Rune_Holder(string rune_Id)
    {
        if (string.IsNullOrEmpty(rune_Id))
            return null;

        if (!p_Rune_Holder.ContainsKey(rune_Id))
            return null;

        return p_Rune_Holder[rune_Id];
    }

    public bool Is_Owned(string rune_Id)
    {
        Rune_Holder rune_Holder = Get_Rune_Holder(rune_Id);
        return rune_Holder != null && rune_Holder.is_Owned;
    }

    public bool Is_Max_Level(string rune_Id)
    {
        Rune_Holder rune_Holder = Get_Rune_Holder(rune_Id);

        if (rune_Holder == null || rune_Holder.data == null)
            return false;

        if (!rune_Holder.is_Owned)
            return false;

        return rune_Holder.level >= rune_Holder.data.max_Level;
    }

    public int Get_Level(string rune_Id)
    {
        Rune_Holder rune_Holder = Get_Rune_Holder(rune_Id);

        if (rune_Holder == null)
            return 0;

        return rune_Holder.level;
    }

    public float Get_Current_Chance(string rune_Id)
    {
        Rune_Holder rune_Holder = Get_Rune_Holder(rune_Id);

        if (rune_Holder == null || rune_Holder.data == null)
            return 0f;

        if (!rune_Holder.is_Owned)
            return rune_Holder.data.craft_Chance;

        if (rune_Holder.level >= rune_Holder.data.max_Level)
            return 0f;

        if (rune_System_Settings == null)
            return 0f;

        return rune_System_Settings.Get_Upgrade_Chance(rune_Holder.data.rarity, rune_Holder.level);
    }

    public List<Rune_Material_Data> Get_Current_Material_List(string rune_Id)
    {
        Rune_Holder rune_Holder = Get_Rune_Holder(rune_Id);

        if (rune_Holder == null || rune_Holder.data == null)
            return new List<Rune_Material_Data>();

        int target_Level = rune_Holder.is_Owned ? rune_Holder.level + 1 : 1;

        Rune_Level_Data level_Data = rune_Holder.data.Get_Level_Data(target_Level);

        if (level_Data == null || level_Data.materials == null)
            return new List<Rune_Material_Data>();

        return level_Data.materials;
    }

    public bool Has_Material(string rune_Id)
    {
        if (Base_Manager.Inventory_Mng == null || Base_Manager.Inventory_Mng.inventory_Logic == null)
            return false;

        List<Rune_Material_Data> material_List = Get_Current_Material_List(rune_Id);

        if (material_List == null || material_List.Count == 0)
            return false;

        for (int i = 0; i < material_List.Count; i++)
        {
            Rune_Material_Data material_Data = material_List[i];

            if (material_Data == null || material_Data.item == null)
                return false;

            int current_Count = Base_Manager.Inventory_Mng.inventory_Logic.Get_Item_Count(material_Data.item.item_ID);

            if (current_Count < material_Data.amount)
                return false;
        }

        return true;
    }

    public bool Can_Craft_Or_Upgrade(string rune_Id)
    {
        Rune_Holder rune_Holder = Get_Rune_Holder(rune_Id);

        if (rune_Holder == null || rune_Holder.data == null)
            return false;

        if (rune_Holder.is_Owned && rune_Holder.level >= rune_Holder.data.max_Level)
            return false;

        return Has_Material(rune_Id);
    }

    public bool Try_Craft_Or_Upgrade(string rune_Id, out bool is_Success)
    {
        is_Success = false;

        Rune_Holder rune_Holder = Get_Rune_Holder(rune_Id);

        if (rune_Holder == null || rune_Holder.data == null)
            return false;

        if (!Can_Craft_Or_Upgrade(rune_Id))
            return false;

        Consume_Material(rune_Id);

        float chance = Get_Current_Chance(rune_Id);
        is_Success = UnityEngine.Random.Range(0f, 100f) <= chance;

        if (is_Success)
        {
            if (!rune_Holder.is_Owned)
            {
                rune_Holder.is_Owned = true;
                rune_Holder.level = 1;
            }
            else
            {
                rune_Holder.level++;
            }
        }

        On_Rune_Changed?.Invoke(rune_Id);
        return true;
    }

    private void Consume_Material(string rune_Id)
    {
        if (Base_Manager.Inventory_Mng == null || Base_Manager.Inventory_Mng.inventory_Logic == null)
            return;

        List<Rune_Material_Data> material_List = Get_Current_Material_List(rune_Id);

        if (material_List == null || material_List.Count == 0)
            return;

        for (int i = 0; i < material_List.Count; i++)
        {
            Rune_Material_Data material_Data = material_List[i];

            if (material_Data == null || material_Data.item == null)
                continue;

            Base_Manager.Inventory_Mng.inventory_Logic.Consume_Item(material_Data.item, material_Data.amount);
        }
    }

    public bool Is_Equipped(string rune_Id)
    {
        if (string.IsNullOrEmpty(rune_Id))
            return false;

        for (int i = 0; i < equipped_Rune_Id_List.Count; i++)
        {
            if (equipped_Rune_Id_List[i] == rune_Id)
                return true;
        }

        return false;
    }

    public bool Try_Equip(string rune_Id)
    {
        Rune_Holder rune_Holder = Get_Rune_Holder(rune_Id);

        if (rune_Holder == null || rune_Holder.data == null)
            return false;

        if (!rune_Holder.is_Owned)
            return false;

        if (Is_Equipped(rune_Id)) 
        {
            return false;
        }

        for (int i = 0; i < equipped_Rune_Id_List.Count; i++)
        {
            if (string.IsNullOrEmpty(equipped_Rune_Id_List[i]))
            {
                equipped_Rune_Id_List[i] = rune_Id;

                On_Rune_Equipped_Changed?.Invoke();
                On_Rune_Changed?.Invoke(rune_Id);
                return true;
            }
        }

        return false;
    }

    public bool Try_UnEquip(string rune_Id)
    {
        if (string.IsNullOrEmpty(rune_Id))
            return false;

        for (int i = 0; i < equipped_Rune_Id_List.Count; i++)
        {
            if (equipped_Rune_Id_List[i] == rune_Id)
            {
                equipped_Rune_Id_List[i] = string.Empty;

                On_Rune_Equipped_Changed?.Invoke();
                On_Rune_Changed?.Invoke(rune_Id);
                return true;
            }
        }

        return false;
    }

    public float Get_Total_Bonus_Percent(Rune_Stat_Type stat_Type)
    {
        float total_Percent = 0f;

        for (int i = 0; i < equipped_Rune_Id_List.Count; i++)
        {
            string rune_Id = equipped_Rune_Id_List[i];

            if (string.IsNullOrEmpty(rune_Id))
                continue;

            Rune_Holder rune_Holder = Get_Rune_Holder(rune_Id);

            if (rune_Holder == null || rune_Holder.data == null)
                continue;

            if (!rune_Holder.is_Owned)
                continue;

            if (rune_Holder.data.stat_Type != stat_Type)
                continue;

            total_Percent += rune_Holder.level * rune_Holder.data.value_Per_Level;
        }

        return total_Percent;
    }
}