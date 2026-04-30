using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rune_System_Settings", menuName = "Scriptable Objects/Rune_System_Settings")]
public class Rune_System_Settings : ScriptableObject
{
    public List<Rune_Rarity_Chance_Data> rarity_Chance_Datas = new List<Rune_Rarity_Chance_Data>();

    public float Get_Upgrade_Chance(Rarity rarity, int current_Level) 
    {
        Rune_Rarity_Chance_Data data = rarity_Chance_Datas.Find(x => x.rarity == rarity);

        if (data == null) 
        {
            Debug.Log("data == null");
            return 0f;
        }

        int index = current_Level - 1;
        if (index < 0 || index >= data.chances.Count)
        {
            Debug.Log("index < 0 || index >= data.chances.Count");
            return 0f;
        }

        return data.chances[index];
    }
}
