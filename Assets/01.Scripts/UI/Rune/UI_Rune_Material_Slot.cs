using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Rune_Material_Slot : MonoBehaviour
{
    [SerializeField] 
    private Image item_Image;
    [SerializeField] 
    private TextMeshProUGUI current_Count_Text;
    [SerializeField] 
    private TextMeshProUGUI need_Count_Text;

    public void Init(Material_Item item_Data, int current_Count, int need_Count)
    {
        if (item_Data == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        if (item_Image != null)
        {
            item_Image.sprite = Utils.Get_Item_Atlas(item_Data.item_ID);
            item_Image.SetNativeSize();

            RectTransform rect = item_Image.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.sizeDelta = new Vector2(rect.sizeDelta.x * 4f, rect.sizeDelta.y * 4f);
            }
        }

        if (current_Count_Text != null)
            current_Count_Text.text = current_Count.ToString();

        if (need_Count_Text != null)
            need_Count_Text.text = need_Count.ToString();
    }
}