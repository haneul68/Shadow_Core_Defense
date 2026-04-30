using System;
using System.Collections.Generic;
using UnityEngine;

public class Character_Manager
{
    public Character current_Character;

    private string equipped_Character_Name;

    public Stack<GameObject> Character_Action_Panel_Holder = new Stack<GameObject>();
    public Stack<GameObject> Character_Purchase_Panel_Holder = new Stack<GameObject>();

    public string Equipped_Character_Name => equipped_Character_Name;

    public event Action<string> On_Character_Changed;
    public event Action<string> On_Character_Equipped;
    public event Action<Character> On_Current_Character_Changed;

    public void Init()
    {
        equipped_Character_Name = string.Empty;
    }

    #region Character_State
    public bool Is_Owned_Character(string ch_Name)
    {
        if (!Base_Manager.Data_Mng.p_Character_Holder.ContainsKey(ch_Name))
            return false;

        return Base_Manager.Data_Mng.p_Character_Holder[ch_Name].holder.Quantity > 0;
    }

    public bool Is_Equipped_Character(string ch_Name)
    {
        return equipped_Character_Name == ch_Name;
    }

    public bool Equip_Character(string ch_Name)
    {
        if (!Base_Manager.Data_Mng.p_Character_Holder.ContainsKey(ch_Name))
        {
            Debug.LogWarning($"캐릭터 : {ch_Name} 없음");
            return false;
        }

        Character_Holder ch_Holder = Base_Manager.Data_Mng.p_Character_Holder[ch_Name];

        if (ch_Holder.holder.Quantity <= 0)
        {
            Debug.LogWarning($"미보유 캐릭터 : {ch_Name}");
            return false;
        }

        equipped_Character_Name = ch_Name;

        Lobby_Canvas.Instance.Get_Text_Pop_Up($"{Base_Manager.Data_Mng.d_Character_Data[equipped_Character_Name].Character_Name}을 장착하였습니다", Color.white);

        On_Character_Equipped?.Invoke(ch_Name);
        On_Character_Changed?.Invoke(ch_Name);
        return true;
    }

    public bool UnEquip_Character(string ch_Name)
    {
        if (string.IsNullOrEmpty(equipped_Character_Name))
            return false;

        if (equipped_Character_Name != ch_Name)
            return false;

        Lobby_Canvas.Instance.Get_Text_Pop_Up($"{Base_Manager.Data_Mng.d_Character_Data[equipped_Character_Name].Character_Name}을 장착 해제하였습니다", Color.white);

        equipped_Character_Name = string.Empty;


        On_Character_Equipped?.Invoke(ch_Name);
        On_Character_Changed?.Invoke(ch_Name);
        return true;
    }

    public bool Buy_Character(string ch_Name)
    {
        if (!Base_Manager.Data_Mng.p_Character_Holder.ContainsKey(ch_Name))
        {
            Debug.LogWarning($"캐릭터 : {ch_Name} 없음");
            return false;
        }

        Character_Holder ch_Holder = Base_Manager.Data_Mng.p_Character_Holder[ch_Name];

        if (ch_Holder.holder.Quantity > 0)
        {
            Debug.LogWarning($"이미 보유 중 : {ch_Name}");
            return false;
        }

        int price = ch_Holder.Data.Price;

        if (!Base_Manager.Data_Mng.Spend_Diamond(price))
        {
            Debug.LogWarning($"다이아 부족 : 필요 {price}, 보유 {Base_Manager.Data_Mng.Diamond}");
            return false;
        }

        ch_Holder.holder.Quantity = 1;
        ch_Holder.holder.Level = 1;
        ch_Holder.Exp = 0;

        On_Character_Changed?.Invoke(ch_Name);
        return true;
    }
    #endregion

    #region Exp
    public void Add_Character_Exp(string ch_Name, int amount)
    {
        if (!Base_Manager.Data_Mng.p_Character_Holder.ContainsKey(ch_Name))
        {
            Debug.LogWarning($"캐릭터 : {ch_Name} 없음");
            return;
        }

        if (amount <= 0) return;

        Character_Holder ch_Holder = Base_Manager.Data_Mng.p_Character_Holder[ch_Name];

        if (ch_Holder.holder.Quantity <= 0)
        {
            Debug.LogWarning($"미보유 캐릭터 : {ch_Name}");
            return;
        }

        ch_Holder.Exp += amount;

        while (ch_Holder.Exp >= Get_Need_Exp(ch_Name))
        {
            ch_Holder.Exp -= Get_Need_Exp(ch_Name);
            ch_Holder.holder.Level++;
        }

        On_Character_Changed?.Invoke(ch_Name);
    }

