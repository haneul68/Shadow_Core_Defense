using System;
using UnityEngine;

public abstract class Character : MonoBehaviour, IHealth_Source, IMinimap_Target
{
    [SerializeField] private string character_Name;
    [SerializeField]
    protected Health_Manager health_Manager;

    #region Base Stats (타이틀/로비에서 기본값)
    [SerializeField] private double base_ATK;
    private double base_Max_HP;
    private float base_Max_MP;
    private float base_Max_Stamina;
    private float base_Move_Speed;
    #endregion

    #region Runtime Modifiers (버프/디버프 전용, 인게임)
    [SerializeField] private double bonus_ATK;
    private double bonus_Max_HP;
    private float bonus_Max_MP;
    private float bonus_Max_Stamina;
    private float bonus_Move_Speed;
    #endregion

    #region Final Stats (실제 인게임에서 사용)
    public double Final_ATK => Apply_Rune_Percent(base_ATK + bonus_ATK, Rune_Stat_Type.Attack);
    public double Final_Max_HP => Apply_Rune_Percent(base_Max_HP + bonus_Max_HP, Rune_Stat_Type.HP);
    public float Final_Max_MP => Apply_Rune_Percent(base_Max_MP + bonus_Max_MP, Rune_Stat_Type.Mana);
    public float Final_Max_Stamina => Apply_Rune_Percent(base_Max_Stamina + bonus_Max_Stamina, Rune_Stat_Type.Stamina);
    public float Final_Move_Speed => Apply_Rune_Percent(base_Move_Speed + bonus_Move_Speed, Rune_Stat_Type.Speed);
    #endregion

    #region Current Stats
    private double current_HP;
    private float current_MP;
    private float current_Stamina;

    public RectTransform minimap_Icon;
    public RectTransform Get_Minimap_Icon() => minimap_Icon;    

    public double Current_HP
    {
        get => current_HP;
        set => current_HP = Math.Clamp(value, 0, Final_Max_HP);
    }

    public float Current_MP
    {
        get => current_MP;
        set => current_MP = Mathf.Clamp(value, 0, Final_Max_MP);
    }

    public float Current_Stamina
    {
        get => current_Stamina;
        set => current_Stamina = Mathf.Clamp(value, 0, Final_Max_Stamina);
    }
    #endregion

    #region Buff / Debuff 적용 함수
    public void Add_ATK_Buff(double amount) => bonus_ATK += amount;
    public void Remove_ATK_Buff(double amount) => bonus_ATK -= amount;

    public void Add_HP_Buff(double amount)
    {
        bonus_Max_HP += amount;
        Current_HP += amount;
        Current_HP = Math.Clamp(Current_HP, 0, Final_Max_HP);

        health_Manager.Refresh_Health_UI();
    }
    public void Remove_HP_Buff(double amount)
    {
        bonus_Max_HP -= amount;
        Current_HP = Math.Clamp(Current_HP, 0, Final_Max_HP);

        health_Manager.Refresh_Health_UI();
    }

    public void Add_MP_Buff(float amount)
    {
        bonus_Max_MP += amount;
        Current_MP += amount;
        Current_MP = Mathf.Clamp(Current_MP, 0, Final_Max_MP);
    }
    public void Remove_MP_Buff(float amount)
    {
        bonus_Max_MP -= amount;
        Current_MP = Mathf.Clamp(Current_MP, 0, Final_Max_MP);
    }

    public void Add_Stamina_Buff(float amount)
    {
        bonus_Max_Stamina += amount;
        Current_Stamina = Mathf.Clamp(Current_Stamina, 0, Final_Max_Stamina);
    }
    public void Remove_Stamina_Buff(float amount)
    {
        bonus_Max_Stamina -= amount;
        Current_Stamina = Mathf.Clamp(Current_Stamina, 0, Final_Max_Stamina);
    }

    public void Add_Speed_Buff(float amount) => bonus_Move_Speed += amount;
    public void Remove_Speed_Buff(float amount) => bonus_Move_Speed -= amount;
    #endregion

    #region Base Stat Properties (읽기/쓰기)
    public double Base_ATK
    {
        get => base_ATK;
        set => base_ATK = Math.Max(0, value);
    }

    public double Base_Max_HP
    {
        get => base_Max_HP;
        set => base_Max_HP = Math.Max(0, value);
    }

    public float Base_Max_MP
    {
        get => base_Max_MP;
        set => base_Max_MP = Mathf.Max(0, value);
    }

    public float Base_Max_Stamina
    {
        get => base_Max_Stamina;
        set => base_Max_Stamina = Mathf.Max(0, value);
    }

    public float Base_Move_Speed
    {
        get => base_Move_Speed;
        set => base_Move_Speed = Mathf.Max(0, value);
    }
    #endregion

    public virtual void Init(Character_Scriptable data)
    {
        base_ATK = Base_Manager.Character_Mng.Get_ATK(data.name);
        base_Max_HP = Base_Manager.Character_Mng.Get_Max_HP(data.name);
        base_Max_MP = Base_Manager.Character_Mng.Get_Max_MP(data.name);
        base_Max_Stamina = Base_Manager.Character_Mng.Get_Max_Stamina(data.name);
        base_Move_Speed = Base_Manager.Character_Mng.Get_Move_Speed(data.name);

        current_HP = base_Max_HP;
        current_MP = base_Max_MP;
        current_Stamina = base_Max_Stamina;

        GetComponent<Player_Skill_Manager>()?.Init_Skills();

        Debug.Log($"Character Initialized: ATK={base_ATK}, MaxHP={base_Max_HP}, CurrentHP={current_HP}, MaxMP={base_Max_MP}, CurrentMP={current_MP}, MaxStamina={base_Max_Stamina}, CurrentStamina={current_Stamina}, MoveSpeed={base_Move_Speed}");
    }

    public Transform Get_Transform() => transform;

    #region Rune
    private float Get_Rune_Bonus_Percent(Rune_Stat_Type stat_Type)
    {
        if (Base_Manager.Rune_Mng == null)
            return 0f;

        return Base_Manager.Rune_Mng.Get_Total_Bonus_Percent(stat_Type);
    }

    private double Apply_Rune_Percent(double value, Rune_Stat_Type stat_Type)
    {
        float rune_Percent = Get_Rune_Bonus_Percent(stat_Type);
        return value * (1f + rune_Percent / 100f);
    }

    private float Apply_Rune_Percent(float value, Rune_Stat_Type stat_Type)
    {
        float rune_Percent = Get_Rune_Bonus_Percent(stat_Type);
        return value * (1f + rune_Percent / 100f);
    }
    #endregion
}