using UnityEngine;

public class Player_directioon_Provider : MonoBehaviour, IDirection_Provider
{
    [SerializeField]
    private Player_Input_Manager player_Input_Manager;

    public Vector2 Last_Facing_Dir { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private void Awake()
    {
        if(player_Input_Manager == null)
            player_Input_Manager = GetComponent<Player_Input_Manager>();    
    }

    public Vector2 Get_Direction()
    {
        return player_Input_Manager.Move_Vector_2D;
    }
}
