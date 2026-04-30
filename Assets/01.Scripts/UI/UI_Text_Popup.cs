using TMPro;
using UnityEngine;

public class UI_Text_Popup : MonoBehaviour
{
    [SerializeField] 
    TextMeshProUGUI Text_Popup_Text;
    [SerializeField]
    private Pool_ID pool_ID = Pool_ID.UI_Text_Popup;

    public void Init(string temp, Color color)
    {
        Text_Popup_Text.color = color;
        Text_Popup_Text.text = temp;

        Invoke(nameof(Return_To_Pool), 2f);
    }

    void Return_To_Pool()
    {
        Base_Manager.Pool_Mng.pool_Dictionary[pool_ID].Return(this.gameObject);
    }
}