    public int Get_Need_Exp(string ch_Name)
    {
        if (!Base_Manager.Data_Mng.p_Character_Holder.ContainsKey(ch_Name))
        {
            Debug.LogWarning($"캐릭터 : {ch_Name} 없음");
            return 1;
        }

        Character_Holder ch_Holder = Base_Manager.Data_Mng.p_Character_Holder[ch_Name];
        int level = ch_Holder.holder.Level;

        return 100 + ((level - 1) * 50);
    }

    public float Get_Exp_Fill_Amount(string ch_Name)
    {
        if (!Base_Manager.Data_Mng.p_Character_Holder.ContainsKey(ch_Name))
        {
            Debug.LogWarning($"캐릭터 : {ch_Name} 없음");
            return 0f;
        }

        Character_Holder ch_Holder = Base_Manager.Data_Mng.p_Character_Holder[ch_Name];

        if (ch_Holder.holder.Quantity <= 0)
            return 0f;

        int need_Exp = Get_Need_Exp(ch_Name);
        if (need_Exp <= 0) return 0f;

        return (float)ch_Holder.Exp / need_Exp;
    }

    public int Get_Level(string ch_Name)
    {
        if (!Base_Manager.Data_Mng.p_Character_Holder.ContainsKey(ch_Name))
        {
            Debug.LogWarning($"캐릭터 : {ch_Name} 없음");
            return 1;
        }

        return Base_Manager.Data_Mng.p_Character_Holder[ch_Name].holder.Level;
    }
    #endregion

    #region GET_STAT
    public double Get_ATK(string ch_Name)
    {
        if (!Base_Manager.Data_Mng.p_Character_Holder.ContainsKey(ch_Name))
        {
            Debug.LogWarning($"캐릭터 : {ch_Name} 없음");
            return 0;
        }

        Character_Holder ch_Holder = Base_Manager.Data_Mng.p_Character_Holder[ch_Name];
        double base_ATK = ch_Holder.Data.ATK;
        int level = ch_Holder.holder.Level;
        double atk = base_ATK + ((level - 1) * ch_Holder.Data.atk_Growth_Per_Level);

        return atk;
    }

    public float Get_Max_HP(string ch_Name)
    {
        if (!Base_Manager.Data_Mng.p_Character_Holder.ContainsKey(ch_Name))
        {
            Debug.LogWarning($"캐릭터 : {ch_Name} 없음");
            return 0;
        }

        Character_Holder ch_Holder = Base_Manager.Data_Mng.p_Character_Holder[ch_Name];
        float base_Max_HP = ch_Holder.Data.Max_HP;
        int level = ch_Holder.holder.Level;
        float max_HP = base_Max_HP + ((level - 1) * ch_Holder.Data.hp_Growth_Per_Level);

        return max_HP;
    }

    public float Get_Max_MP(string ch_Name)
    {
        if (!Base_Manager.Data_Mng.p_Character_Holder.ContainsKey(ch_Name))
        {
            Debug.LogWarning($"캐릭터 : {ch_Name} 없음");
            return 0;
        }

        Character_Holder ch_Holder = Base_Manager.Data_Mng.p_Character_Holder[ch_Name];
        float base_Max_MP = ch_Holder.Data.Max_MP;
        int level = ch_Holder.holder.Level;
        float max_MP = base_Max_MP + ((level - 1) * ch_Holder.Data.mp_Growth_Per_Level);

        return max_MP;
    }

    public float Get_Max_Stamina(string ch_Name)
    {
        if (!Base_Manager.Data_Mng.p_Character_Holder.ContainsKey(ch_Name))
        {
            Debug.LogWarning($"캐릭터 : {ch_Name} 없음");
            return 0;
        }

        Character_Holder ch_Holder = Base_Manager.Data_Mng.p_Character_Holder[ch_Name];
        float base_Max_Stamina = ch_Holder.Data.Max_Stamina;
        int level = ch_Holder.holder.Level;
        float max_Stamina = base_Max_Stamina + ((level - 1) * ch_Holder.Data.stamina_Growth_Per_Level);

        return max_Stamina;
    }

    public float Get_Move_Speed(string ch_Name)
    {
        if (!Base_Manager.Data_Mng.p_Character_Holder.ContainsKey(ch_Name))
        {
            Debug.LogWarning($"캐릭터 : {ch_Name} 없음");
            return 0;
        }

        Character_Holder ch_Holder = Base_Manager.Data_Mng.p_Character_Holder[ch_Name];
        return ch_Holder.Data.Speed;
    }
    #endregion

    public void Set_Current_Character(Character character)
    {
        current_Character = character;
        On_Current_Character_Changed?.Invoke(character);
    }
}