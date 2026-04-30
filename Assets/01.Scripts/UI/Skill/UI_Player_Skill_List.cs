using System.Collections.Generic;
using UnityEngine;

public class UI_Player_Skill_List : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private Transform popup_Content;

    private readonly List<GameObject> garbage_Slots = new List<GameObject>();

    private void OnEnable()
    {
        Refresh_Equipped_Character_Skills();

        if (Base_Manager.Character_Mng != null)
        {
            Base_Manager.Character_Mng.On_Character_Changed += On_Character_Changed;
            Base_Manager.Character_Mng.On_Character_Equipped += On_Character_Equipped;
        }
    }

    private void OnDisable()
    {
        if (Base_Manager.Character_Mng != null)
        {
            Base_Manager.Character_Mng.On_Character_Changed -= On_Character_Changed;
            Base_Manager.Character_Mng.On_Character_Equipped -= On_Character_Equipped;
        }
    }

    private void On_Character_Changed(string ch_Name)
    {
        Refresh_Equipped_Character_Skills();
    }

    private void On_Character_Equipped(string ch_Name)
    {
        Refresh_Equipped_Character_Skills();
    }

    public void Refresh_Equipped_Character_Skills()
    {
        Clear();

        string equippedName = Base_Manager.Character_Mng.Equipped_Character_Name;

        if (string.IsNullOrEmpty(equippedName))
            return;

        if (!Base_Manager.Data_Mng.p_Character_Holder.ContainsKey(equippedName))
            return;

        Character_Holder ch_Holder = Base_Manager.Data_Mng.p_Character_Holder[equippedName];
        if (ch_Holder == null || ch_Holder.Data == null)
            return;

        Character_Scriptable characterData = ch_Holder.Data;

        if (characterData.Character_Prefab == null)
            return;

        Player_Skill_Manager skillManager = characterData.Character_Prefab.GetComponent<Player_Skill_Manager>();
        if (skillManager == null)
            return;

        IReadOnlyList<Player_Skill_Base> skills = skillManager.Skill_List;
        if (skills == null || skills.Count == 0)
            return;

        for (int i = 0; i < skills.Count; i++)
        {
            Player_Skill_Base skill = skills[i];
            if (skill == null) continue;

            Base_Manager.Pool_Mng.Pooling_OBJ(Pool_ID.Player_Skill_Slot).Get(obj =>
            {
                obj.transform.SetParent(content, false);
                obj.SetActive(true);

                UI_Player_Skill_Slot slot = obj.GetComponent<UI_Player_Skill_Slot>();
                slot.Init(skill, popup_Content);

                garbage_Slots.Add(obj);
            });
        }
    }

    public void Clear()
    {
        for (int i = 0; i < garbage_Slots.Count; i++)
        {
            Base_Manager.Pool_Mng.pool_Dictionary[Pool_ID.Player_Skill_Slot].Return(garbage_Slots[i]);
        }

        garbage_Slots.Clear();
    }
}