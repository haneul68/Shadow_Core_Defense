using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Rune_Slot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] 
    private Image rune_Image_01;
    [SerializeField]
    private Image rune_Image_02;
    [SerializeField] 
    private TextMeshProUGUI level_Text;
    [SerializeField] 
    private GameObject not_Owned_Obj;
    [SerializeField]
    private GameObject equipped_Obj;
    [SerializeField] 
    private Image level_Fill;

    private Rune_Scriptable rune_Data;
    private Transform popup_Content;

    public void Init(Rune_Scriptable rune_Data, Transform popup_Content)
    {
        this.rune_Data = rune_Data;
        this.popup_Content = popup_Content;

        if (rune_Data == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        Sprite rune_Sprite = Utils.Get_Rune_Atlas(rune_Data.item_ID);

        if (rune_Image_01 != null)
            rune_Image_01.sprite = rune_Sprite;

        if (rune_Image_02 != null)
            rune_Image_02.sprite = rune_Sprite;

        Refresh();
    }

    public void Refresh()
    {
        if (rune_Data == null) return;
        if (Base_Manager.Rune_Mng == null) return;

        Rune_Holder rune_Holder = Base_Manager.Rune_Mng.Get_Rune_Holder(rune_Data.item_ID);
        if (rune_Holder == null) return;

        bool is_Owned = rune_Holder.is_Owned;
        bool is_Equipped = Base_Manager.Rune_Mng.Is_Equipped(rune_Data.item_ID);

        if (not_Owned_Obj != null)
            not_Owned_Obj.SetActive(!is_Owned);

        if (equipped_Obj != null)
            equipped_Obj.SetActive(is_Owned && is_Equipped);

        if (level_Text != null)
            level_Text.text = is_Owned ? $"LV.{rune_Holder.level:00}" : "LV.00";

        if (level_Fill != null)
        {
            float fill_Value = 0f;

            if (is_Owned && rune_Data.max_Level > 0)
                fill_Value = (float)rune_Holder.level / rune_Data.max_Level;

            level_Fill.fillAmount = fill_Value;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (rune_Data == null) return;
        if (popup_Content == null) return;

        Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Slot);

        UI_Rune.Instance.Close_Action_Panel();

        Base_Manager.Pool_Mng.Pooling_OBJ(Pool_ID.UI_Rune_Action_Panel).Get(obj =>
        {
            obj.transform.SetParent(popup_Content, false);

            UI_Rune_Action_Panel action_Panel = obj.GetComponent<UI_Rune_Action_Panel>();

            if (action_Panel != null)
            {
                action_Panel.Init(rune_Data);
            }

            UI_Rune.Instance.Set_Current_Action_Panel(obj);
        });
    }
}