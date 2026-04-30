using TMPro;
using UnityEngine;
using DG.Tweening;

public class UI_Character_Stat_Info : MonoBehaviour
{
    [SerializeField] 
    private RectTransform panel_Rect;

    [SerializeField] 
    private TextMeshProUGUI atk_Text;
    [SerializeField] 
    private TextMeshProUGUI hp_Text;
    [SerializeField] 
    private TextMeshProUGUI mp_Text;
    [SerializeField] 
    private TextMeshProUGUI stamina_Text;
    [SerializeField] 
    private TextMeshProUGUI speed_Text;

    [SerializeField]
    private float close_X = 350f;
    [SerializeField] 
    private float open_X = 0f;
    [SerializeField] 
    private float move_Duration = 0.25f;

    private bool is_Open;
    private Tween move_Tween;

    private void Awake()
    {
        if (panel_Rect == null)
            panel_Rect = GetComponent<RectTransform>();

        Set_X(close_X);
    }

    private void Update()
    {
        if (!is_Open) return;

        Character character = Base_Manager.Character_Mng.current_Character;

        if (character == null)
            return;

        Refresh_Text(character);
    }

    public void Init()
    {
        Character character = Base_Manager.Character_Mng.current_Character;

        if (character == null)
        {
            Debug.Log("character == null");
            return;
        }

        Refresh_Text(character);
        Toggle_Move();
    }

    private void Refresh_Text(Character character)
    {
        Set_Text(atk_Text, character.Base_ATK, character.Final_ATK);
        Set_Text(hp_Text, character.Base_Max_HP, character.Final_Max_HP);
        Set_Text(mp_Text, character.Base_Max_MP, character.Final_Max_MP);
        Set_Text(stamina_Text, character.Base_Max_Stamina, character.Final_Max_Stamina);
        Set_Text(speed_Text, character.Base_Move_Speed, character.Final_Move_Speed);
    }

    private void Set_Text(TextMeshProUGUI text, double base_Value, double final_Value)
    {
        if (text == null) return;

        int base_Int = (int)base_Value;   
        int final_Int = (int)final_Value;  

        int bonus = final_Int - base_Int;

        if (bonus > 0)
        {
            text.text = $"{base_Int} <color=#FFFFFF>+{bonus}</color>";
        }
        else
        {
            text.text = $"{base_Int}";
        }
    }

    private void Toggle_Move()
    {
        is_Open = !is_Open;

        float target_X = is_Open ? open_X : close_X;

        move_Tween?.Kill();
        move_Tween = panel_Rect.DOAnchorPosX(target_X, move_Duration).SetEase(Ease.OutQuad);
    }

    private void Set_X(float x)
    {
        Vector2 pos = panel_Rect.anchoredPosition;
        pos.x = x;
        panel_Rect.anchoredPosition = pos;
    }
}