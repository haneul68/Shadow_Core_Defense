using System;
using UnityEngine;

public class Stamina_Manager : MonoBehaviour, IStat_Provider
{
    [SerializeField]
    private Character character;
    public double Current => character.Current_Stamina;
    public double Max => character.Final_Max_Stamina;

    [SerializeField]
    private float recovery_Per_Second_timer = 1;

    private float timer;

    [SerializeField]
    private float recovery_Per_Second_Percent = 0.0f;

    public event Action<double, double> On_Value_Changed;


    private void Start()
    {
        Invoke_On_Stamina_Change();
    }

    private void Update()
    {
        Recovery_Per_Second(recovery_Per_Second_Percent);
    }

    public bool Use(float amount)
    {
        if (Current < amount) return false;
        character.Current_Stamina -= amount;
        Invoke_On_Stamina_Change();
        return true;
    }

    public void Recover(float amount)
    {
        if (Current >= Max) return;

        float value = character.Current_Stamina;

        character.Current_Stamina = Mathf.Min(character.Current_Stamina + amount, (float)Max);

        if (value != character.Current_Stamina)
            Invoke_On_Stamina_Change();
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

    public void Invoke_On_Stamina_Change() 
    {
        On_Value_Changed?.Invoke(Current, Max);
    }
}
