using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Ability_Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private Animator animator;
    private UI_Ability uI_Ability;

    [Header("REF")]
    [SerializeField] private Ability ability;
    [Space(10)]
    [SerializeField] private TextMeshProUGUI ability_Name_Text;
    [SerializeField] private TextMeshProUGUI ability_Des_Text;
    [SerializeField] private Image ability_Image;
    private bool is_Selected = false;

    private void Awake()
    {
        if(animator == null)
            animator = GetComponent<Animator>();    
    }

    public void Init(UI_Ability uI_Ability) 
    {
        this.uI_Ability = uI_Ability;
        is_Selected = false;
    }

    public void Set_Ability(Ability ability)
    {
        this.ability = ability;

        if (ability != null)
        {
            if (ability_Name_Text != null)
                ability_Name_Text.text = ability.ability_Name;

            if (ability_Des_Text != null)
                ability_Des_Text.text = ability.Get_Description();
            if (ability_Image != null)
                ability_Image.sprite = ability.ablilty_Image;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (is_Selected) return;

        Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Button_Hover);

        animator.SetBool(Animation_Parameter_Hash.Ablilty_Slot_Hover_Hash, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (is_Selected) return;
        animator.SetBool(Animation_Parameter_Hash.Ablilty_Slot_Hover_Hash, false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (is_Selected) return;

        if (ability == null)
        {
            Debug.LogError("Ability 없음");
            return;
        }

        animator.SetTrigger(Animation_Parameter_Hash.Ablilty_Slot_Select_Hash);
        uI_Ability.Set_Active_Lock_Obj(true);

        Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Ability_Select);

        Apply_Ability();

        Invoke(nameof(Close_UI),1f);
    }
    private void Apply_Ability()
    {
        var character = Base_Manager.Character_Mng.current_Character;

        if (character == null)
        {
            Debug.LogError("캐릭터 없음");
            return;
        }

        ability.Apply(character.gameObject);
    }
    public void Close_UI() 
    {
        InGame_State_Manager.State_Change(InGame_State.READY);
        uI_Ability.gameObject.SetActive(false);
        animator.Rebind();
        animator.Update(0f);
    }
}
