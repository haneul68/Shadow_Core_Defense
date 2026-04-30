public class InGame_State_Manager
{
    public static InGame_State m_state;

    public static void State_Change(InGame_State state)
    {
        m_state = state;

        switch (state)
        {
            case InGame_State.READY:
                Delegate_Holder.Ready();
                break;

            case InGame_State.BATTLE:
                Delegate_Holder.Battle();
                break;

            case InGame_State.BOSS_READY:
                Delegate_Holder.Boss_Ready();
                break;

            case InGame_State.BOSS_BATTLE:
                Delegate_Holder.Boss_Battle();
                break;

            case InGame_State.ABILITY_SELECT:
                Delegate_Holder.Ability_Select();
                break;

            case InGame_State.VICTORY:
                Delegate_Holder.Victory();
                break;

            case InGame_State.DEATH:
                Delegate_Holder.Death();
                break;
        }
    }
}