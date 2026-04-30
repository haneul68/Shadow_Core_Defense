using System;
using System.Collections;
using UnityEngine;

public class Core_Manager : MonoBehaviour
{
    public static Core_Manager Instance;

    [Header("Core")]
    [SerializeField] private Core_Health core;

    public InGame_State CurrentState { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        Delegate_Holder.OnReady += Handle_Ready;
        Delegate_Holder.OnBattle += Handle_Battle;
        Delegate_Holder.OnBossReady += Handle_Boss_Ready;
        Delegate_Holder.OnBossBattle += Handle_Boss_Battle;
        Delegate_Holder.OnAbilitySelect += Handle_Ability_Select;
        Delegate_Holder.OnVictory += Handle_Victory;
        Delegate_Holder.OnDeath += Handle_Death;
    }

    private void OnDisable()
    {
        Delegate_Holder.OnReady -= Handle_Ready;
        Delegate_Holder.OnBattle -= Handle_Battle;
        Delegate_Holder.OnBossReady -= Handle_Boss_Ready;
        Delegate_Holder.OnBossBattle -= Handle_Boss_Battle;
        Delegate_Holder.OnAbilitySelect -= Handle_Ability_Select;
        Delegate_Holder.OnVictory -= Handle_Victory;
        Delegate_Holder.OnDeath -= Handle_Death;
    }

    Coroutine Core_Reset_Coroutine;
    private void Handle_Ready()
    {
        CurrentState = InGame_State.READY;

        if (Core_Reset_Coroutine != null) 
        {
            StopCoroutine(Core_Reset_Coroutine);
            Core_Reset_Coroutine = null;    
        }

        Core_Reset_Coroutine = StartCoroutine(Heal_Coroutine());

        core.Set_Active_State(true);
    }
    private IEnumerator Heal_Coroutine()
    {
        for (int i = 0; i < core.Max_Gauge; i++)
        {
            core.Heal(1);
            yield return new WaitForSeconds(0.4f);
        }
    }

    private void Handle_Battle()
    {
        CurrentState = InGame_State.BATTLE;
    }

    private void Handle_Boss_Ready()
    {
        CurrentState = InGame_State.BOSS_READY;
        core.Set_Active_State(false);
    }

    private void Handle_Boss_Battle()
    {
        CurrentState = InGame_State.BOSS_BATTLE;
    }

    private void Handle_Ability_Select()
    {
        CurrentState = InGame_State.ABILITY_SELECT;
    }

    private void Handle_Victory()
    {
        CurrentState = InGame_State.VICTORY;
        core.Set_Active_State(false);
    }

    private void Handle_Death()
    {
        CurrentState = InGame_State.DEATH;
        core.Set_Active_State(false);
    }
}