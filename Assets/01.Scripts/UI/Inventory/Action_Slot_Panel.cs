using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Action_Slot_Panel : MonoBehaviour, IPointerExitHandler
{
    [SerializeField]
    private Button equip_Button, Exit_Button;
    [SerializeField] 
    private TextMeshProUGUI equip_Button_Text;

    private Item_Scriptable current_Item;

    private Inventory_Item_Slot current_Slot;

    public void Init(Item_Scriptable item, Inventory_Item_Slot slot)
    {
        current_Item = item;
        current_Slot = slot;

        Exit_Button.onClick.RemoveAllListeners();
        Exit_Button.onClick.AddListener(() =>
        {
            Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Button_Click);
            Close_Panel();
        });

        equip_Button.onClick.RemoveAllListeners();

        bool is_Equipped = Base_Manager.Inventory_Mng.inventory_Logic.Is_Equipped(current_Item);

        if (is_Equipped)
        {
            if (equip_Button_Text != null)
                equip_Button_Text.text = "장착 해제";

            equip_Button.onClick.AddListener(() =>
            {
                Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Button_Click);
                Un_Equip_Item();
            });
        }
        else
        {
            if (equip_Button_Text != null)
                equip_Button_Text.text = "장착";

            equip_Button.onClick.AddListener(() =>
            {
                Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Button_Click);
                Equip_Item();
            });
        }
    }

    private void Use_in_Inventory_Item()
    {
        var inventory = Base_Manager.Inventory_Mng.inventory_Data;
        var slot = inventory.Inventory_Slots[current_Item.item_Type][current_Slot.Slot_Index];

        bool success = inventory.Use_Slot(current_Item.item_Type, current_Slot.Slot_Index, 1);
        if (!success)
        {
            Debug.Log("아이템 사용 실패");
            return;
        }

        Character target = Base_Manager.Character_Mng.current_Character.GetComponent<Character>();

        current_Item.Use(target);
   

        if (slot.quantity > 0)
            current_Slot.Init(slot.item, slot.quantity, null, current_Slot.Slot_Index, Item_Slot_Type.Inventory);
        else
            current_Slot.Init(null, 0, null, -1, Item_Slot_Type.Inventory);

        Close_Panel();
    }

    public void Close_Panel()
    {
        Base_Manager.Pool_Mng.pool_Dictionary[Pool_ID.Inv_Item_Action_Panel].Return(this.gameObject);
        Base_Manager.Inventory_Mng.Acrtion_Panal_Holder.Clear();
        current_Item = null;
    }

    private void Equip_Item()
    {
        if (current_Item == null || current_Slot == null)
        {
            Debug.Log("장착 실패 : 데이터 없음");
            return;
        }

        bool success = Base_Manager.Inventory_Mng.inventory_Logic.Try_Equip_Item(current_Item);

        if (!success)
        {
            Debug.Log("장착 실패");
            return;
        }

        Lobby_Canvas.Instance.Get_Text_Pop_Up($"{current_Item.item_Name} 장착하였습니다", Color.white);
        Close_Panel();
    }
    private void Un_Equip_Item()
    {
        if (current_Item == null || current_Slot == null)
        {
            Debug.Log("장착 해제 실패 : 데이터 없음");
            return;
        }

        bool success = Base_Manager.Inventory_Mng.inventory_Logic.Try_UnEquip_Item(current_Item);

        if (!success)
        {
            Debug.Log("장착 해제 실패");
            return;
        }

        Lobby_Canvas.Instance.Get_Text_Pop_Up($"{current_Item.item_Name} 장착 해제하였습니다", Color.white);
        Close_Panel();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Close_Panel();
    }
}
