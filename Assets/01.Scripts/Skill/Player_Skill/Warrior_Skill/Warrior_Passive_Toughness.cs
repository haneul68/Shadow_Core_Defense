using System.Collections;
using UnityEngine;

public class Warrior_Passive_Toughness : Player_Skill_Base
{
    [Header("Base Passive Value")]
    [SerializeField]
    private float atk_Bonus = 5f;
    [SerializeField] 
    private float hp_Bonus = 20f;

    [Header("Bonus Per Round")]
    [SerializeField] 
    private float atk_Bonus_Per_Round = 0.5f;
    [SerializeField] 
    private float hp_Bonus_Per_Round = 1.0f;

    [SerializeField]
    private string buff_Name = "Warrior_Passive_Toughness";

    private Player_Skill_Manager current_Owner;

    protected override object[] Get_Description_Values()
    {
        float total_Atk_Bonus = Get_Total_ATK_Bonus();
        float total_Hp_Bonus = Get_Total_HP_Bonus();

        return new object[]
        {
            total_Atk_Bonus.ToString("F1"),
            total_Hp_Bonus.ToString("F1"),
            atk_Bonus_Per_Round.ToString("F1"),
            hp_Bonus_Per_Round.ToString("F1")
        };
    }

    public override void Apply_Passive(Player_Skill_Manager owner)
    {
        if (owner == null) return;

        current_Owner = owner;

        Buff_Manager buff_Manager = owner.GetComponent<Buff_Manager>();
        if (buff_Manager == null) return;

        buff_Manager.Remove_Buff_By_Name(buff_Name);

        Buff buff = new Buff
        {
            buff_Name = buff_Name,
            duration = -1f,
            time_Left = 0f,
            stackable = false,
            stack_Count = 1,
            atk_Bonus_Percent = Get_Total_ATK_Bonus(),
            hp_Bonus_Percent = Get_Total_HP_Bonus(),
            mp_Bonus_Percent = 0f,
            stamina_Bonus_Percent = 0f,
            speed_Bonus_Percent = 0f
        };

        buff_Manager.Apply_Buff(buff);

        Round_Manager.On_Round_Text_Changed -= On_Round_Changed;
        Round_Manager.On_Round_Text_Changed += On_Round_Changed;
    }

    public override void Remove_Passive(Player_Skill_Manager owner)
    {
        Buff_Manager buff_Manager = owner.GetComponent<Buff_Manager>();
        if (buff_Manager != null)
        {
            buff_Manager.Remove_Buff_By_Name(buff_Name);
        }

        Round_Manager.On_Round_Text_Changed -= On_Round_Changed;

        if (current_Owner == owner)
            current_Owner = null;
    }

    private void On_Round_Changed(int round)
    {
        if (current_Owner == null) return;

        Buff_Manager buff_Manager = current_Owner.GetComponent<Buff_Manager>();
        if (buff_Manager == null) return;

        buff_Manager.Remove_Buff_By_Name(buff_Name);

        Buff buff = new Buff
        {
            buff_Name = buff_Name,
            duration = -1f,
            time_Left = 0f,
            stackable = false,
            stack_Count = 1,
            atk_Bonus_Percent = Get_Total_ATK_Bonus(),
            hp_Bonus_Percent = Get_Total_HP_Bonus(),
            mp_Bonus_Percent = 0f,
            stamina_Bonus_Percent = 0f,
            speed_Bonus_Percent = 0f
        };

        buff_Manager.Apply_Buff(buff);
    }

    protected override IEnumerator Execute(Player_Skill_Manager owner)
    {
        yield break;
    }

    private float Get_Total_ATK_Bonus()
    {
        return atk_Bonus + (Round_Manager.Current_Round * atk_Bonus_Per_Round);
    }

    private float Get_Total_HP_Bonus()
    {
        return hp_Bonus + (Round_Manager.Current_Round * hp_Bonus_Per_Round);
    }
}