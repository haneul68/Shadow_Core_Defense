using UnityEngine;
using UnityEngine.U2D;

public class Utils
{
    public static SpriteAtlas Item_Atlas = Resources.Load<SpriteAtlas>("Item_Atlas");
    public static SpriteAtlas Character_Atlas = Resources.Load<SpriteAtlas>("Character_Atlas");
    public static SpriteAtlas Rune_Atlas = Resources.Load<SpriteAtlas>("Rune_Atlas");
    public static Sprite Get_Item_Atlas(string id)
    {
        if (Item_Atlas == null) Debug.Log("없음");
        return Item_Atlas.GetSprite(id);
    }
    public static Sprite Get_Character_Atlas(string id)
    {
        if (Character_Atlas == null) Debug.Log("없음");
        return Character_Atlas.GetSprite(id);
    }
    public static Sprite Get_Rune_Atlas(string id)
    {
        if (Rune_Atlas == null) Debug.Log("없음");
        return Rune_Atlas.GetSprite(id);
    }
    public static double Calculate_Value(double base_Value, int level, float value) 
    {
        return base_Value * Mathf.Pow((level + 1), value);
    }
    public static void Set_Popup_Position(RectTransform popupRect, RectTransform targetRect)
    {
        Vector3[] corners = new Vector3[4];
        targetRect.GetWorldCorners(corners);

        Vector3 center = (corners[0] + corners[2]) * 0.5f;

        float screenCenterX = Screen.width * 0.5f;
        float screenCenterY = Screen.height * 0.5f;

        bool isLeft = center.x < screenCenterX;
        bool isTop = center.y > screenCenterY;

        if (isLeft && isTop)
        {
            popupRect.pivot = new Vector2(0f, 1f); // 좌상단 모서리
        }
        else if (!isLeft && isTop)
        {
            popupRect.pivot = new Vector2(1f, 1f); // 우상단 모서리
        }
        else if (isLeft && !isTop)
        {
            popupRect.pivot = new Vector2(0f, 0f); // 좌하단 모서리
        }
        else
        {
            popupRect.pivot = new Vector2(1f, 0f); // 우하단 모서리
        }

        popupRect.position = center;
    }
}

[System.Serializable]
public class Round_Data 
{
    public int magma_Count;
    public int green_Count;
    public int blue_Count;

    public Pool_ID boss_ID;
}

public static class Direction8
{
    public static Vector2 ToVector2(DIRECTION direction)
    {
        return direction switch
        {
            DIRECTION.Left => Vector2.left,
            DIRECTION.Right => Vector2.right,
            DIRECTION.Up => Vector2.up,
            DIRECTION.Down => Vector2.down,
            DIRECTION.LeftUp => new Vector2(-1f, 1f).normalized,
            DIRECTION.LeftDown => new Vector2(-1f, -1f).normalized,
            DIRECTION.RightUp => new Vector2(1f, 1f).normalized,
            DIRECTION.RightDown => new Vector2(1f, -1f).normalized,
            _ => Vector2.zero
        };
    }
}