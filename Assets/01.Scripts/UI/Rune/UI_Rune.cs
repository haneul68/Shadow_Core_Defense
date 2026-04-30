using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Rune : UI_Base
{
    public static UI_Rune Instance { get; private set; }

    [Header("Rune Inventory")]
    [SerializeField] 
    private Transform rune_Slot_Content;
    [SerializeField] 
    private Transform popup_Content;
    [SerializeField]
    private Pool_ID rune_Slot_Pool_Id;

    [Space(20)]
    [Header("Equipped Slot")]
    [SerializeField] 
    private UI_Rune_Equipped_Slot[] equipped_Slot_List;

    [SerializeField]
    private TextMeshProUGUI gold_Text;
    [SerializeField]
    private TextMeshProUGUI dia_Text;

    private readonly List<GameObject> spawned_Rune_Slot_List = new List<GameObject>();

    private GameObject current_Action_Panel;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        if (Base_Manager.Rune_Mng != null)
        {
            Base_Manager.Rune_Mng.On_Rune_Changed += On_Rune_Changed;
            Base_Manager.Rune_Mng.On_Rune_Equipped_Changed += On_Rune_Equipped_Changed;
        }

        Set_Gold_Text();
        Set_Dia_Text();

        Draw_Rune_Slot_List();
        Draw_Equipped_Slot_List();
    }

    private void OnDisable()
    {
        if (Base_Manager.Rune_Mng != null)
        {
            Base_Manager.Rune_Mng.On_Rune_Changed -= On_Rune_Changed;
            Base_Manager.Rune_Mng.On_Rune_Equipped_Changed -= On_Rune_Equipped_Changed;
        }

        Clear_Rune_Slot_List();
        Close_Action_Panel();
    }

    private void On_Rune_Changed(string rune_Id)
    {
        Refresh_All();
    }

    private void On_Rune_Equipped_Changed()
    {
        Draw_Equipped_Slot_List();
    }

    public void Refresh_All()
    {
        Set_Gold_Text();
        Set_Dia_Text();
        Refresh_Rune_Slot_List();
        Draw_Equipped_Slot_List();
    }

    public void Set_Current_Action_Panel(GameObject panel_Obj)
    {
        current_Action_Panel = panel_Obj;
    }

    public void Clear_Current_Action_Panel(GameObject panel_Obj)
    {
        if (current_Action_Panel == panel_Obj)
            current_Action_Panel = null;
    }

    public void Close_Action_Panel()
    {
        if (current_Action_Panel == null) return;

        UI_Rune_Action_Panel panel = current_Action_Panel.GetComponent<UI_Rune_Action_Panel>();
        if (panel != null)
        {
            panel.Close_Panel();
        }
        else
        {
            Base_Manager.Pool_Mng.pool_Dictionary[Pool_ID.UI_Rune_Action_Panel].Return(current_Action_Panel);
            current_Action_Panel = null;
        }
    }

    private void Draw_Rune_Slot_List()
    {
        Clear_Rune_Slot_List();

        if (Base_Manager.Data_Mng == null) return;

        foreach (var pair in Base_Manager.Data_Mng.Rune_Data)
        {
            if (pair.Value == null) continue;

            Rune_Scriptable rune_Data = pair.Value as Rune_Scriptable;
            if (rune_Data == null) continue;

            Base_Manager.Pool_Mng.Pooling_OBJ(rune_Slot_Pool_Id).Get(obj =>
            {
                obj.transform.SetParent(rune_Slot_Content, false);

                UI_Rune_Slot rune_Slot = obj.GetComponent<UI_Rune_Slot>();
                if (rune_Slot != null)
                {
                    rune_Slot.Init(rune_Data, popup_Content);
                }

                spawned_Rune_Slot_List.Add(obj);
            });
        }
    }

    private void Refresh_Rune_Slot_List()
    {
        for (int i = 0; i < spawned_Rune_Slot_List.Count; i++)
        {
            if (spawned_Rune_Slot_List[i] == null) continue;

            UI_Rune_Slot rune_Slot = spawned_Rune_Slot_List[i].GetComponent<UI_Rune_Slot>();
            if (rune_Slot != null)
            {
                rune_Slot.Refresh();
            }
        }
    }

    private void Clear_Rune_Slot_List()
    {
        for (int i = 0; i < spawned_Rune_Slot_List.Count; i++)
        {
            if (spawned_Rune_Slot_List[i] != null)
            {
                Base_Manager.Pool_Mng.pool_Dictionary[rune_Slot_Pool_Id].Return(spawned_Rune_Slot_List[i]);
            }
        }

        spawned_Rune_Slot_List.Clear();
    }

    private void Draw_Equipped_Slot_List()
    {
        if (equipped_Slot_List == null) return;
        if (Base_Manager.Rune_Mng == null) return;

        for (int i = 0; i < equipped_Slot_List.Length; i++)
        {
            string rune_Id = string.Empty;

            if (i < Base_Manager.Rune_Mng.Equipped_Rune_Id_List.Count)
                rune_Id = Base_Manager.Rune_Mng.Equipped_Rune_Id_List[i];

            equipped_Slot_List[i].Init(rune_Id);
        }
    }
    private void Set_Gold_Text()
    {
        if (gold_Text == null) return;
        gold_Text.text = Base_Manager.Data_Mng.Gold.ToString();
    }

    private void Set_Dia_Text()
    {
        if (dia_Text == null) return;
        dia_Text.text = Base_Manager.Data_Mng.Diamond.ToString();
    }
}