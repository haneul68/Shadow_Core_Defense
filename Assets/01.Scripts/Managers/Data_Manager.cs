using System;
using System.Collections.Generic;
using UnityEngine;

public class Character_Holder
{
    public Character_Scriptable Data;
    public Holder holder;

    public int Exp = 0;
}
public class Item_Holder
{
    public Item_Scriptable Data;
    public Holder holder;
}

public class Holder
{
    public int Level = 0;
    public int Quantity = 0;
}

public class Data_Manager
{

    public Dictionary<string, Character_Scriptable> d_Character_Data = new Dictionary<string, Character_Scriptable>();

    public Dictionary<string, Character_Holder> p_Character_Holder = new Dictionary<string, Character_Holder>();

    private Dictionary<string, Item_Scriptable> d_Item_Data = new Dictionary<string, Item_Scriptable>();
    public IReadOnlyDictionary<string, Item_Scriptable> Item_Data => d_Item_Data;

    private Dictionary<string, Item_Scriptable> d_Rune_Data = new Dictionary<string, Item_Scriptable>();
    public IReadOnlyDictionary<string, Item_Scriptable> Rune_Data => d_Rune_Data;

    private Dictionary<string, Enemy_Scriptable> d_Enemy_Data = new Dictionary<string, Enemy_Scriptable>();
    public IReadOnlyDictionary<string, Enemy_Scriptable> D_Enemy_Data => d_Enemy_Data;

    private int gold = 0;
    public int Gold => gold;

    private int diamond = 0;
    public int Diamond => diamond;

    public event Action<int> On_Gold_Changed;
    public event Action<int> On_Diamond_Changed;

    public void Init() 
    {
        Set_Character_Data();
        Set_Enemy_Data();
        Set_Item_Data();
        Set_Rune_Data();

        gold = 1000;
        diamond = 1500;
    }

    #region Data_Set
    /// <summary>
    /// 캐릭터 기본 데이터 및 보유 캐릭터 데이터 초기화
    /// </summary>
    private void Set_Character_Data()
    {
        var datas = Resources.LoadAll<Character_Scriptable>("Scriptable/Character");

        foreach (var data in datas)
        {
            if (!d_Character_Data.ContainsKey(data.name))
            {
                d_Character_Data.Add(data.name, data);
            }


            if (!p_Character_Holder.ContainsKey(data.name))
            {
                Character_Holder character_Holder = new Character_Holder();
                character_Holder.Data = data;
                character_Holder.holder = new Holder();
                p_Character_Holder.Add(data.name, character_Holder);
            }
            else
            {
                //TODO: 저장 되어있던 값 p_Character_Holder에 할당
            }

            Debug.Log($"{data.Character_Name}");
        }
    }

    /// <summary>
    /// 아이템 기본 데이터 초기화
    /// </summary>
    private void Set_Item_Data()
    {
        var datas = Resources.LoadAll<Item_Scriptable>("Scriptable/Item");

        foreach (var data in datas)
        {
            var item = new Item_Scriptable();
            item = data;

            if (!d_Item_Data.ContainsKey(data.item_ID))
            {
                d_Item_Data.Add(data.item_ID, item);
            }
        }
    }
    public bool Get_Item_Data(string item_Id, out Item_Scriptable item_Data)
    {
        return Item_Data.TryGetValue(item_Id, out item_Data);
    }

    /// <summary>
    /// Enemy 기본 데이터 초기화
    /// </summary>
    private void Set_Enemy_Data()
    {
        var datas = Resources.LoadAll<Enemy_Scriptable>("Scriptable/Enemy");

        foreach (var data in datas)
        {
            var enemy = new Enemy_Scriptable();
            enemy = data;

            if (!d_Enemy_Data.ContainsKey(data.name))
            {
                d_Enemy_Data.Add(data.name, enemy);
            }
        }
    }

    /// <summary>
    /// 룬 기본 데이터 초기화
    /// </summary>
    private void Set_Rune_Data()
    {
        var datas = Resources.LoadAll<Item_Scriptable>("Scriptable/Rune");

        foreach (var data in datas)
        {
            var item = new Item_Scriptable();
            item = data;

            if (!d_Rune_Data.ContainsKey(data.item_ID))
            {
                d_Rune_Data.Add(data.item_ID, item);
            }
        }
    }
    public bool Get_Rune_Data(string item_Id, out Item_Scriptable item_Data)
    {
        return Rune_Data.TryGetValue(item_Id, out item_Data);
    }
    #endregion

    #region  Gold
    public void Add_Gold(int amount)
    {
        if (amount <= 0) return;
        gold += amount;

        On_Gold_Changed?.Invoke(gold);
    }

    public bool Spend_Gold(int amount)
    {
        if (amount <= 0 || amount > gold) return false;

        gold -= amount;

        On_Gold_Changed?.Invoke(gold);
       
        return true;
    }
    public void Add_Diamond(int amount)
    {
        if (amount <= 0) return;

        diamond += amount;
        On_Diamond_Changed?.Invoke(diamond);
    }

    public bool Spend_Diamond(int amount)
    {
        if (amount <= 0 || amount > diamond) return false;

        diamond -= amount;
        On_Diamond_Changed?.Invoke(diamond);
        return true;
    }
    #endregion

    #region Exp
    public void Add_Character_Exp(string ch_Name, int amount)
    {
        if (!p_Character_Holder.ContainsKey(ch_Name)) return;

        Character_Holder ch_Holder = p_Character_Holder[ch_Name];

        if (ch_Holder.holder.Quantity <= 0) return;

        ch_Holder.Exp += amount;

        while (ch_Holder.Exp >= Get_Need_Exp(ch_Holder.holder.Level))
        {
            ch_Holder.Exp -= Get_Need_Exp(ch_Holder.holder.Level);
            ch_Holder.holder.Level++;
        }
    }

    public int Get_Need_Exp(int level)
    {
        return 100 + (level - 1) * 50;
    }
    #endregion
}
