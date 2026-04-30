using UnityEngine;

public class Anim_Event_Proxy : MonoBehaviour
{
    private Enemy_Melee_Attack enemy_Melee_Attack;

    bool has_Attackded = false;

    private void Awake()
    {
        if (enemy_Melee_Attack == null)
            enemy_Melee_Attack = GetComponentInParent<Enemy_Melee_Attack>();
    }

    public void AE_Attack() 
    {
        if (has_Attackded) return;

        Debug.LogError("슬라임 공격");
        has_Attackded = true;
        Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Enemy_Attack);
        enemy_Melee_Attack?.Spawn_Hit_Box();

    }
    public void AE_W_Attack()
    {
        if (has_Attackded) return;
        has_Attackded = true;
        Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Enemy_Attack);
        enemy_Melee_Attack?.Spawn_Hit_Box_With_Effect();
    }

    public void Reset_Attack() => has_Attackded = false;    
}
