using UnityEngine;

public class Player_State_Bar_Manager : MonoBehaviour
{
    [SerializeField]
    private UI_Slider HP_Bar;
    [SerializeField]
    private UI_Slider MP_Bar;
    [SerializeField]
    private UI_Slider SP_Bar;

    public void Set_Player(GameObject player) 
    {
        if (player == null) return;

        HP_Bar.Set_Player_Stat_Manager(player);
        MP_Bar.Set_Player_Stat_Manager(player);
        SP_Bar.Set_Player_Stat_Manager(player);
    }
}
