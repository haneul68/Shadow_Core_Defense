using System;
using System.Collections;
using UnityEngine;

public class Round_Manager : MonoBehaviour
{
 
    public static Round_Manager Instance { get; private set; }

    [SerializeField]
    private int Boss_Round_Interval = 2;
    public int BOSS_ROUND_INTERVAL => Boss_Round_Interval;

    [SerializeField] private int max_Round = 2;
    [SerializeField] private float round_Time = 60f;

    private static int current_Round = 0;
    public static int Current_Round => current_Round;

    private Coroutine round_Coroutine;
    private Coroutine start_Delay_Coroutine;

    #region UI Events 
    public static event Action<int> On_Round_Text_Changed;
    public static event Action<float> On_Timer_Changed;
    public static event Action<int> On_Count_Down;
    #endregion

    #region End_Gmae
    private float game_Start_Time;
    public float Game_Play_Time => Mathf.Max(0f, Time.time - game_Start_Time);
    #endregion

    [SerializeField] GameObject UI_A;

    public bool Game_Ended { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Start_Game();
    }

    private void OnEnable()
    {
        Delegate_Holder.OnReady += Handle_Ready;
        Delegate_Holder.OnBattle += Handle_Battle;
        Delegate_Holder.OnBossReady += Handle_Boss_Ready;
        Delegate_Holder.OnBossBattle += Handle_Boss_Battle;
        Delegate_Holder.OnAbilitySelect += Handle_Ability_Select;
        Delegate_Holder.OnDeath += Handle_Game_Lose;
    }

    private void OnDisable()
    {
        Delegate_Holder.OnReady -= Handle_Ready;
        Delegate_Holder.OnBattle -= Handle_Battle;
        Delegate_Holder.OnBossReady -= Handle_Boss_Ready;
        Delegate_Holder.OnBossBattle -= Handle_Boss_Battle;
        Delegate_Holder.OnAbilitySelect -= Handle_Ability_Select;
    }

    #region 게임 시작
    private void Start_Game()
    {
        current_Round = 0;
        Game_Ended = false;
        game_Start_Time = Time.time;
        InGame_State_Manager.State_Change(InGame_State.READY);
    }
    #endregion

    #region READY
    private void Handle_Ready()
    {
        Debug.Log($"[READY] Round : {current_Round + 1}");

        On_Round_Text_Changed?.Invoke(current_Round);

        if (start_Delay_Coroutine != null)
            StopCoroutine(start_Delay_Coroutine);

        start_Delay_Coroutine = StartCoroutine(Ready_Coroutine());
    }

    private IEnumerator Ready_Coroutine()
    {
        for (int i = 3; i > 0; i--)
        {
            On_Count_Down?.Invoke(i);
            yield return new WaitForSeconds(1f);
        }

        if (Is_Boss_Round())
            InGame_State_Manager.State_Change(InGame_State.BOSS_READY);
        else
            InGame_State_Manager.State_Change(InGame_State.BATTLE);
    }
    #endregion

    #region 일반 전투
    private void Handle_Battle()
    {
        Debug.Log("[BATTLE]");

        Enemy_Spawn_Manager.Instance.Spawn_Round_Enemy(current_Round);

        Start_Round_Timer();
    }
    #endregion

    #region 보스 준비
    private void Handle_Boss_Ready()
    {
        Debug.Log("[BOSS READY]");
        StartCoroutine(Boss_Ready_Coroutine());
    }

    private IEnumerator Boss_Ready_Coroutine()
    {
        yield return new WaitForSeconds(3f);
        InGame_State_Manager.State_Change(InGame_State.BOSS_BATTLE);
    }
    #endregion

    #region 보스 전투
    private void Handle_Boss_Battle()
    {
        Debug.Log("[BOSS BATTLE]");

        Enemy_Spawn_Manager.Instance.Spawn_Boss(current_Round);

        Start_Round_Timer();
    }
    #endregion

    #region 타이머
    private void Start_Round_Timer()
    {
        if (round_Coroutine != null)
            StopCoroutine(round_Coroutine);

        round_Coroutine = StartCoroutine(Round_Timer());
    }

    private IEnumerator Round_Timer()
    {
        float timer = round_Time;

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            On_Timer_Changed?.Invoke(timer);

            yield return null;
        }

        // 시간 초과 → 죽음
        InGame_State_Manager.State_Change(InGame_State.DEATH);
    }
    #endregion

    #region 라운드 종료
    public void On_Enemy_All_Dead()
    {
        if (round_Coroutine != null)
            StopCoroutine(round_Coroutine);

        Debug.Log($"[CLEAR] Round : {current_Round + 1}");

        On_Normal_Clear();
    }

    public void On_Boss_Dead()
    {
        if (round_Coroutine != null)
            StopCoroutine(round_Coroutine);

        Debug.Log($"[CLEAR] Boss Round : {current_Round + 1}");

        if (Is_Boss_Round())
            On_Boss_Clear();
    }

    private void On_Normal_Clear()
    {
        current_Round++;
        InGame_State_Manager.State_Change(InGame_State.READY);
    }

    private void On_Boss_Clear()
    {
        current_Round++;

        if (Is_Last_Round())
            Game_Win();
        else
            InGame_State_Manager.State_Change(InGame_State.ABILITY_SELECT);
    }
    #endregion

    #region 어빌 선택
    private void Handle_Ability_Select()
    {
        StartCoroutine(Ability_Select_Coroutine());
    }

    private IEnumerator Ability_Select_Coroutine()
    {
        yield return new WaitForSeconds(2f);

        //Delegate_Holder.AbilitySelected();
        UI_A.SetActive(true);
        //InGame_State_Manager.State_Change(InGame_State.READY);
    }
    #endregion

    #region 유틸
    public bool Is_Boss_Round()
    {
        return (current_Round + 1) % Boss_Round_Interval == 0;
    }

    private bool Is_Last_Round()
    {
        return current_Round + 1 >= max_Round;
    }
    #endregion

    #region 게임 종료
    private void Game_Win() 
    {
        if (Game_Ended == true) return;

        Game_Ended = true;

        Stop_All();
        Debug.Log("Game_Win");
        InGame_State_Manager.State_Change(InGame_State.VICTORY);
    }
    private void Handle_Game_Lose()
    {
        if (Game_Ended == true) return;

        Game_Ended = true;

        Stop_All();
        Debug.Log("Game_Lose");
    }

    private void Stop_All() 
    {
        if (round_Coroutine != null) 
        {
            StopCoroutine(round_Coroutine);
        }
        if (start_Delay_Coroutine != null)
        {
            StopCoroutine(start_Delay_Coroutine);
        }

        Enemy_Spawn_Manager.Instance.Stop_All();
        Debug.Log("Stop_All");
    }
    #endregion
}