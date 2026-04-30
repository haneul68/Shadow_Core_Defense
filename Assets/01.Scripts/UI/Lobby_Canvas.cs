using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_Canvas : Base_Canvas
{
    public static Lobby_Canvas Instance { get; private set; }

    [SerializeField]
    private Button Character_Button, Shop_Button, Inventory_Button, Rune_Button, Setting_Button;

    [SerializeField]
    private TextMeshProUGUI gold_Text;
    [SerializeField]
    private TextMeshProUGUI dia_Text;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Base_Manager.UI_Mng.Init(this);
        Init_Action_Button();
    }

    private void OnEnable()
    {
        if (Base_Manager.Data_Mng != null)
        {
            Base_Manager.Data_Mng.On_Gold_Changed += Update_Gold_Text;
            Base_Manager.Data_Mng.On_Diamond_Changed += Update_Dia_Text;
        }
        Update_Gold_Text(Base_Manager.Data_Mng.Gold);
        Update_Dia_Text(Base_Manager.Data_Mng.Diamond);
    }

    private void OnDisable()
    {
        if (Base_Manager.Data_Mng != null) 
        {
            Base_Manager.Data_Mng.On_Gold_Changed -= Update_Gold_Text;
            Base_Manager.Data_Mng.On_Diamond_Changed -= Update_Dia_Text;
        }
    }
   
    private void Init_Action_Button() 
    {
        if (Shop_Button == null || Inventory_Button == null || Character_Button == null || Rune_Button == null || Setting_Button == null) 
        {
            Debug.Log("Shop_Button == null || Inventory_Button == null || Character_Button == null || Rune_Button == null || Setting_Button == null");
            return;
        }

        Shop_Button.onClick.RemoveAllListeners();
        Inventory_Button.onClick.RemoveAllListeners();
        Character_Button.onClick.RemoveAllListeners();
        Rune_Button.onClick.RemoveAllListeners();
        Setting_Button.onClick.RemoveAllListeners();

        Shop_Button.onClick.AddListener(() => 
        {
            Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Button_Click);
            Get_UI(Pool_ID.UI_Shop);
        });

        Inventory_Button.onClick.AddListener(() =>
        {
            Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Button_Click);
            Get_UI(Pool_ID.UI_Inventory);
        });

        Character_Button.onClick.AddListener(() =>
        {
            Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Button_Click);
            Get_UI(Pool_ID.UI_Character);
        });

        Rune_Button.onClick.AddListener(() =>
        {
            Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Button_Click);
            Get_UI(Pool_ID.UI_Rune);
        });

        Setting_Button.onClick.AddListener(() =>
        {
            Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Button_Click);
            Get_UI(Pool_ID.UI_Setting);
        });
    }

    private void Update_Gold_Text(int gold) 
    {
        if(gold_Text == null) return;
        
        gold_Text.text = gold.ToString();
    }

    private void Update_Dia_Text(int dia)
    {
        if (dia_Text == null) return;

        dia_Text.text = dia.ToString();
    }
    public override bool Get_Setting_UI()
    {
        bool handled = base.Get_Setting_UI();

        if (handled)
            return true;

        Get_UI(Pool_ID.UI_Setting);
        return true;
    }
}
