using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Ability
{
    public string ability_Name;

    [TextArea(2, 5)]
    public string ablilty_Des;

    public Sprite ablilty_Image;

    [Header("Effects")]
    public List<Ability_Effect_Wrapper> effects = new List<Ability_Effect_Wrapper>();

    public void Apply(GameObject target)
    {
        foreach (var effect in effects)
        {
            effect?.Apply(target);
        }
    }
    public string Get_Description()
    {
        if (string.IsNullOrEmpty(ablilty_Des))
            return string.Empty;

        List<object> values = new List<object>();

        foreach (var effect in effects)
        {
            effect?.Add_Description_Values(values);
        }

        if (values.Count == 0)
            return ablilty_Des;

        return string.Format(ablilty_Des, values.ToArray());
    }
    
}