using UnityEngine;
using UnityEngine.UI;

public class UI_Rune_Equipped_Slot : MonoBehaviour
{
    [SerializeField] 
    private Image rune_Image_01;
    [SerializeField]
    private Image rune_Image_02;

    public void Init(string rune_Id)
    {
        bool has_Rune = !string.IsNullOrEmpty(rune_Id);

        Sprite rune_Sprite = null;

        if (has_Rune)
            rune_Sprite = Utils.Get_Rune_Atlas(rune_Id);

        if (rune_Image_01 != null)
        {
            rune_Image_01.enabled = has_Rune;
            rune_Image_01.sprite = rune_Sprite;
        }

        if (rune_Image_02 != null)
        {
            rune_Image_02.enabled = has_Rune;
            rune_Image_02.sprite = rune_Sprite;
        }
    }
}