using System.Collections.Generic;
using UnityEngine;
public class Tracked_Pool_Object
{
    public GameObject obj;
    public Pool_ID pool_ID;
}
public class Player_Skill_Manager : MonoBehaviour
{
    [SerializeField] private Player_Input_Manager input_Manager;
    [SerializeField] private Health_Manager health_Manager;

    [Header("Skill List")]
    [SerializeField] private List<Player_Skill_Base> skill_List = new List<Player_Skill_Base>();

    private readonly List<Player_Skill_Base> active_Skills = new List<Player_Skill_Base>();
    private readonly List<Player_Skill_Base> passive_Skills = new List<Player_Skill_Base>();
    private readonly List<Player_Skill_Base> ui_Sorted_Skills = new List<Player_Skill_Base>();

    private readonly List<Tracked_Pool_Object> tracked_Objects = new List<Tracked_Pool_Object>();

    public IReadOnlyList<Player_Skill_Base> Skill_List => skill_List;
    public IReadOnlyList<Player_Skill_Base> Active_Skills => active_Skills;
    public IReadOnlyList<Player_Skill_Base> Passive_Skills => passive_Skills;
    public IReadOnlyList<Player_Skill_Base> UI_Sorted_Skills => ui_Sorted_Skills;

    private void Awake()
    {
        if (input_Manager == null) input_Manager = GetComponent<Player_Input_Manager>();
        if (health_Manager == null) health_Manager = GetComponent<Health_Manager>();
    }

    private void OnEnable()
    {
        if (health_Manager != null)
            health_Manager.On_Died += Force_Stop_All_Skills;
    }

    private void OnDisable()
    {
        if (health_Manager != null)
            health_Manager.On_Died -= Force_Stop_All_Skills;

        Force_Stop_All_Skills();
    }

    public void Init_Skills()
    {
        Force_Stop_All_Skills();

        active_Skills.Clear();
        passive_Skills.Clear();
        ui_Sorted_Skills.Clear();

        Classify_Skills();
        Apply_Passives();
        Build_UI_Skill_Order();
    }

    private void Update()
    {
        if (input_Manager == null) return;
        if (health_Manager != null && health_Manager.is_Dead) return;

        if (input_Manager.Skill_Q_Triggered)
        {
            Debug.Log("Q ´­¸²");
            Use_Active_Skill(0);
        }

        if (input_Manager.Skill_W_Triggered)
            Use_Active_Skill(1);

        if (input_Manager.Skill_E_Triggered)
            Use_Active_Skill(2);
    }

    private void Classify_Skills()
    {
        for (int i = 0; i < skill_List.Count; i++)
        {
            Player_Skill_Base skill = skill_List[i];
            if (skill == null) continue;

            if (skill.Is_Passive)
                passive_Skills.Add(skill);
            else
                active_Skills.Add(skill);
        }
    }

    private void Apply_Passives()
    {
        for (int i = 0; i < passive_Skills.Count; i++)
        {
            passive_Skills[i].Apply_Passive(this);
        }
    }

    private void Build_UI_Skill_Order()
    {
        for (int i = 0; i < active_Skills.Count; i++)
        {
            ui_Sorted_Skills.Add(active_Skills[i]);
        }

        for (int i = 0; i < passive_Skills.Count; i++)
        {
            ui_Sorted_Skills.Add(passive_Skills[i]);
        }
    }

    private void Use_Active_Skill(int index)
    {
        if (index < 0 || index >= active_Skills.Count) return;
        active_Skills[index].Try_Use(this);
    }

    public string Get_Skill_Key_Text(Player_Skill_Base skill)
    {
        if (skill == null) return string.Empty;

        if (skill.Is_Passive)
            return "P";

        int index = active_Skills.IndexOf(skill);

        return index switch
        {
            0 => "Q",
            1 => "W",
            2 => "E",
            _ => string.Empty
        };
    }

    public void Register_Spawned_Object(GameObject obj, Pool_ID pool_ID)
    {
        if (obj == null) return;

        tracked_Objects.Add(new Tracked_Pool_Object
        {
            obj = obj,
            pool_ID = pool_ID
        });
    }

    public void Force_Stop_All_Skills()
    {
        for (int i = 0; i < skill_List.Count; i++)
        {
            if (skill_List[i] == null) continue;

            skill_List[i].Remove_Passive(this);
            skill_List[i].Force_Stop(this);
        }

        for (int i = tracked_Objects.Count - 1; i >= 0; i--)
        {
            if (tracked_Objects[i].obj != null)
            {
                Base_Manager.Pool_Mng.pool_Dictionary[tracked_Objects[i].pool_ID].Return(tracked_Objects[i].obj);
            }
        }

        tracked_Objects.Clear();
    }
}