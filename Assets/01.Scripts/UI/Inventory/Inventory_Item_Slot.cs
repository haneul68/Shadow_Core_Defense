using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Inventory_Item_Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public int Slot_Index { get; private set; }

    [SerializeField]
    private Image item_Image;
    [SerializeField]
    private TextMeshProUGUI quantity_Text;
    [SerializeField]
    private GameObject Equipped_Obj;

    Item_Scriptable data;
    RectTransform slot_Rect;

    private Transform content;

    private GameObject Popup;

    public int item_stack = 0;

    Item_Slot_Type item_Slot_Type;

    public void Init(Item_Scriptable item, int quantity = 0, Transform popup_Content = null, int slotIndex = -1, Item_Slot_Type item_Slot_Type = Item_Slot_Type.None)
    {
        Slot_Index = slotIndex;

        if (item == null)
        {
            data = null;
            item_Image.gameObject.SetActive(false);
            quantity_Text.text = "0";
            Equipped_Obj.SetActive(false);
            return;
        }

        if (popup_Content != null)
            content = popup_Content;

        data = item;

        this.item_Slot_Type = item_Slot_Type;

        item_Image.gameObject.SetActive(true);
        item_Image.sprite = Utils.Get_Item_Atlas(item.item_ID);
        item_Image.SetNativeSize();

        slot_Rect = item_Image.GetComponent<RectTransform>();
        slot_Rect.sizeDelta = new Vector2(slot_Rect.sizeDelta.x * 4, slot_Rect.sizeDelta.y * 4);

        quantity_Text.text = quantity.ToString();

        if (item_Slot_Type == Item_Slot_Type.Equipped)
        {
            Equipped_Obj.SetActive(false);
            return;
        }

        bool is_Equipped = false;

        if (item != null && Base_Manager.Inventory_Mng.inventory_Logic != null)
        {
            is_Equipped = Base_Manager.Inventory_Mng.inventory_Logic.Is_Equipped(item);
        }

        Equipped_Obj.SetActive(is_Equipped);
    }

    public void Reset_Data()
    {
        data = null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (data == null)
        {
            Debug.Log("데이터 없음");
            return;
        }

        if (item_Slot_Type == Item_Slot_Type.Shop_Inventory) return;

        if(data.item_Type == Item_Type.Material) return;

        Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Slot);

        if (Popup != null)
        {
            Base_Manager.Pool_Mng.pool_Dictionary[Pool_ID.UI_Des_PopUp].Return(Popup);
            Popup = null;
        }

        if (Base_Manager.Inventory_Mng.Acrtion_Panal_Holder.Count > 0)
        {
            Base_Manager.Inventory_Mng.Acrtion_Panal_Holder.Pop().GetComponent<Action_Slot_Panel>().Close_Panel();
            Debug.Log($"기존 액션 팝업 종료{Base_Manager.Inventory_Mng.Acrtion_Panal_Holder.Count}");
        }

        Base_Manager.Pool_Mng.Pooling_OBJ(Pool_ID.Inv_Item_Action_Panel).Get(obj =>
        {
            obj.GetComponent<Action_Slot_Panel>().Init(data, this);
            obj.transform.SetParent(content, false);
            RectTransform rect = obj.GetComponent<RectTransform>();
            RectTransform targetRect = GetComponent<RectTransform>();

            Utils.Set_Popup_Position(rect, targetRect);

            Base_Manager.Inventory_Mng.Acrtion_Panal_Holder.Push(obj);
        });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (data == null)
        {
            Debug.Log("데이터 없음");
            return;
        }

        if (Base_Manager.Inventory_Mng.Acrtion_Panal_Holder.Count > 0)
        {
            Debug.Log("액션 팝업 활성화 중");
            return;
        }

        Base_Manager.Pool_Mng.Pooling_OBJ(Pool_ID.UI_Des_PopUp).Get(obj =>
        {
            obj.GetComponent<UI_Des_PopUp>().init(data);
            obj.transform.SetParent(content, false);
            Popup = obj;

            RectTransform rect = obj.GetComponent<RectTransform>();
            RectTransform targetRect = GetComponent<RectTransform>();

            Utils.Set_Popup_Position(rect, targetRect);
        });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Popup != null)
        {
            Base_Manager.Pool_Mng.pool_Dictionary[Pool_ID.UI_Des_PopUp].Return(Popup);
            Popup = null;
        }
    }
}
