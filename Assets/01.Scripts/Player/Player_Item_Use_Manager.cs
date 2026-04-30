using UnityEngine;

public class Player_Item_Use_Manager : MonoBehaviour
{
    [Header("REF")]
    [SerializeField] 
    private Player_Input_Manager player_Input_Manager;

    private void Update()
    {
        Handle_Item_Input();
    }
    private void Handle_Item_Input()
    {
        for (int i = 0; i < 5; i++)
        {
            if (Is_Item_Input_Triggered(i))
            {
                UI_InGame_Item_Slots.Instance.Try_Use_Slot(i);
            }
        }
    }
    private bool Is_Item_Input_Triggered(int index)
    {
        switch (index)
        {
            case 0: return player_Input_Manager.Use_Item_1_Triggered;
            case 1: return player_Input_Manager.Use_Item_2_Triggered;
            case 2: return player_Input_Manager.Use_Item_3_Triggered;
            case 3: return player_Input_Manager.Use_Item_4_Triggered;
            case 4: return player_Input_Manager.Use_Item_5_Triggered;
        }

        return false;
    }
}
