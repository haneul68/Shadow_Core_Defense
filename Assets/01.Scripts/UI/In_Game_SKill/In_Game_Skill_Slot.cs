using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class In_Game_Skill_Slot : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image skill_Image;
    [SerializeField] private Image cooldown_Fill;
    [SerializeField] private TextMeshProUGUI mana_Text;

    [Header("Key Text")]
    [SerializeField] private TextMeshProUGUI key_Text_1;
    [SerializeField] private TextMeshProUGUI key_Text_2;

    private Player_Skill_Base skill;
    private Player_Skill_Manager owner_Skill_Manager;

    public void Init(Player_Skill_Base skill, Player_Skill_Manager skillManager)
    {
        this.skill = skill;
        owner_Skill_Manager = skillManager;

        if (skill == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        if (skill_Image != null)
            skill_Image.sprite = skill.Skill_Icon;

        if (cooldown_Fill != null)
            cooldown_Fill.fillAmount = 0f;

        if (mana_Text != null)
            mana_Text.text = skill.Is_Passive ? string.Empty : skill.Mana_Cost.ToString("F0");

        string key_Text = owner_Skill_Manager != null ? owner_Skill_Manager.Get_Skill_Key_Text(skill) : string.Empty;

        if (key_Text_1 != null)
            key_Text_1.text = key_Text;

        if (key_Text_2 != null)
            key_Text_2.text = key_Text;
    }

    public void Tick(float deltaTime)
    {
        if (skill == null) return;
        if (cooldown_Fill == null) return;

        if (skill.Is_Passive)
        {
            cooldown_Fill.fillAmount = 0f;
            return;
        }

        float remain = skill.Get_Remain_Cooldown();
        float max = skill.Cooldown;

        if (max <= 0f)
        {
            cooldown_Fill.fillAmount = 0f;
            return;
        }

        cooldown_Fill.fillAmount = remain / max;
    }
}
