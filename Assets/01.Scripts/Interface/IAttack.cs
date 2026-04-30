using System;
using UnityEngine;

public interface IAttack
{
    bool Is_Attack { get; set; }
    void Try_Attack();

    event Action OnAttack;
}
