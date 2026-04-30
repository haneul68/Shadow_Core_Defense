using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Character_Purchase_Panel : MonoBehaviour
{
    [SerializeField] private Button close_Button;
    [SerializeField] private Button purchase_Button;

    [SerializeField] private TextMeshProUGUI character_Name_Text;
    [SerializeField] private TextMeshProUGUI price_Text;

    private Character_Scriptable data;

    public void Init(Character_Scriptable character_Data)
    {
        data = character_Data;

        if (character_Name_Text != null)
            character_Name_Text.text = data.Character_Name;

        if (price_Text != null) 
        {
            price_Text.text = data.Price.ToString();

            if (Base_Manager.Data_Mng.Diamond >= data.Price)
            {
                price_Text.color = Color.white;
            }
            else
            {
                price_Text.color = Color.red;
            }
        }
         
        close_Button.onClick.RemoveAllListeners();
        close_Button.onClick.AddListener(()=> 
        {
            Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Button_Click);
            Close_Panel(); 
        });

        purchase_Button.onClick.RemoveAllListeners();
        purchase_Button.onClick.AddListener(() =>
        {
            Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Button_Buy);
            OnClick_Purchase_Button(); 
        });

        purchase_Button.interactable = Base_Manager.Data_Mng.Diamond >= data.Price;
    }

    private void OnClick_Purchase_Button()
    {
        if (data == null) return;

        bool success = Base_Manager.Character_Mng.Buy_Character(data.name);

        if (!success)
        {
            Debug.Log("캐릭터 구매 실패");
            return;
        }

        Debug.Log($"캐릭터 구매 성공 : {data.Character_Name}");
        Close_Panel();
    }

    public void Close_Panel()
    {
        gameObject.SetActive(false);

        if (Base_Manager.Character_Mng.Character_Purchase_Panel_Holder.Count > 0)
        {
            GameObject topObj = Base_Manager.Character_Mng.Character_Purchase_Panel_Holder.Peek();

            if (topObj == gameObject)
            {
                Base_Manager.Character_Mng.Character_Purchase_Panel_Holder.Pop();
            }
        }

        Base_Manager.Pool_Mng.pool_Dictionary[Pool_ID.Character_Purchase_Panel].Return(gameObject);
    }
}