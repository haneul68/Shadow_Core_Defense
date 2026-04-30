using System;
using UnityEngine;

public class Health_Manager : MonoBehaviour, IDamageable, IStat_Provider
{
    [Header("REF")]
    [SerializeField]
    private GameObject owner;
    [SerializeField]
    private Transform head_Point = null;
    private IHealth_Source health_Source;

    public bool set_Full_Health_On_Start = true;

    public event Action<double, double> On_Value_Changed;
    public event Action On_Hit;
    public event Action On_Died;

    public bool is_Dead { get; set; }

    [SerializeField]
    private bool need_HP_UI = false;
    [SerializeField]
    private bool is_Boss = false;

    private Color damage_Text_Color;

    public double Current => health_Source.Current_HP;
    public double Max => health_Source.Final_Max_HP;

    [SerializeField]
    private float recovery_Per_Second_timer = 1;

    private float timer;

    [SerializeField]
    private float recovery_Per_Second_Percent = 0.0f;

    private UI_Slider slider;
    private UI_Boss_Slider uI_Boss_Slider;

    private void Awake()
    {
        if (health_Source == null)
        {
            if (owner != null)
            {
                health_Source = owner.GetComponent<IHealth_Source>();
            }
        }
    }

    private void Update()
    {
        Recovery_Per_Second(recovery_Per_Second_Percent);
    }

    public void Init()
    {
        if (health_Source == null)
        {
            Debug.Log("(health_Source == null");
            return;
        }

        if (set_Full_Health_On_Start)
        {
            health_Source.Current_HP = health_Source.Final_Max_HP;
        }

        if (need_HP_UI == true)
        {
            damage_Text_Color = Color.white;
            if (is_Boss == true)
            {
                Base_Manager.Pool_Mng.Pooling_OBJ(Pool_ID.Boss_HP_Bar).Get(hp_Bar =>
                {
                    uI_Boss_Slider = hp_Bar.GetComponent<UI_Boss_Slider>();
                    uI_Boss_Slider.transform.localPosition = new Vector2(0, -210);
                    uI_Boss_Slider.Init(this);

                    In_Game_Canvas.Instance.Set_Layer(uI_Boss_Slider, Canvas_Layer.Laver_02);
                });
            }
            else
            {
                Base_Manager.Pool_Mng.Pooling_OBJ(Pool_ID.Enemy_HP_Bar).Get(hp_Bar =>
                {
                    slider = hp_Bar.GetComponent<UI_Slider>();
                    slider.Init(this, head_Point);
                    In_Game_Canvas.Instance.Set_Layer(slider, Canvas_Layer.Layer_01);
                });
            }

        }
        else 
        {
            damage_Text_Color = Color.red;
        }

        is_Dead = false;

        Invoke_On_Health_Change();
    }

    #region Heal
    public void Heal(double amount, bool show_Text = false)
    {
        if (is_Dead) return;

        if(show_Text == true)
            Damage_Text_Manager.Instance.Show_Damage(owner.transform, amount, Color.green);

        health_Source.Current_HP += amount;
        Invoke_On_Health_Change();
    }
    #endregion

    #region Damage
    public void TakeDamage(double damage)
    {
        if (is_Dead) return;

        health_Source.Current_HP -= damage;

        Damage_Text_Manager.Instance.Show_Damage(owner.transform, damage, damage_Text_Color);

        Camera_Shake_Manager.Instance.Shake();

        Invoke_On_Health_Change();

        On_Hit?.Invoke();

        if (health_Source.Current_HP <= 0)
        {
            Die();
        }
    }
    #endregion

    #region Die
    private void Die()
    {
        if (is_Dead) return;

        is_Dead = true;

        if (need_HP_UI == true)
        {
            if (is_Boss)
            {
                Base_Manager.Pool_Mng.pool_Dictionary[Pool_ID.Boss_HP_Bar].Return(uI_Boss_Slider.gameObject);
                Enemy_Spawn_Manager.Instance.Remove_Boss(owner.GetComponent<Enemy_Base>());
            }
            else
            {
                Base_Manager.Instance.Return_Pool_Obj_Delay(Pool_ID.Enemy_HP_Bar, slider.gameObject, 0.8f);
                //Base_Manager.Pool_Mng.pool_Dictionary[Pool_ID.Enemy_HP_Bar].Return(slider.gameObject);
                Enemy_Spawn_Manager.Instance.Remove_Enemy(owner.GetComponent<Enemy_Base>());

                CircleCollider2D col = GetComponent<CircleCollider2D>();
                if (col != null)
                    col.isTrigger = true;

                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.simulated = true;
                    rb.linearVelocity = Vector2.zero;
                }
            }
        }
        else 
        {
            InGame_State_Manager.State_Change(InGame_State.DEATH);
        }

        On_Died?.Invoke();
    }
    #endregion

    private void Recovery_Per_Second(float value)
    {
        if (Current >= Max) return;

        float percent = value / 100;

        timer += Time.deltaTime;

        if (timer >= recovery_Per_Second_timer)
        {
            double amount = (Max * percent);
            Heal(amount);

            timer = 0f;
        }
    }

    private void Invoke_On_Health_Change()
    {
        On_Value_Changed?.Invoke(Current, Max);
    }

    public void Return_HP_Bar()
    {
        if (need_HP_UI) 
        {
            if (is_Boss)
            {
                if (uI_Boss_Slider != null)
                {
                    Base_Manager.Pool_Mng.pool_Dictionary[Pool_ID.Boss_HP_Bar].Return(uI_Boss_Slider.gameObject);
                    uI_Boss_Slider = null;
                }
            }
            else
            {
                if (slider != null)
                {
                    Base_Manager.Pool_Mng.pool_Dictionary[Pool_ID.Enemy_HP_Bar].Return(slider.gameObject);
                    slider = null;
                }
            }
        }
        
    }
    public void Refresh_Health_UI()
    {
        Invoke_On_Health_Change();
    }
}
