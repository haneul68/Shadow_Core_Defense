using System;

[Serializable]
public class Buff
{
    public string buff_Name;
    public float duration = -999f;
    public bool stackable;
    public int max_Stack = 1;

    public double atk_Bonus_Percent;
    public double hp_Bonus_Percent;
    public float mp_Bonus_Percent;
    public float stamina_Bonus_Percent;
    public float speed_Bonus_Percent;

    [NonSerialized] public float time_Left;
    [NonSerialized] public int stack_Count = 1;

    public bool Has_Duration => duration > 0f;

    public Buff Clone()
    {
        Buff clone = (Buff)this.MemberwiseClone();
        clone.time_Left = duration;
        return clone;
    }
}