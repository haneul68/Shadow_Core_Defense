using System.Collections.Generic;
using UnityEngine;

public class Buff_Manager : MonoBehaviour
{
    [SerializeField]
    private Character character;

    private readonly Dictionary<string, Buff> active_Buffs = new Dictionary<string, Buff>();

    private readonly List<string> timed_Buff_Names = new List<string>();

    private readonly List<string> buffs_To_Remove = new List<string>();

    [SerializeField]
    private float buff_Check_Interval = 0.1f;

    private float buff_Check_Timer;

    private void Awake()
    {
        if (character == null)
            character = GetComponent<Character>();
    }

    private void OnEnable()
    {
        Clear_All_Buffs();
    }

    private void Update()
    {
        if (character == null) return;

        if (timed_Buff_Names.Count <= 0) return;

        buff_Check_Timer += Time.deltaTime;

        if (buff_Check_Timer < buff_Check_Interval)
            return;

        float delta_Time = buff_Check_Timer;
        buff_Check_Timer = 0f;

        Update_Timed_Buffs(delta_Time);
    }

    private void Update_Timed_Buffs(float delta_Time)
    {
        buffs_To_Remove.Clear();

        for (int i = timed_Buff_Names.Count - 1; i >= 0; i--)
        {
            string buff_Name = timed_Buff_Names[i];

            if (!active_Buffs.TryGetValue(buff_Name, out Buff buff))
            {
                timed_Buff_Names.RemoveAt(i);
                continue;
            }

            buff.time_Left -= delta_Time;

            if (buff.time_Left <= 0f)
            {
                buffs_To_Remove.Add(buff_Name);
            }
        }

        for (int i = 0; i < buffs_To_Remove.Count; i++)
        {
            Remove_Buff(buffs_To_Remove[i]);
        }
    }

    public void Apply_Buff(Buff new_Buff)
    {
        if (new_Buff == null)
        {
            Debug.Log("new_Buff == null");
            return;
        }

        if (string.IsNullOrEmpty(new_Buff.buff_Name))
        {
            Debug.Log("string.IsNullOrEmpty(new_Buff.buff_Name)");
            return;
        }

        if (active_Buffs.TryGetValue(new_Buff.buff_Name, out Buff existing_Buff))
        {
            if (new_Buff.stackable)
            {
                if (existing_Buff.stack_Count < existing_Buff.max_Stack)
                {
                    Remove_Buff_From_Character(existing_Buff);

                    existing_Buff.stack_Count++;

                    if (existing_Buff.Has_Duration)
                        existing_Buff.time_Left = new_Buff.duration;

                    Apply_Buff_To_Character(existing_Buff);
                }
                else
                {
                    if (existing_Buff.Has_Duration)
                        existing_Buff.time_Left = new_Buff.duration;
                }
            }
            else
            {
                if (existing_Buff.Has_Duration)
                    existing_Buff.time_Left = new_Buff.duration;
            }

            Register_Timed_Buff(existing_Buff);
            return;
        }

        Buff buff_Instance = new_Buff.Clone();
        buff_Instance.time_Left = new_Buff.duration;

        active_Buffs.Add(buff_Instance.buff_Name, buff_Instance);

        Register_Timed_Buff(buff_Instance);
        Apply_Buff_To_Character(buff_Instance);
    }

    private void Register_Timed_Buff(Buff buff)
    {
        if (buff == null) return;

        if (!buff.Has_Duration)
            return;

        if (!timed_Buff_Names.Contains(buff.buff_Name))
            timed_Buff_Names.Add(buff.buff_Name);
    }

    public void Remove_Buff_By_Name(string buff_Name)
    {
        Remove_Buff(buff_Name);
    }

    private void Remove_Buff(string buff_Name)
    {
        if (!active_Buffs.TryGetValue(buff_Name, out Buff buff))
            return;

        Remove_Buff_From_Character(buff);

        active_Buffs.Remove(buff_Name);
        timed_Buff_Names.Remove(buff_Name);
    }

    private void Apply_Buff_To_Character(Buff buff)
    {
        character.Add_ATK_Buff(Calculate_ATK_Bonus(buff));
        character.Add_HP_Buff(Calculate_HP_Bonus(buff));
        character.Add_MP_Buff(Calculate_MP_Bonus(buff));
        character.Add_Stamina_Buff(Calculate_Stamina_Bonus(buff));
        character.Add_Speed_Buff(Calculate_Speed_Bonus(buff));
    }

    private void Remove_Buff_From_Character(Buff buff)
    {
        character.Remove_ATK_Buff(Calculate_ATK_Bonus(buff));
        character.Remove_HP_Buff(Calculate_HP_Bonus(buff));
        character.Remove_MP_Buff(Calculate_MP_Bonus(buff));
        character.Remove_Stamina_Buff(Calculate_Stamina_Bonus(buff));
        character.Remove_Speed_Buff(Calculate_Speed_Bonus(buff));
    }

    private double Calculate_ATK_Bonus(Buff buff)
    {
        return character.Base_ATK * (buff.atk_Bonus_Percent / 100.0) * buff.stack_Count;
    }

    private double Calculate_HP_Bonus(Buff buff)
    {
        return character.Base_Max_HP * (buff.hp_Bonus_Percent / 100.0) * buff.stack_Count;
    }

    private float Calculate_MP_Bonus(Buff buff)
    {
        return character.Base_Max_MP * (buff.mp_Bonus_Percent / 100f) * buff.stack_Count;
    }

    private float Calculate_Stamina_Bonus(Buff buff)
    {
        return character.Base_Max_Stamina * (buff.stamina_Bonus_Percent / 100f) * buff.stack_Count;
    }

    private float Calculate_Speed_Bonus(Buff buff)
    {
        return character.Base_Move_Speed * (buff.speed_Bonus_Percent / 100f) * buff.stack_Count;
    }

    public List<Buff> Get_Active_Buffs()
    {
        return new List<Buff>(active_Buffs.Values);
    }

    public void Clear_All_Buffs()
    {
        if (character == null) return;

        foreach (var buff in active_Buffs.Values)
        {
            Remove_Buff_From_Character(buff);
        }

        active_Buffs.Clear();
        timed_Buff_Names.Clear();
        buffs_To_Remove.Clear();
        buff_Check_Timer = 0f;
    }
}