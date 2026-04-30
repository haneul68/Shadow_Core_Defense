using System.Collections.Generic;
using UnityEngine;

public class Ability_Manager : MonoBehaviour
{
    public static Ability_Manager Instance;

    [Header("Ability DB")]
    [SerializeField] private List<Ability> ability_DB = new List<Ability>();

    private void Awake()
    {
        Instance = this;
    }

    public Ability Get_Random_Ability()
    {
        if (ability_DB.Count == 0)
        {
            Debug.LogError("Ability DB 비어있음");
            return null;
        }

        return ability_DB[Random.Range(0, ability_DB.Count)];
    }

    public List<Ability> Get_Random_Abilities(int count)
    {
        List<Ability> result = new List<Ability>();

        List<Ability> copy = new List<Ability>(ability_DB);

        for (int i = 0; i < count; i++)
        {
            if (copy.Count == 0) break;

            int index = Random.Range(0, copy.Count);
            result.Add(copy[index]);
            copy.RemoveAt(index);
        }

        return result;
    }
}