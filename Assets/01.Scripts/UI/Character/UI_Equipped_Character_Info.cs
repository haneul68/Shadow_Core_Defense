using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Equipped_Character_Info : MonoBehaviour
{
    [Header("Image")]
    [SerializeField] private Image[] character_Images;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI name_Text;
    [SerializeField] private TextMeshProUGUI level_Text;
    [SerializeField] private TextMeshProUGUI atk_Text;
    [SerializeField] private TextMeshProUGUI hp_Text;
    [SerializeField] private TextMeshProUGUI mp_Text;
    [SerializeField] private TextMeshProUGUI stamina_Text;
    [SerializeField] private TextMeshProUGUI moveSpeed_Text;
    [SerializeField] private TextMeshProUGUI rarity_Text;
    [SerializeField] private TextMeshProUGUI des_Text;

    [Header("Skill UI")]
    [SerializeField] private UI_Player_Skill_List skill_List_UI;

    private void OnEnable()
    {
        Refresh();

        if (Base_Manager.Character_Mng != null)
        {
            Base_Manager.Character_Mng.On_Character_Changed += On_Character_Changed;
            Base_Manager.Character_Mng.On_Character_Equipped += On_Character_Equipped;
        }
    }

    private void OnDisable()
    {
        if (Base_Manager.Character_Mng != null)
        {
            Base_Manager.Character_Mng.On_Character_Changed -= On_Character_Changed;
            Base_Manager.Character_Mng.On_Character_Equipped -= On_Character_Equipped;
        }
    }

    private void On_Character_Changed(string ch_Name)
    {
        Refresh();
    }

    private void On_Character_Equipped(string ch_Name)
    {
        Refresh();
    }

    public void Refresh()
    {
        string equippedName = Base_Manager.Character_Mng.Equipped_Character_Name;

        if (string.IsNullOrEmpty(equippedName))
        {
            Set_Empty();
            return;
        }

        if (!Base_Manager.Data_Mng.p_Character_Holder.ContainsKey(equippedName))
        {
            Set_Empty();
            return;
        }

        Character_Holder ch_Holder = Base_Manager.Data_Mng.p_Character_Holder[equippedName];
        Character_Scriptable data = ch_Holder.Data;

        if (data == null)
        {
            Set_Empty();
            return;
        }

        name_Text.text = data.Character_Name;
        level_Text.text = $"<color=#00FFF0>LV.</color>{ch_Holder.holder.Level:D2}";
        atk_Text.text = Base_Manager.Character_Mng.Get_ATK(equippedName).ToString("F0");
        hp_Text.text = Base_Manager.Character_Mng.Get_Max_HP(equippedName).ToString("F0");
        mp_Text.text = Base_Manager.Character_Mng.Get_Max_MP(equippedName).ToString("F0");
        stamina_Text.text = Base_Manager.Character_Mng.Get_Max_Stamina(equippedName).ToString("F0");
        moveSpeed_Text.text = Base_Manager.Character_Mng.Get_Move_Speed(equippedName).ToString("F1");
        rarity_Text.text = data.rarity.ToString();
        des_Text.text = data.Character_DES;

        Sprite sprite = Utils.Get_Character_Atlas(data.name);

        for (int i = 0; i < character_Images.Length; i++)
        {
            if (character_Images[i] == null) continue;

            character_Images[i].gameObject.SetActive(true);
            character_Images[i].sprite = sprite;
        }

        if (skill_List_UI != null)
            skill_List_UI.Refresh_Equipped_Character_Skills();
    }

    private void Set_Empty()
    {
        name_Text.text = "";
        rarity_Text.text = "";
        des_Text.text = "";

        level_Text.text = "<color=#00FFF0>LV.</color>00";
        atk_Text.text = "0";
        hp_Text.text = "0";
        mp_Text.text = "0";
        stamina_Text.text = "0";
        moveSpeed_Text.text = "0";

        for (int i = 0; i < character_Images.Length; i++)
        {
            if (character_Images[i] == null) continue;
            character_Images[i].gameObject.SetActive(false);
        }

        if (skill_List_UI != null)
            skill_List_UI.Clear();
    }
}