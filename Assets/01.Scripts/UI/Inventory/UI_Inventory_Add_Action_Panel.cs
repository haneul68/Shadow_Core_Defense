using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory_Add_Action_Panel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cost_Text;
    [SerializeField] private Button add_Button;
    [SerializeField] private Button close_Button;

    private UI_Inventory ui_Inventory;
    private int diamond_Cost;

    private void Awake()
    {
        if (add_Button != null)
            add_Button.onClick.AddListener(OnClick_Add_Button);

        if (close_Button != null)
            close_Button.onClick.AddListener(Close_Panel);
    }

    public void Init(UI_Inventory ui_Inventory, int diamond_Cost)
    {
        this.ui_Inventory = ui_Inventory;
        this.diamond_Cost = diamond_Cost;

        if (cost_Text != null)
            cost_Text.text = $"다이아 <color=#00CFFF>{diamond_Cost}</color>개를 사용하여 추가하시겠습니까?";
    }

    private void OnClick_Add_Button()
    {
        if (ui_Inventory == null)
        {
            Debug.Log("ui_Inventory == null");
            Close_Panel();
            return;
        }
        if (ui_Inventory.Try_Expand_Inventory_With_Diamond(diamond_Cost) == true)
        {
            Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Button_Buy);
        }
        else 
        {
            Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Rune_Upgrade_Fail);
        }

        Close_Panel();
    }

    public void Close_Panel()
    {
        Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Button_Click);
        Base_Manager.Pool_Mng.pool_Dictionary[Pool_ID.Inv_Add_Action_Panel].Return(gameObject);
    }
}