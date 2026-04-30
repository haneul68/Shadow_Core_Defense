using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Skill_Des_Popup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skill_Name_Text;
    [SerializeField] private TextMeshProUGUI skill_Desc_Text;

    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void Init(Player_Skill_Base skill)
    {
        if (skill == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        if (skill_Name_Text != null)
            skill_Name_Text.text = skill.Get_Skill_Name();

        if (skill_Desc_Text != null)
            skill_Desc_Text.text = skill.Get_Skill_Description();

        Refresh_Layout();
    }

    private void Refresh_Layout()
    {
        if (skill_Name_Text != null)
            skill_Name_Text.ForceMeshUpdate();

        if (skill_Desc_Text != null)
            skill_Desc_Text.ForceMeshUpdate();

        Canvas.ForceUpdateCanvases();

        if (rect != null)
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);

        RectTransform parentRect = transform.parent as RectTransform;
        if (parentRect != null)
            LayoutRebuilder.ForceRebuildLayoutImmediate(parentRect);
    }
}