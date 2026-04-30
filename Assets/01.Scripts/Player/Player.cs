using UnityEngine;

public class Player : Character
{
    public void Init_Player(Character_Scriptable data) 
    {
        Init(data);
        health_Manager.Init();

        Base_Manager.Pool_Mng.Pooling_OBJ(Pool_ID.Minimap_Player_Icon).Get(icon =>
        {
            RectTransform rect = icon.GetComponent<RectTransform>();

            rect.SetParent(In_Game_Canvas.Instance.Minimap_Rect);
            rect.localScale = Vector3.one;

            minimap_Icon = rect;

            Minimap_Manager.Instance.Register(this);
        });
    }
}
