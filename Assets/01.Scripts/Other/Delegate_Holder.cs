using System;

public static class Delegate_Holder
{
    public static event Action OnReady;
    public static event Action OnBattle;
    public static event Action OnBossReady;
    public static event Action OnBossBattle;
    public static event Action OnAbilitySelect;
    public static event Action OnVictory;
    public static event Action OnDeath;

    public static void Ready() => OnReady?.Invoke();
    public static void Battle() => OnBattle?.Invoke();
    public static void Boss_Ready() => OnBossReady?.Invoke();
    public static void Boss_Battle() => OnBossBattle?.Invoke();
    public static void Ability_Select() => OnAbilitySelect?.Invoke();
    public static void Victory() => OnVictory?.Invoke();
    public static void Death() => OnDeath?.Invoke();
}