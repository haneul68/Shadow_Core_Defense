using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Player_Skill_Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image skill_Image;

    private Player_Skill_Base skill_Data;
    private GameObject popup;
    private Transform popup_Content;

    public void Init(Player_Skill_Base skill, Transform popupContent = null)
    {
        skill_Data = skill;
        popup_Content = popupContent;

        if (skill == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        skill_Image.sprite = skill.Skill_Icon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (skill_Data == null) return;

        Base_Manager.Pool_Mng.Pooling_OBJ(Pool_ID.UI_Skill_Des_Popup).Get(obj =>
        {
            popup = obj;

            obj.transform.SetParent(popup_Content != null ? popup_Content : transform.root, false);
            obj.GetComponent<UI_Skill_Des_Popup>().Init(skill_Data);

            RectTransform rect = obj.GetComponent<RectTransform>();
            RectTransform targetRect = GetComponent<RectTransform>();

            Utils.Set_Popup_Position(rect, targetRect);
        });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (popup == null) return;

        Base_Manager.Pool_Mng.pool_Dictionary[Pool_ID.UI_Skill_Des_Popup].Return(popup);
        popup = null;
    }
}