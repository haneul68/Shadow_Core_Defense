using UnityEngine;

public interface IDirection_Provider
{
    Vector2 Last_Facing_Dir { get; set; }
    Vector2 Get_Direction();
}
