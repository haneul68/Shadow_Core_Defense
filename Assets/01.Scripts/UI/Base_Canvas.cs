using UnityEngine;

public class Base_Canvas : MonoBehaviour
{
    [Header("Canvas Parents (0: Laver_01, 1: Laver_02, 2: Laver_03)")]
    [SerializeField] private Transform[] layers;
    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Get_Setting_UI();
        }
    }
    public void Set_Layer(UI_Base ui, Canvas_Layer layer)
    {
        if (ui == null)
        {
            Debug.LogWarning("SetParent: ui is null");
            return;
        }

        int index = (int)layer;

        if (layers == null || layers.Length <= index || layers[index] == null)
        {
            Debug.LogWarning($"SetParent 실패: index {index} 없음");
            return;
        }

        ui.gameObject.transform.SetParent(layers[index], false);
    }

    public void Get_UI(Pool_ID pool_ID) 
    {
        Base_Manager.UI_Mng.Get_UI(pool_ID, Canvas_Layer.Laver_03);
    }

    public void Get_Text_Pop_Up(string temp, Color color)
    {
        UI_Text_Popup pop_Up = null;
        Base_Manager.Pool_Mng.Pooling_OBJ(Pool_ID.UI_Text_Popup).Get(obj =>
        {
            pop_Up = obj.GetComponent<UI_Text_Popup>();
            pop_Up.Init(temp, color);
            pop_Up.transform.SetParent(this.transform, false);
        });
    }
    public virtual bool Get_Setting_UI()
    {
        if (Base_Manager.UI_Mng == null)
        {
            Debug.Log("Base_Manager.UI_Mng == null");
            return true;
        }

        UI_Base ui = null;

        if (Base_Manager.UI_Mng.Is_UI_Open(Pool_ID.UI_Setting, out ui))
        {
            ui.GetComponent<UI_Setting>().Close_UI();
            return true;
        }

        return false;
    }
}
