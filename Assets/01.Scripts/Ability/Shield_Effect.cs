using UnityEngine;

[System.Serializable]
public class Shield_Effect : IAbility_Effect
{
    public GameObject shield_Prefab;
    public float duration = 5f;

    public void Apply(GameObject target)
    {
        GameObject shield = GameObject.Instantiate(shield_Prefab, target.transform);
        shield.transform.localPosition = Vector3.zero;

        GameObject.Destroy(shield, duration);
    }

}