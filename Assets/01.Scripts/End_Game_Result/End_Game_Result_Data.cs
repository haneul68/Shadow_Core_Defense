using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class End_Game_Result_Data
{
    public int cleared_Round;
    public float play_Time;
    public int gold_Reward;
    public int exp_Reward;
    public int score;
    public Sprite player_Sprite;
    public string character_Name;

    public List<Material_Reward_Data> material_Rewards = new List<Material_Reward_Data>();
}