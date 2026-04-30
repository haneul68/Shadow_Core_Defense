using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop_Action_Slot_Panel : MonoBehaviour, IPointerExitHandler
{
    [SerializeField] private Button buy_Button;
    [SerializeField] private Button Exit_Button;
    [SerializeField] private Button plus_Button;
    [SerializeField] private Button minus_Button;

    [SerializeField] private TextMeshProUGUI quantity_Text;


    [SerializeField] private Image item_Image;

    private Shop_Item_Data current_Item;
    private Shop_Item_Slot current_Slot;

    private bool is_Processing = false;

    private int buy_Count = 1;
    private const int MIN_BUY_COUNT = 1;

    RectTransform slot_Rect;

    public void Init(Shop_Item_Data item, Shop_Item_Slot slot)
    {
        current_Item = item;
        current_Slot = slot;
        is_Processing = false;

        buy_Count = 1;
        Refresh_Quantity_Text();

        item_Image.gameObject.SetActive(true);
        item_Image.sprite = Utils.Get_Item_Atlas(item.item_ID);
        item_Image.SetNativeSize();

        slot_Rect = item_Image.GetComponent<RectTransform>();
        slot_Rect.sizeDelta = new Vector2(slot_Rect.sizeDelta.x * 7, slot_Rect.sizeDelta.y * 7);

        Exit_Button.onClick.RemoveAllListeners();
        Exit_Button.onClick.AddListener(() => 
        {
            Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Button_Click);
            Close_Panel();
        });

        buy_Button.onClick.RemoveAllListeners();
        buy_Button.onClick.AddListener(()=> 
        {
            Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Button_Click);
            Buy_Item();
        });

        plus_Button.onClick.RemoveAllListeners();
        plus_Button.onClick.AddListener(() =>
        {
            Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Shop_Add_Button);
            Add_Count();
        });

        minus_Button.onClick.RemoveAllListeners();
        minus_Button.onClick.AddListener(()=> 
        {
            Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Shop_Add_Button);
            Subtract_Count();
        });
    }

    private void Add_Count()
    {
        if (current_Item == null) return;

        if (buy_Count >= current_Item.total_Amount)
        {
            buy_Count = current_Item.total_Amount;
            Refresh_Quantity_Text();
            return;
        }

        buy_Count++;
        Refresh_Quantity_Text();
    }

    private void Subtract_Count()
    {
        if (buy_Count <= MIN_BUY_COUNT)
        {
            buy_Count = MIN_BUY_COUNT;
            Refresh_Quantity_Text();
            return;
        }

        buy_Count--;
        Refresh_Quantity_Text();
    }

    private void Refresh_Quantity_Text()
    {
        if (quantity_Text == null) return;
        quantity_Text.text = buy_Count.ToString();
    }

    private void Buy_Item()
    {
        if (is_Processing) return;
        if (current_Item == null) return;

        is_Processing = true;

        if (Shop_Manager.Instance.Try_Buy_Item(current_Item.item_ID, buy_Count))
        {
            Debug.Log($"구매 성공, 구매 수량: {buy_Count}, 남은 수량: {current_Item.total_Amount}");
            Lobby_Canvas.Instance.Get_Text_Pop_Up($"{Base_Manager.Data_Mng.Item_Data[current_Item.item_ID]}을/를 {buy_Count}개 구매하였습니다", Color.white);
            Close_Panel();
            return;
        }

        Debug.Log("구매 실패");
        is_Processing = false;
        Close_Panel();
    }

    public void Close_Panel()
    {
        Base_Manager.Pool_Mng.pool_Dictionary[Pool_ID.Shop_Item_Action_Panel].Return(gameObject);
        Shop_Manager.Instance.Shop_Acrtion_Panal_Holder.Clear();
        current_Item = null;
        current_Slot = null;
        buy_Count = 1;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Close_Panel();
    }
}