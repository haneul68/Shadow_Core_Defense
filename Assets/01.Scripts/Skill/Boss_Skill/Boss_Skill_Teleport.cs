using System.Collections;
using UnityEngine;

public class Boss_Skill_Teleport : Skill_Definition
{
    public override Skill_Runtime Create_Runtime()
    {
        return new Boss_Skill_Teleport_Runtime(this);
    }
}
public class Boss_Skill_Teleport_Runtime : Skill_Runtime
{
    private Boss boss;

    public Boss_Skill_Teleport_Runtime(Skill_Definition definition) : base(definition) { }

    public override IEnumerator Execute_Coroutine(GameObject owner)
    {
        if (boss == null)
        {
            boss = owner.GetComponent<Boss>();
        }

        if (boss == null)
        {
            yield break;
        }

        yield return base.Execute_Coroutine(owner);
    }

    protected override void On_Execute(GameObject owner)
    {
        Teleport(boss);
    }

    private void Teleport(Boss boss)
    {
        int current_Index = boss.Current_Index;

        int new_Index = Get_Random_Index(current_Index);

        boss.Move_To_Point(new_Index);
    }

    private int Get_Random_Index(int current_Index)
    {
        int[] option = current_Index switch
        {
            0 => new[] { 1, 2, 3 },
            1 => new[] { 0, 2, 3 },
            2 => new[] { 0, 1, 3 },
            3 => new[] { 0, 1, 2 },
            _ => new[] { 0, 1, 2, 3 }
        };

        return option[Random.Range(0, option.Length)];
    }
}
