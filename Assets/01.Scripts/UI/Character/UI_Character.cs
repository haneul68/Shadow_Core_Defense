using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Character : UI_Base
{
    [SerializeField] private Transform content;
    [SerializeField] private Transform panel_Content;

    [SerializeField] private TextMeshProUGUI gold_Text;
    [SerializeField] private TextMeshProUGUI diamond_Text;

    private List<GameObject> garbage_Slots = new List<GameObject>();

    [Header("Skill UI")]
    [SerializeField] private UI_Player_Skill_List skill_List_UI;

    private void OnEnable()
    {
        Draw_Character_Slots();
        Refresh_Currency();

        if (Base_Manager.Character_Mng != null)
        {
            Base_Manager.Character_Mng.On_Character_Changed += On_Character_Changed;
            Base_Manager.Character_Mng.On_Character_Equipped += On_Character_Equipped;
        }

        Base_Manager.Data_Mng.On_Gold_Changed += On_Gold_Changed;
        Base_Manager.Data_Mng.On_Diamond_Changed += On_Diamond_Changed;
    }

    private void OnDisable()
    {
        if (Base_Manager.Character_Mng != null)
        {
            Base_Manager.Character_Mng.On_Character_Changed -= On_Character_Changed;
            Base_Manager.Character_Mng.On_Character_Equipped -= On_Character_Equipped;
        }

        if (Base_Manager.Data_Mng != null)
        {
            Base_Manager.Data_Mng.On_Gold_Changed -= On_Gold_Changed;
            Base_Manager.Data_Mng.On_Diamond_Changed -= On_Diamond_Changed;
        }

        Close_Action_Panel();
        Close_Purchase_Panel();
        Return_Slots();
    }

    private void Draw_Character_Slots()
    {
        Return_Slots();

        foreach (var pair in Base_Manager.Data_Mng.d_Character_Data)
        {
            Character_Scriptable data = pair.Value;

            Base_Manager.Pool_Mng.Pooling_OBJ(Pool_ID.Character_Slot).Get(obj =>
            {
                Character_Slot slot = obj.GetComponent<Character_Slot>();
                slot.transform.SetParent(content, false);
                slot.gameObject.SetActive(true);
                slot.Init(data, panel_Content);

                garbage_Slots.Add(slot.gameObject);
            });
        }
    }

    private void Return_Slots()
    {
        if (garbage_Slots.Count <= 0) return;

        for (int i = 0; i < garbage_Slots.Count; i++)
        {
            Base_Manager.Pool_Mng.pool_Dictionary[Pool_ID.Character_Slot].Return(garbage_Slots[i]);
        }

        garbage_Slots.Clear();
    }

    private void Close_Action_Panel()
    {
        if (Base_Manager.Character_Mng.Character_Action_Panel_Holder.Count > 0)
        {
            GameObject obj = Base_Manager.Character_Mng.Character_Action_Panel_Holder.Peek();
            obj.GetComponent<Character_Action_Panel>().Close_Panel();
        }
    }

    private void Close_Purchase_Panel()
    {
        if (Base_Manager.Character_Mng.Character_Purchase_Panel_Holder.Count > 0)
        {
            GameObject obj = Base_Manager.Character_Mng.Character_Purchase_Panel_Holder.Peek();
            obj.GetComponent<Character_Purchase_Panel>().Close_Panel();
        }
    }

    private void On_Character_Changed(string ch_Name)
    {
        Draw_Character_Slots();
    }

    private void On_Character_Equipped(string ch_Name)
    {
        Draw_Character_Slots();
    }

    private void Refresh_Currency()
    {
        if (gold_Text != null)
            gold_Text.text = Base_Manager.Data_Mng.Gold.ToString();

        if (diamond_Text != null)
            diamond_Text.text = Base_Manager.Data_Mng.Diamond.ToString();
    }

    private void On_Gold_Changed(int amount)
    {
        if (gold_Text != null)
            gold_Text.text = amount.ToString();
    }

    private void On_Diamond_Changed(int amount)
    {
        if (diamond_Text != null)
            diamond_Text.text = amount.ToString();
    }

    public void Reset_UI()
    {
        Close_Action_Panel();
        Close_Purchase_Panel();
        Return_Slots();
    }

    public override void Close_UI()
    {
        if (skill_List_UI != null)
            skill_List_UI.Clear();

        Reset_UI();
        base.Close_UI();
    }
}