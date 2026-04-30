using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_End_Game_Item_Slot : MonoBehaviour
{
    [SerializeField] private Image item_Image;
    [SerializeField] private TextMeshProUGUI quantity_Text;

    public void Init(Material_Reward_Data data)
    {
        if (data == null || data.item == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        if (item_Image != null)
        {
            item_Image.sprite = Utils.Get_Item_Atlas(data.item.item_ID);
            item_Image.SetNativeSize();

            RectTransform rect = item_Image.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x * 2.5f, rect.sizeDelta.y * 2.5f);
        }

        if (quantity_Text != null)
        {
            quantity_Text.text = $"x{data.amount:00}";
        }
    }
}