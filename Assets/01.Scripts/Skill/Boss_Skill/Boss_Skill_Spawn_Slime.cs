using UnityEngine;

public class Boss_Skill_Spawn_Slime : Skill_Definition
{
    public int magma_Count;
    public int green_Count;
    public int blue_Count;

    public override Skill_Runtime Create_Runtime()
    {
        return new Boss_Skill_Spawn_Slime_Runtime(this);
    }
}

public class Boss_Skill_Spawn_Slime_Runtime : Skill_Runtime
{
    private Boss_Skill_Spawn_Slime skill_Data;

    public Boss_Skill_Spawn_Slime_Runtime(Skill_Definition definition) : base(definition)
    {
        skill_Data = definition as Boss_Skill_Spawn_Slime;
    }

    protected override void On_Execute(GameObject owner)
    {
        Round_Data data = new Round_Data
        {
            magma_Count = skill_Data.magma_Count,
            green_Count = skill_Data.green_Count,
            blue_Count = skill_Data.blue_Count,
            boss_ID = Pool_ID.None
        };

        Enemy_Spawn_Manager.Instance.Start_Boss_Spawn_Skill(data);

        Debug.Log("Boss_Skill_Spawn_Slime ½ÃÀü");
    }
}