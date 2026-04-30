using UnityEngine;

[System.Serializable]
public class Material_Reward_Data
{
    public Material_Item item;
    public int amount;

    public Material_Reward_Data(Material_Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }
}