using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Rune_Action_Panel : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] 
    private TextMeshProUGUI rune_Name_Text;
    [SerializeField] 
    private TextMeshProUGUI rarity_Text;
    [SerializeField] 
    private TextMeshProUGUI level_Text;
    [SerializeField] 
    private TextMeshProUGUI chance_Text;
    [SerializeField]
    private TextMeshProUGUI des_Text;

    [Space(20)]
    [Header("Image")]
    [SerializeField] 
    private Image rune_Image_01;
    [SerializeField] 
    private Image rune_Image_02;

    [Space(20)]
    [Header("Button")]
    [SerializeField] 
    private Button craft_Button;
    [SerializeField] 
    private TextMeshProUGUI craft_Button_Text;

    [SerializeField] 
    private Button equip_Button;
    [SerializeField] 
    private TextMeshProUGUI equip_Button_Text;

    [SerializeField] 
    private Button close_Button;

    [Space(20)]
    [Header("Material")]
    [SerializeField] 
    private Transform material_Content;

    private readonly List<GameObject> spawned_Material_Slot_List = new List<GameObject>();

    private Rune_Scriptable rune_Data;

    private void Awake()
    {
        if (craft_Button != null)
            craft_Button.onClick.AddListener(OnClick_Craft_Button);

        if (equip_Button != null)
            equip_Button.onClick.AddListener(OnClick_Equip_Button);

        if (close_Button != null)
            close_Button.onClick.AddListener(Close_Panel);
    }

    public void Init(Rune_Scriptable rune_Data)
    {
        this.rune_Data = rune_Data;
        Refresh();
    }

    public void Refresh()
    {
        if (rune_Data == null) return;
        if (Base_Manager.Rune_Mng == null) return;

        Rune_Holder rune_Holder = Base_Manager.Rune_Mng.Get_Rune_Holder(rune_Data.item_ID);
        if (rune_Holder == null) return;

        Sprite rune_Sprite = Utils.Get_Rune_Atlas(rune_Data.item_ID);

        if (rune_Name_Text != null)
            rune_Name_Text.text = rune_Data.item_Name;

        if (rarity_Text != null)
            rarity_Text.text = rune_Data.rarity.ToString();

        if (level_Text != null)
            level_Text.text = rune_Holder.is_Owned ? $"LV.{rune_Holder.level:00}" : "LV.00";

        if (chance_Text != null)
        {
            float chance_Value = Base_Manager.Rune_Mng.Get_Current_Chance(rune_Data.item_ID);

            if (rune_Holder.is_Owned && rune_Holder.level >= rune_Data.max_Level)
                chance_Text.text = "MAX";
            else
                chance_Text.text = $"{chance_Value:0}%";
        }

        if (des_Text != null) 
        {
            des_Text.text = rune_Data.Get_Description(rune_Holder.level);
        }

        if (rune_Image_01 != null)
            rune_Image_01.sprite = rune_Sprite;

        if (rune_Image_02 != null)
            rune_Image_02.sprite = rune_Sprite;

        Refresh_Craft_Button(rune_Holder);
        Refresh_Equip_Button(rune_Holder);
        Refresh_Material_List();
    }

    private void Refresh_Craft_Button(Rune_Holder rune_Holder)
    {
        if (craft_Button == null || craft_Button_Text == null || rune_Holder == null)
            return;

        if (!rune_Holder.is_Owned)
        {
            craft_Button_Text.text = "제작";
            craft_Button.interactable = Base_Manager.Rune_Mng.Can_Craft_Or_Upgrade(rune_Data.item_ID);
            return;
        }

        if (rune_Holder.level >= rune_Data.max_Level)
        {
            craft_Button_Text.text = "최대";
            craft_Button.interactable = false;
            return;
        }

        craft_Button_Text.text = "강화";
        craft_Button.interactable = Base_Manager.Rune_Mng.Can_Craft_Or_Upgrade(rune_Data.item_ID);
    }

    private void Refresh_Equip_Button(Rune_Holder rune_Holder)
    {
        if (equip_Button == null || equip_Button_Text == null || rune_Holder == null)
            return;

        if (!rune_Holder.is_Owned)
        {
            equip_Button_Text.text = "장착";
            equip_Button.interactable = false;
            return;
        }

        bool is_Equipped = Base_Manager.Rune_Mng.Is_Equipped(rune_Data.item_ID);

        equip_Button_Text.text = is_Equipped ? "장착 해제" : "장착";
        equip_Button.interactable = true;
    }

    private void Refresh_Material_List()
    {
        Clear_Material_List();

        if (material_Content == null) return;

        List<Rune_Material_Data> material_List = Base_Manager.Rune_Mng.Get_Current_Material_List(rune_Data.item_ID);

        if (material_List == null || material_List.Count == 0)
            return;

        for (int i = 0; i < material_List.Count; i++)
        {
            Rune_Material_Data material_Data = material_List[i];
            if (material_Data == null || material_Data.item == null) continue;

            int current_Count = 0;

            if (Base_Manager.Inventory_Mng != null && Base_Manager.Inventory_Mng.inventory_Logic != null)
            {
                current_Count = Base_Manager.Inventory_Mng.inventory_Logic.Get_Item_Count(material_Data.item.item_ID);
            }

            Base_Manager.Pool_Mng.Pooling_OBJ(Pool_ID.UI_Rune_Material_Slot).Get(obj =>
            {
                obj.transform.SetParent(material_Content, false);

                UI_Rune_Material_Slot material_Slot = obj.GetComponent<UI_Rune_Material_Slot>();
                if (material_Slot != null)
                {
                    material_Slot.Init(material_Data.item, current_Count, material_Data.amount);
                }

                spawned_Material_Slot_List.Add(obj);
            });
        }
    }

    private void Clear_Material_List()
    {
        for (int i = 0; i < spawned_Material_Slot_List.Count; i++)
        {
            if (spawned_Material_Slot_List[i] != null)
            {
                Base_Manager.Pool_Mng.pool_Dictionary[Pool_ID.UI_Rune_Material_Slot].Return(spawned_Material_Slot_List[i]);
            }
        }

        spawned_Material_Slot_List.Clear();
    }

    private void OnClick_Craft_Button()
    {
        if (rune_Data == null) return;

        Rune_Holder rune_Holder = Base_Manager.Rune_Mng.Get_Rune_Holder(rune_Data.item_ID);
        bool is_First_Craft = rune_Holder != null && !rune_Holder.is_Owned;

        bool is_Success;
        bool is_Executed = Base_Manager.Rune_Mng.Try_Craft_Or_Upgrade(rune_Data.item_ID, out is_Success);

        if (!is_Executed)
        {
            Lobby_Canvas.Instance.Get_Text_Pop_Up("재료가 부족합니다", Color.red);
            Refresh();
            UI_Rune.Instance.Refresh_All();
            return;
        }

        if (is_Success)
        {
            if (is_First_Craft)
                Lobby_Canvas.Instance.Get_Text_Pop_Up($"{rune_Data.item_Name} 제작 성공", Color.white);
            else
                Lobby_Canvas.Instance.Get_Text_Pop_Up($"{rune_Data.item_Name} 강화 성공", Color.white);

            Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Rune_Upgrade_Success);
        }
        else
        {
            if (is_First_Craft)
                Lobby_Canvas.Instance.Get_Text_Pop_Up($"{rune_Data.item_Name} 제작 실패", Color.red);
            else
                Lobby_Canvas.Instance.Get_Text_Pop_Up($"{rune_Data.item_Name} 강화 실패", Color.red);

            Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Rune_Upgrade_Fail);
        }

        Refresh();
        UI_Rune.Instance.Refresh_All();
    }

    private void OnClick_Equip_Button()
    {
        Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Button_Click);
        if (rune_Data == null) return;

        bool is_Equipped = Base_Manager.Rune_Mng.Is_Equipped(rune_Data.item_ID);

        if (is_Equipped)
        {
            Base_Manager.Rune_Mng.Try_UnEquip(rune_Data.item_ID);
            Lobby_Canvas.Instance.Get_Text_Pop_Up($"{rune_Data.item_Name}을/를 장착 해제하였습니다", Color.white);

        }
        else
        {
            if (Base_Manager.Rune_Mng.Try_Equip(rune_Data.item_ID) == true)
            {
                Lobby_Canvas.Instance.Get_Text_Pop_Up($"{rune_Data.item_Name}을/를 장착하였습니다", Color.white);
            }
            else 
            {
                Lobby_Canvas.Instance.Get_Text_Pop_Up($"장착할 수 있는 슬롯이 없습니다", Color.red);
            }
        }

        Refresh();
        UI_Rune.Instance.Refresh_All();
    }

    public void Close_Panel()
    {
        Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Button_Click);
        Clear_Material_List();
        UI_Rune.Instance.Clear_Current_Action_Panel(gameObject);
        Base_Manager.Pool_Mng.pool_Dictionary[Pool_ID.UI_Rune_Action_Panel].Return(gameObject);
    }

    private void OnDisable()
    {
        Clear_Material_List();
    }
}