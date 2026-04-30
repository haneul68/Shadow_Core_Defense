using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGame_Item_Slot : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image item_Image;
    [SerializeField] private Image cooldown_Fill_Image;
    [SerializeField] private TextMeshProUGUI quantity_Text;

    private Item_Scriptable current_Item;
    RectTransform slot_Rect;

    public Item_Scriptable Current_Item => current_Item;

    private float current_Cooldown;
    private float max_Cooldown;

    public void Init(Item_Scriptable item, int quantity)
    {
        current_Item = item;

        if (item == null)
        {
            item_Image.gameObject.SetActive(false);
            quantity_Text.text = "";
            Set_Cooldown(0f, 0f);
            return;
        }

        item_Image.gameObject.SetActive(true);
        item_Image.sprite = Utils.Get_Item_Atlas(item.item_ID);
        item_Image.SetNativeSize();

        slot_Rect = item_Image.GetComponent<RectTransform>();
        slot_Rect.sizeDelta = new Vector2(slot_Rect.sizeDelta.x * 4, slot_Rect.sizeDelta.y * 4);

        quantity_Text.text = quantity > 0 ? quantity.ToString() : "";

        Set_Cooldown(0f, 0f);
    }

    public void Refresh_Quantity(int quantity)
    {
        quantity_Text.text = quantity > 0 ? quantity.ToString() : "";
    }

    public void Set_Cooldown(float current, float max)
    {
        current_Cooldown = current;
        max_Cooldown = max;

        if (cooldown_Fill_Image == null) return;

        if (max_Cooldown <= 0f || current_Cooldown <= 0f)
        {
            cooldown_Fill_Image.fillAmount = 0f;
            cooldown_Fill_Image.gameObject.SetActive(false);
            return;
        }

        cooldown_Fill_Image.gameObject.SetActive(true);
        cooldown_Fill_Image.fillAmount = current_Cooldown / max_Cooldown;
    }


    public void Start_Cooldown()
    {
        if (current_Item == null) return;

        if (current_Item.cool_Down <= 0f) return;
        
        Set_Cooldown(current_Item.cool_Down, current_Item.cool_Down);
    }

    public void Tick_Cooldown(float deltaTime)
    {
        if (current_Cooldown <= 0f) return;

        current_Cooldown -= deltaTime;

        if (current_Cooldown < 0f)
            current_Cooldown = 0f;

        Set_Cooldown(current_Cooldown, max_Cooldown);
    }

    public bool Is_On_Cooldown()
    {
        return current_Cooldown > 0f;
    }

}
