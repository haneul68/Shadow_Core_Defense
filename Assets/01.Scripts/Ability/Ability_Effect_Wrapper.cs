using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Ability_Effect_Wrapper
{
    public Effect_Type type;

    public Buff_Effect buff_Effect;
    public Shield_Effect shield_Effect;

    public void Apply(GameObject target)
    {
        switch (type)
        {
            case Effect_Type.Buff:
                buff_Effect?.Apply(target);
                break;

            case Effect_Type.Shield:
                shield_Effect?.Apply(target);
                break;
        }
    }
    public void Add_Description_Values(List<object> values)
    {
        switch (type)
        {
            case Effect_Type.Buff:
                buff_Effect?.Add_Description_Values(values);
                break;

            case Effect_Type.Shield:
                break;
        }
    }
}