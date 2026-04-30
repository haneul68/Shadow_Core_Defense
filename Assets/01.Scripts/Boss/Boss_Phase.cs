using UnityEngine;

[System.Serializable]
public class Boss_Phase
{
    [Range(0, 1)]
    public float enter_HP_Percent;

    public Boss_Pattern[] patterns;
}

