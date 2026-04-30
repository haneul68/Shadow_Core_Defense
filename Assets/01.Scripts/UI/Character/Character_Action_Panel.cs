using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Character_Action_Panel : MonoBehaviour
{
    [SerializeField] private Button close_Button;
    [SerializeField] private Button equip_Button;
    [SerializeField] private TextMeshProUGUI equip_Button_Text;

    private Character_Scriptable data;
    private Character_Slot owner_Slot;

    public void Init(Character_Scriptable character_Data, Character_Slot slot)
    {
        data = character_Data;
        owner_Slot = slot;

        close_Button.onClick.RemoveAllListeners();
        close_Button.onClick.AddListener(() =>
        {
            Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Button_Click);
            Close_Panel();
        });

        equip_Button.onClick.RemoveAllListeners();
        equip_Button.onClick.AddListener(()=> 
        {
            Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Button_Click);
            OnClick_Equip_Button();
        });

        Refresh_Button_Text();
    }

    private void Refresh_Button_Text()
    {
        if (data == null) return;

        bool is_Equipped = Base_Manager.Character_Mng.Is_Equipped_Character(data.name);
        equip_Button_Text.text = is_Equipped ? "ÀåÂø ÇØÁ¦" : "ÀåÂø";
    }

    private void OnClick_Equip_Button()
    {
        if (data == null) return;

        bool is_Equipped = Base_Manager.Character_Mng.Is_Equipped_Character(data.name);

        if (is_Equipped)
        {
            Base_Manager.Character_Mng.UnEquip_Character(data.name);
        }
        else
        {
            Base_Manager.Character_Mng.Equip_Character(data.name);
        }

        Close_Panel();
    }

    public void Close_Panel()
    {       
        gameObject.SetActive(false);

        if (Base_Manager.Character_Mng.Character_Action_Panel_Holder.Count > 0)
        {
            GameObject topObj = Base_Manager.Character_Mng.Character_Action_Panel_Holder.Peek();

            if (topObj == gameObject)
            {
                Base_Manager.Character_Mng.Character_Action_Panel_Holder.Pop();
            }
        }

        Base_Manager.Pool_Mng.pool_Dictionary[Pool_ID.Character_Action_Panel].Return(gameObject);
    }
}