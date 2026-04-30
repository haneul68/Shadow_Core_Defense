
[System.Serializable]
public class Skill_Data
{
    public Boss_Skill_Type boss_Skill_Type;
}

public abstract class Skill_Definition 
{
    public string skill_Name;
    public float cast_Time;

    public bool is_Attack_Skill;

    public abstract Skill_Runtime Create_Runtime();
}
