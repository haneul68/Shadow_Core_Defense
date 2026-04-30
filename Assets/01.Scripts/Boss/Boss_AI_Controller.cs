using System.Collections;
using UnityEngine;

public class Boss_AI_Controller : MonoBehaviour
{
    [Header("REF")]
    [SerializeField]
    private Enemy_Base enemy;
    [SerializeField]
    private Boss_Combat_Ani_Controller boss_Combat_Ani_Controller;
    [SerializeField]
    private Health_Manager health_manager;
    [SerializeField]
    private Boss_Skill_Factory skill_factory;

    [Space(20)]
    [Header("REF")]
    [SerializeField]
    private Boss_Phase[] phases;

    private int current_Phase_Index;
    private int current_Pattern_Index;

    private Coroutine pattern_Coroutine;

    private void Awake()
    {
        if(enemy == null)
            enemy = GetComponent<Enemy_Base>();

        if (boss_Combat_Ani_Controller == null)
            boss_Combat_Ani_Controller = GetComponent<Boss_Combat_Ani_Controller>();

        if (health_manager == null)
        {
            health_manager = GetComponent<Health_Manager>();
        }
        if (skill_factory == null)
        {
            skill_factory = GetComponent<Boss_Skill_Factory>();
        }

    }

    private void OnEnable()
    {
        health_manager.On_Died += OnDeath;

        if (pattern_Coroutine != null) 
        {
            StopCoroutine(pattern_Coroutine);
            pattern_Coroutine = null;
        }

        StartCoroutine(Start_Boss_AI_Delay());
    }

    private void OnDisable()
    {
        health_manager.On_Died -= OnDeath;

    }
    private void Update()
    {
        if (health_manager.is_Dead == true)
        {
            return;
        }
            
        Check_Phase_Change();
    }

    private IEnumerator Start_Boss_AI_Delay() 
    {
        yield return null;
        Start_Phase(0);
    }

    private void Check_Phase_Change()
    {
        float hp_Percent = (float)(enemy.Current_HP / enemy.Final_Max_HP);

        for (int i = phases.Length - 1; i >= 0; i--)
        {
            if (hp_Percent <= phases[i].enter_HP_Percent)
            {
                if (current_Phase_Index != i)
                {
                    Start_Phase(i);
                }
                return;
            }
        }
    }

    private void Start_Phase(int index)
    {
        current_Phase_Index = index;
        current_Pattern_Index = 0;

        Debug.Log($"{current_Phase_Index + 1} 페이즈 시작");

        if (pattern_Coroutine != null)
            StopCoroutine(pattern_Coroutine);

        pattern_Coroutine = StartCoroutine(Pattern_Loop());
    }

    private IEnumerator Pattern_Loop()
    {
        Boss_Phase phase = phases[current_Phase_Index];

        while (true)
        {
            Boss_Pattern pattern = phase.patterns[current_Pattern_Index];

            Skill_Runtime skill = skill_factory.Create_Boss_Skill(pattern.skill_Data.boss_Skill_Type);

            if (boss_Combat_Ani_Controller) 
            {
                skill.On_Skill_Execute += boss_Combat_Ani_Controller.Handle_Attack;
            }

            yield return StartCoroutine(skill.Execute_Coroutine(gameObject));

            if (boss_Combat_Ani_Controller)
            {
                skill.On_Skill_Execute -= boss_Combat_Ani_Controller.Handle_Attack;
            }

            Debug.Log("Idle_Start");
            yield return new WaitForSeconds(pattern.idle_Time);
            Debug.Log("Idle_End");

            current_Pattern_Index++;

            if (current_Pattern_Index >= phase.patterns.Length)
                current_Pattern_Index = 0;
        }
    }

    private void OnDeath()
    {
        if (health_manager == null) return;
        Debug.Log("[Boss] : OnDeath");

        if (pattern_Coroutine != null)
        {
            StopCoroutine(pattern_Coroutine);
            pattern_Coroutine = null;
        }

        Enemy_Spawn_Manager.Instance.Return_Boss_Spawned_Enemies();

        Base_Manager.Instance.Return_Pool_Obj_Delay(enemy.Enemy_Pool_ID, this.gameObject, 1f);
    }
}
