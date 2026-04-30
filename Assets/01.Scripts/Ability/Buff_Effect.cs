using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Buff_Effect : IAbility_Effect
{
    [Header("Buff Info")]
    public string buff_Name;
    public float duration = 10f;
    public bool stackable;
    public int max_Stack = 1;

    public double atk_Bonus_Percent;
    public double hp_Bonus_Percent;
    public float mp_Bonus_Percent;
    public float stamina_Bonus_Percent;
    public float speed_Bonus_Percent;

    public void Apply(GameObject target)
    {
        Buff_Manager buff_Manager = target.GetComponent<Buff_Manager>();

        if (buff_Manager == null)
        {
            Debug.LogError("Buff_Manager ¾øÀ½");
            return;
        }

        Buff buff = new Buff
        {
            buff_Name = buff_Name,
            duration = duration,
            stackable = stackable,
            atk_Bonus_Percent = atk_Bonus_Percent,
            hp_Bonus_Percent = hp_Bonus_Percent,
            mp_Bonus_Percent = mp_Bonus_Percent,
            stamina_Bonus_Percent = stamina_Bonus_Percent,
            speed_Bonus_Percent = speed_Bonus_Percent,
            max_Stack = max_Stack
        };

        buff_Manager.Apply_Buff(buff);
    }

    public void Add_Description_Values(List<object> values)
    {
        if (atk_Bonus_Percent > 0)
            values.Add(atk_Bonus_Percent.ToString("F0"));

        if (hp_Bonus_Percent > 0)
            values.Add(hp_Bonus_Percent.ToString("F0"));

        if (mp_Bonus_Percent > 0)
            values.Add(mp_Bonus_Percent.ToString("F0"));

        if (stamina_Bonus_Percent > 0)
            values.Add(stamina_Bonus_Percent.ToString("F0"));

        if (speed_Bonus_Percent > 0)
            values.Add(speed_Bonus_Percent.ToString("F0"));
    }
}