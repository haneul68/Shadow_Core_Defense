using UnityEngine;

[CreateAssetMenu(fileName = "Enemy_Data", menuName = "Enemy_Data/Enemy", order = int.MaxValue)]
public class Enemy_Scriptable : ScriptableObject
{
    [Header("Info")]
    public string Enemy_Name; 
    public string Enemy_DES;
    public Enemy_Type Enemy_Type;

    [Space(10)]
    [Header("Stat")]
    public double ATK;
    public float Max_HP;
    public float Speed;
    public float Attack_Distance = 1.0f;
}
