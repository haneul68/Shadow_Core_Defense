using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rune_Data", menuName = "Rune_Data/Rune", order = int.MaxValue)]
public class Rune_Scriptable : Item_Scriptable
{
    public int max_Level = 11;

    public Rune_Stat_Type stat_Type;

    public float value_Per_Level;

    [Range(0f, 100f)]
    public float craft_Chance;

    public List<Rune_Level_Data> level_Datas = new List<Rune_Level_Data>();

    public Rune_Level_Data Get_Level_Data(int level) 
    {
        return level_Datas.Find(x=> x.level == level);
    }

    public string Get_Description(int level)
    {
        float value = level * value_Per_Level;
        return string.Format(item_DES, value.ToString("0"));
    }
}
