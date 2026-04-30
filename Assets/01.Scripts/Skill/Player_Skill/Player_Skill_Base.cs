using System.Collections;
using UnityEngine;

public abstract class Player_Skill_Base : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] protected string skill_ID;
    [SerializeField] protected string skill_Name;
    [SerializeField] protected Player_Skill_Type skill_Type;

    [Header("UI")]
    [SerializeField] protected Sprite skill_Icon;

    [TextArea(2, 5)]
    [SerializeField] protected string skill_Desc_Format;

    [Header("Cost")]
    [SerializeField] protected float cooldown = 0f;
    [SerializeField] protected float mana_Cost = 0f;

    public float Cooldown => cooldown;
    public float Mana_Cost => mana_Cost;

    protected float last_Use_Time = -999f;

    public string Skill_ID => skill_ID;
    public string Skill_Name => skill_Name;
    public Player_Skill_Type Skill_Type => skill_Type;
    public Sprite Skill_Icon => skill_Icon;
    public bool Is_Passive => skill_Type == Player_Skill_Type.Passive;

    public virtual string Get_Skill_Name()
    {
        return skill_Name;
    }

    public virtual string Get_Skill_Description()
    {
        object[] values = Get_Description_Values();

        if (string.IsNullOrEmpty(skill_Desc_Format))
            return string.Empty;

        if (values == null || values.Length == 0)
            return skill_Desc_Format;

        return string.Format(skill_Desc_Format, values);
    }

    protected virtual object[] Get_Description_Values()
    {
        return null;
    }

    public virtual bool Can_Use(Player_Skill_Manager owner)
    {
        if (owner == null) return false;
        if (Is_Passive) return false;
        if (Time.time < last_Use_Time + cooldown)
        {
            In_Game_Canvas.Instance.Get_Text_Pop_Up($"아직 사용할 수 없습니다", Color.red);
            return false;
        }

        Mana_Manager mana = owner.GetComponent<Mana_Manager>();
        if (mana != null && mana_Cost > 0 && mana.Current < mana_Cost)
        {
            In_Game_Canvas.Instance.Get_Text_Pop_Up($"마나가 부족합니다", Color.red);
            return false;
        }

        return true;
    }

    public bool Try_Use(Player_Skill_Manager owner)
    {
        if (!Can_Use(owner)) return false;

        Mana_Manager mana = owner.GetComponent<Mana_Manager>();
        if (mana != null && mana_Cost > 0)
        {
            if (!mana.Use(mana_Cost)) 
            {
                return false;
            }
        }

        last_Use_Time = Time.time;
        owner.StartCoroutine(Execute(owner));
        return true;
    }
    public float Get_Remain_Cooldown()
    {
        float remain = (last_Use_Time + cooldown) - Time.time;
        return Mathf.Max(0, remain);
    }
    public virtual void Apply_Passive(Player_Skill_Manager owner) { }
    public virtual void Remove_Passive(Player_Skill_Manager owner) { }

    protected abstract IEnumerator Execute(Player_Skill_Manager owner);

    public virtual void Force_Stop(Player_Skill_Manager owner) { }
}