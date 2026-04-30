using UnityEngine;

public class Item_Scriptable : ScriptableObject
{
    public string item_ID;
    public string item_Name; // 아이템 이름
    public string item_DES; // 아이템 설명
    public int max_Stack;
    public float item_Value;
    public float cool_Down;

    public Item_Type item_Type; // 아이템 유형
    public Rarity rarity; // 아이템 등급

    public float item_Chance; // 아이템 획득 확률

    public virtual void Use(Character target) { }
}

