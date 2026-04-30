using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Shop_Item_Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public int Slot_Index { get; private set; }

    [SerializeField]
    private Image item_Image;
    [SerializeField]
    private TextMeshProUGUI total_Amount_Text;
    [SerializeField]
    private TextMeshProUGUI price_Text;
    [SerializeField]
    private GameObject sold_Out_Obj;

    Shop_Item_Data data;
    RectTransform slot_Rect;

    private Transform content;

    private GameObject Popup;

    public int item_stack = 0;

    public void Init(Shop_Item_Data item, Transform popup_Content = null, int slotIndex = -1)
    {
        Slot_Index = slotIndex;

        if (item.data == null)
        {
            return;
        }

        if (popup_Content != null)
            content = popup_Content;

        data = item;


        item_Image.gameObject.SetActive(true);
        item_Image.sprite = Utils.Get_Item_Atlas(item.item_ID);
        item_Image.SetNativeSize();

        slot_Rect = item_Image.GetComponent<RectTransform>();
        slot_Rect.sizeDelta = new Vector2(slot_Rect.sizeDelta.x * 4, slot_Rect.sizeDelta.y * 4);

        total_Amount_Text.text = item.total_Amount.ToString();

        bool is_Sold_Out = item.total_Amount == 0 ? true : false;

        sold_Out_Obj.SetActive(is_Sold_Out);

        price_Text.text = item.price.ToString();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (data.data == null)
        {
            Debug.Log("데이터 없음");
            return;
        }

        if (data.total_Amount <= 0)
        {
            Debug.Log("매진된 아이템");
            return;
        }

        Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Slot);

        if (Popup != null)
        {
            Base_Manager.Pool_Mng.pool_Dictionary[Pool_ID.UI_Des_PopUp].Return(Popup);
            Popup = null;
        }

        if (Shop_Manager.Instance.Shop_Acrtion_Panal_Holder.Count > 0)
        {
            Shop_Manager.Instance.Shop_Acrtion_Panal_Holder.Pop().GetComponent<Shop_Action_Slot_Panel>().Close_Panel();
            Debug.Log($"기존 액션 팝업 종료{Shop_Manager.Instance.Shop_Acrtion_Panal_Holder.Count}");
        }

        Base_Manager.Pool_Mng.Pooling_OBJ(Pool_ID.Shop_Item_Action_Panel).Get(obj =>
        {
            obj.GetComponent<Shop_Action_Slot_Panel>().Init(data, this);
            obj.transform.SetParent(content, false);
            RectTransform rect = obj.GetComponent<RectTransform>();
            RectTransform targetRect = GetComponent<RectTransform>();

            Utils.Set_Popup_Position(rect, targetRect);

            Shop_Manager.Instance.Shop_Acrtion_Panal_Holder.Push(obj);
        });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (data.data == null)
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
            obj.GetComponent<UI_Des_PopUp>().init(data.data);
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
