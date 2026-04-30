using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Character_Slot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image[] character_Images;
    [SerializeField] private Image level_Fill;
    [SerializeField] private TextMeshProUGUI level_Text;
    [SerializeField] private GameObject equipped_Obj;
    [SerializeField] private Button lock_Button;
    [SerializeField] private TextMeshProUGUI price_Text;

    private Character_Scriptable data;
    private Transform popup_Content;

    public void Init(Character_Scriptable character_Data, Transform panel_Content = null)
    {
        data = character_Data;

        if (panel_Content != null)
            popup_Content = panel_Content;

        if (data == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        Sprite sprite = Utils.Get_Character_Atlas(data.name);

        foreach (var img in character_Images)
        {
            if (img == null) continue;

            img.gameObject.SetActive(true);
            img.sprite = sprite;
        }

        bool isOwned = Base_Manager.Character_Mng.Is_Owned_Character(data.name);
        bool isEquipped = Base_Manager.Character_Mng.Is_Equipped_Character(data.name);

        if (isOwned)
        {
            int level = Base_Manager.Character_Mng.Get_Level(data.name);
            float fillAmount = Base_Manager.Character_Mng.Get_Exp_Fill_Amount(data.name);

            level_Text.text = $"Lv.{level}";
            level_Fill.fillAmount = fillAmount;
        }
        else
        {
            level_Text.text = "Lv.0";
            level_Fill.fillAmount = 0f;
        }

        equipped_Obj.SetActive(isEquipped);
        lock_Button.gameObject.SetActive(!isOwned);

        if (price_Text != null)
        {
            price_Text.text = !isOwned ? data.Price.ToString() : string.Empty;
        }

        lock_Button.onClick.RemoveAllListeners();
        lock_Button.onClick.AddListener(OnClick_Lock_Button);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (data == null) return;

        if (lock_Button.gameObject.activeSelf)
            return;

        Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Slot);

        if (Base_Manager.Character_Mng.Character_Action_Panel_Holder.Count > 0)
        {
            Base_Manager.Character_Mng.Character_Action_Panel_Holder.Pop()
                .GetComponent<Character_Action_Panel>()
                .Close_Panel();
        }

        Base_Manager.Pool_Mng.Pooling_OBJ(Pool_ID.Character_Action_Panel).Get(obj =>
        {
            obj.GetComponent<Character_Action_Panel>().Init(data, this);

            Transform parent = popup_Content != null ? popup_Content : transform.parent;
            obj.transform.SetParent(parent, false);

            RectTransform rect = obj.GetComponent<RectTransform>();
            RectTransform target_Rect = GetComponent<RectTransform>();

            Utils.Set_Popup_Position(rect, target_Rect);

            Base_Manager.Character_Mng.Character_Action_Panel_Holder.Push(obj);
        });
    }

    private void OnClick_Lock_Button()
    {
        if (data == null) return;

        if (Base_Manager.Character_Mng.Character_Purchase_Panel_Holder.Count > 0)
        {
            Base_Manager.Character_Mng.Character_Purchase_Panel_Holder.Peek()
                .GetComponent<Character_Purchase_Panel>()
                .Close_Panel();
        }

        Base_Manager.Pool_Mng.Pooling_OBJ(Pool_ID.Character_Purchase_Panel).Get(obj =>
        {
            obj.GetComponent<Character_Purchase_Panel>().Init(data);

            Transform parent = popup_Content != null ? popup_Content : transform.parent;
            obj.transform.SetParent(parent, false);

            Base_Manager.Character_Mng.Character_Purchase_Panel_Holder.Push(obj);
        });
    }
}