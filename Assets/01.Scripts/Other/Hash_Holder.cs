using UnityEngine;

public class Animation_Parameter_Hash
{
    public static readonly int move_X_Hash = Animator.StringToHash("MoveX");
    public static readonly int move_Y_Hash = Animator.StringToHash("MoveY");
    public static readonly int is_Moving_Hash = Animator.StringToHash("Is_Moving");

    public static readonly int attack_Hash = Animator.StringToHash("Attack");
    public static readonly int Hit_Hash = Animator.StringToHash("Hit");
    public static readonly int Death_Hash = Animator.StringToHash("Death");

    public static readonly int Ablilty_Slot_Spawn = Animator.StringToHash("Spawn");
    public static readonly int Ablilty_Slot_Hover_Hash = Animator.StringToHash("Is_Hover");
    public static readonly int Ablilty_Slot_Select_Hash = Animator.StringToHash("Is_Select");

    public static readonly int Core_Is_Active_Hash = Animator.StringToHash("Is_Active");
}