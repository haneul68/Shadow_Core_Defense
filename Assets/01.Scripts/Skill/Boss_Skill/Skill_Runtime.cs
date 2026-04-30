using System;
using System.Collections;
using UnityEngine;

public abstract class Skill_Runtime
{
    protected Skill_Definition definition;

    public bool Is_Attack_Skill => definition.is_Attack_Skill;

    public event Action<bool> On_Skill_Execute;

    public Skill_Runtime(Skill_Definition definition) 
    {
        this.definition = definition;
    }
    protected void Invoke_Skill_Execute()
    {
        On_Skill_Execute?.Invoke(Is_Attack_Skill);
    }

    public virtual IEnumerator Execute_Coroutine(GameObject owner)
    {
        if (definition.cast_Time > 0)
        {
            yield return new WaitForSeconds(definition.cast_Time);
        }

        Invoke_Skill_Execute();

        On_Execute(owner);
    }

    protected abstract void On_Execute(GameObject owner);
}
