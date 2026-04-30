using UnityEngine;

[CreateAssetMenu(fileName = "Character_Data", menuName = "Character_Data/Character", order = int.MaxValue)]
public class Character_Scriptable : ScriptableObject
{
    [Header("Info")]
    public string Character_Name; // ¿µ¿õ ÀÌ¸§
    public string Character_DES; // ¿µ¿õ ¼³¸í
    public Rarity rarity; // ¿µ¿õ µî±Þ

    [Space(10)]
    [Header("Stat")]
    public double ATK;
    public float Max_HP;
    public float Max_MP;
    public float Max_Stamina;
    public float Speed;

    [Space(10)]
    [Header("Price")]
    public int Price;

    [Space(10)]
    [Header("Prefab")]
    public GameObject Character_Prefab;

    [Space(10)]
    [Header("Level Growth")]
    public double atk_Growth_Per_Level = 3;
    public float hp_Growth_Per_Level = 20f;
    public float mp_Growth_Per_Level = 6f;
    public float stamina_Growth_Per_Level = 5f;
}
