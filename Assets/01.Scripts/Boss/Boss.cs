using UnityEngine;

public class Boss : Enemy_Base, IDirection_Provider
{
    private Vector2 last_Facing_Dir = Vector2.down;

    [SerializeField] Health_Manager health_Manager;

    private void Awake()
    {
        Init();
    }

    public Vector2 Last_Facing_Dir
    {
        get => last_Facing_Dir;
        set => last_Facing_Dir = value;
    }

    [SerializeField] private Transform[] boss_Spawn_Points;

    private int current_Index;

    public int Current_Index => current_Index;
    private void OnEnable()
    {
        health_Manager.On_Hit += On_Hit;
    }

    private void OnDisable()
    {
        health_Manager.On_Hit -= On_Hit;
    }

    #region CORE MOVE 
    public void Move_To_Point(int index)
    {
        if (boss_Spawn_Points == null || index < 0 || index >= boss_Spawn_Points.Length)
            return;

        transform.position = boss_Spawn_Points[index].position;

        Set_Facing_By_Index(index);
    }
    #endregion

    #region INIT
    public void Init_Boss(Transform[] spawnPoints, int index)
    {
        Re_Set_Bonus_Stats();

        Set_Round_Enemy_Stat();

        boss_Spawn_Points = spawnPoints;
        current_Index = index;

        health_Manager.Init();

        Move_To_Point(index); 
    }

    private void Set_Round_Enemy_Stat()
    {
        double bonus_ATK = Utils.Calculate_Value(Base_ATK, Round_Manager.Current_Round, 0.8f);
        double bonus_HP = Utils.Calculate_Value(Base_Max_HP, Round_Manager.Current_Round, 1.2f);

        Add_ATK_Buff(bonus_ATK);
        Add_HP_Buff(bonus_HP);
    }
    #endregion

    #region DIRECTION
    private void Set_Facing_By_Index(int index)
    {
        switch (index)
        {
            case 0: last_Facing_Dir = Vector2.down; break;
            case 1: last_Facing_Dir = Vector2.up; break;
            case 2: last_Facing_Dir = Vector2.right; break;
            case 3: last_Facing_Dir = Vector2.left; break; 
        }
    }
    #endregion

    public void On_Hit()
    {
        Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Enemy_Hit);
    }
    public Vector2 Get_Direction()
    {
        return Vector2.zero;
    }
}