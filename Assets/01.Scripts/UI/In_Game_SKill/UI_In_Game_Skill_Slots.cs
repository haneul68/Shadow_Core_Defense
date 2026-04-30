using System.Collections.Generic;
using UnityEngine;

public class UI_In_Game_Skill_Slots : MonoBehaviour
{
    [SerializeField] private Transform content;

    private readonly List<In_Game_Skill_Slot> slots = new List<In_Game_Skill_Slot>();

    private void OnEnable()
    {
        Draw_Slots();

        Base_Manager.Character_Mng.On_Current_Character_Changed += On_Current_Character_Changed;
    }

    private void OnDisable()
    {
        Base_Manager.Character_Mng.On_Current_Character_Changed -= On_Current_Character_Changed;
        Clear();
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] != null)
                slots[i].Tick(dt);
        }
    }

    public void Draw_Slots()
    {
        Clear();
        Debug.Log("Draw_Slots");
        Character player = Base_Manager.Character_Mng.current_Character;
        if (player == null) 
        {
            Debug.Log("player == null)");
            return;
        }
            

        Player_Skill_Manager skill_Manager = player.GetComponent<Player_Skill_Manager>();
        if (skill_Manager == null)
        {
            Debug.Log("skill_Manager == null");
            return;
        }

        IReadOnlyList<Player_Skill_Base> skills = skill_Manager.UI_Sorted_Skills;
        if (skills == null || skills.Count == 0)
        {
            Debug.Log("skills == null || skills.Count == 0");
            return;
        }

        for (int i = 0; i < skills.Count; i++)
        {
            Debug.Log(skills.Count);
            Player_Skill_Base skill = skills[i];
            if (skill == null) continue;

            Base_Manager.Pool_Mng.Pooling_OBJ(Pool_ID.In_Game_Skill_Slot).Get(obj =>
            {
                Debug.Log("In_Game_Skill_Slot");
                obj.transform.SetParent(content, false);
                obj.SetActive(true);

                In_Game_Skill_Slot slot = obj.GetComponent<In_Game_Skill_Slot>();
                slot.Init(skill, skill_Manager);

                slots.Add(slot);
            });
        }
    }
    private void On_Current_Character_Changed(Character player)
    {
        Draw_Slots();
    }
    public void Clear()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] == null) continue;

            Base_Manager.Pool_Mng.pool_Dictionary[Pool_ID.In_Game_Skill_Slot]
                .Return(slots[i].gameObject);
        }

        slots.Clear();
    }
}