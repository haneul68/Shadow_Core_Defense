using UnityEngine;

public class Enemy : Enemy_Base
{
    [SerializeField] Health_Manager health_Manager;

    private void Awake()
    {
        Init();
    
    }
    public void ReSpawn_Enemy_Init() 
    {
        Reset_Physics();

        Re_Set_Bonus_Stats();

        Set_Round_Enemy_Stat();

        health_Manager.Init();
    }

    private void OnEnable()
    {
        health_Manager.On_Hit += On_Hit;
        health_Manager.On_Died += On_Did;
    }

    private void OnDisable()
    {
        health_Manager.On_Hit -= On_Hit;
        health_Manager.On_Died -= On_Did;
    }
    private void Set_Round_Enemy_Stat() 
    {
        double bonus_ATK = Utils.Calculate_Value(Base_ATK, Round_Manager.Current_Round, 0.52f);
        double bonus_HP = Utils.Calculate_Value(Base_Max_HP, Round_Manager.Current_Round, 0.65f);

        Add_ATK_Buff(bonus_ATK);
        Add_HP_Buff(bonus_HP);
    }
    private void Reset_Physics()
    {
        CircleCollider2D col = GetComponent<CircleCollider2D>();
        if(col != null)
            col.isTrigger = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = true;
            rb.linearVelocity = Vector2.zero;
        }
    }
    public void On_Hit() 
    {
        Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Enemy_Hit);
    }
    public void On_Did() 
    {
        Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Enemy_Death);
    }
}
