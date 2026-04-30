using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Rune_Material_Data
{
    public Material_Item item;
    public int amount;
}


[Serializable]
public class Rune_Level_Data
{
    public int level;
    public List<Rune_Material_Data> materials = new List<Rune_Material_Data>();

}

[Serializable]
public class Rune_Rarity_Chance_Data
{
    public Rarity rarity;
    [Tooltip("0 = 1->2")]
    public List<float> chances = new List<float>();
}

public class Rune_Holder 
{
    public Rune_Scriptable data;
    public bool is_Owned;
    public int level;
}
