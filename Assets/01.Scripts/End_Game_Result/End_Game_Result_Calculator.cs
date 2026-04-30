using UnityEngine;

public static class End_Game_Result_Calculator
{
    public static End_Game_Result_Data Calculate(int clearedRound, float playTime, Sprite playerSprite)
    {
        End_Game_Result_Data data = new End_Game_Result_Data();

        data.cleared_Round = Mathf.Max(0, clearedRound);
        data.play_Time = Mathf.Max(0f, playTime);
        data.player_Sprite = playerSprite;

        data.gold_Reward = Calculate_Gold(data.cleared_Round);
        data.exp_Reward = Calculate_Exp(data.cleared_Round);
        data.score = CalculateScore(data.cleared_Round, data.play_Time);

        data.material_Rewards = Material_Reward_Calculator.Calculate(data.cleared_Round);

        return data;
    }

    private static int Calculate_Gold(int cleared_Round)
    {
        int total = 0;

        for (int round = 1; round <= cleared_Round; round++)
        {
            total += Is_Boss_Round(round) ? round * 30 : round * 10;
        }

        return total;
    }

    private static int Calculate_Exp(int cleared_Round)
    {
        int total = 0;

        for (int round = 1; round <= cleared_Round; round++)
        {
            total += Is_Boss_Round(round) ? round * 45 : round * 15;
        }

        return total;
    }

    private static int CalculateScore(int cleared_Round, float play_Time)
    {
        int round_Score = 0;

        for (int round = 1; round <= cleared_Round; round++)
        {
            round_Score += Is_Boss_Round(round) ? round * 300 : round * 100;
        }

        float target_Time = cleared_Round * 60f;
        float saved_Time = Mathf.Max(0f, target_Time - play_Time);
        int time_Bonus = Mathf.RoundToInt(saved_Time * 20f);

        return round_Score + time_Bonus;
    }

    private static bool Is_Boss_Round(int round)
    {
        return round > 0 && round % Round_Manager.Instance.BOSS_ROUND_INTERVAL == 0;
    }
}