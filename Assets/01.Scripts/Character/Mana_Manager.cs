using System;
using UnityEngine;

public interface IMana 
{
    void Recover_Mana(float amount);
}

public class Mana_Manager : MonoBehaviour, IStat_Provider, IMana
{
    [SerializeField]
    private Character character;

    public double Current => character.Current_MP;
    public double Max => character.Final_Max_MP;

    public event Action<double, double> On_Value_Changed;

    [SerializeField]
    private float recovery_Per_Second_timer = 1;

    private float timer;

    [SerializeField]
    private float recovery_Per_Second_Percent = 0.0f;


    private void Start()
    {
        Invoke_On_MP_Change(); 
    }

    private void Update()
    {
        Recovery_Per_Second(recovery_Per_Second_Percent);
    }

    public bool Use(float amount)
    {
        if (Current < amount) 
        {
            return false; 
        }

        character.Current_MP -= amount;
        Invoke_On_MP_Change();
        return true;
    }

    public void Recover_Mana(float amount) 
    {
        Recover(amount);
    }

    public void Recover(float amount)
    {
        if (Current >= Max) return;

        float value = character.Current_MP;

        character.Current_MP = Mathf.Min(character.Current_MP + amount, (float)Max);

        if (value != character.Current_MP)
            Invoke_On_MP_Change();
    }
    private void Recovery_Per_Second(float value)
    {
        if (Current >= Max) return;

        float percent = value / 100;

        timer += Time.deltaTime;

        if (timer >= recovery_Per_Second_timer)
        {
            float amount = (float)(Max * percent);
            Recover(amount);

            timer = 0f;
        }
    }
    public void Invoke_On_MP_Change()
    {
        On_Value_Changed?.Invoke(Current, Max);
    }
}
