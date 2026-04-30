using UnityEngine;

public class Enemy_Manager : MonoBehaviour
{
    #region GET_STAT
    public double Get_ATK(string name)
    {
        if (!Base_Manager.Data_Mng.D_Enemy_Data.ContainsKey(name))
        {
            Debug.LogWarning($"캐릭터 : {name} 없음");
            return 0;
        }

        Enemy_Scriptable data = Base_Manager.Data_Mng.D_Enemy_Data[name];

        double base_ATK = data.ATK;
        double atk = base_ATK;

        return atk;
    }

    public float Get_Max_HP(string name)
    {
        if (!Base_Manager.Data_Mng.D_Enemy_Data.ContainsKey(name))
        {
            Debug.LogWarning($"캐릭터 : {name} 없음");
            return 0;
        }

        Enemy_Scriptable data = Base_Manager.Data_Mng.D_Enemy_Data[name];

        float base_Max_HP = data.Max_HP;
        float max_HP = base_Max_HP;

        return max_HP;
    }

    public float Get_Move_Speed(string name)
    {
        if (!Base_Manager.Data_Mng.D_Enemy_Data.ContainsKey(name))
        {
            Debug.LogWarning($"캐릭터 : {name} 없음");
            return 0;
        }

        Enemy_Scriptable data = Base_Manager.Data_Mng.D_Enemy_Data[name];

        float base_Speed = data.Speed;

        return base_Speed;
    }

    public Enemy_Type Get_Enemy_Type(string name) 
    {
        if (!Base_Manager.Data_Mng.D_Enemy_Data.ContainsKey(name))
        {
            Debug.LogWarning($"캐릭터 : {name} 없음");
            return 0;
        }

        Enemy_Scriptable data = Base_Manager.Data_Mng.D_Enemy_Data[name];

        Enemy_Type enemy_Type = data.Enemy_Type;

        return enemy_Type;
    }
    public float Get_Enemy_Attack_Distance(string name) 
    {
        if (!Base_Manager.Data_Mng.D_Enemy_Data.ContainsKey(name))
        {
            Debug.LogWarning($"캐릭터 : {name} 없음");
            return 0;
        }

        Enemy_Scriptable data = Base_Manager.Data_Mng.D_Enemy_Data[name];

        float attack_Distance = data.Attack_Distance;

        return attack_Distance;
    }
    #endregion
}
