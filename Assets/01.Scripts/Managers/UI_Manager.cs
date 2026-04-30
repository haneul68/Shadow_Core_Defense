using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager
{
    public Stack<UI_Base> ui_Holder = new Stack<UI_Base>();
    private Base_Canvas base_Canvas;

    public void Init(Base_Canvas _base_Canvas)
    {
        if (_base_Canvas == null) return;

        base_Canvas = _base_Canvas;
    }

    public UI_Base Get_UI(Pool_ID pool_ID, Canvas_Layer layer_Index)
    {
        string path = pool_ID.ToString();

        if (string.IsNullOrEmpty(path)) return null;

        if (ui_Holder.Count > 0 && ui_Holder.Peek().name == path) return null;

        UI_Base ui;

        GameObject go = Base_Manager.Pool_Mng.Pooling_OBJ(pool_ID).Get();

        ui = go.GetComponent<UI_Base>();

        base_Canvas.Set_Layer(ui, layer_Index);

        ui.transform.SetAsLastSibling();
        ui_Holder.Push(ui);

        return ui;
    }

    public void Close_Top_UI()
    {
        if (ui_Holder.Count == 0) return;

        UI_Base ui = ui_Holder.Pop();
        if (ui != null)
        {
            Base_Manager.Pool_Mng.pool_Dictionary[ui.Pool_ID].Return(ui.gameObject);
        }
    }

    public void Close_All_UI()
    {
        if (ui_Holder.Count == 0) return;

        Stack<UI_Base> temp_Stack = new Stack<UI_Base>();

        while (ui_Holder.Count > 0)
        {
            UI_Base ui = ui_Holder.Pop();
            if (ui != null)
            {
                Base_Manager.Pool_Mng.pool_Dictionary[ui.Pool_ID].Return(ui.gameObject);
                temp_Stack.Push(ui);
            }
        }

    }
    public bool Is_UI_Open(Pool_ID pool_ID, out UI_Base currnet_UI)
    {
        foreach (UI_Base ui in ui_Holder)
        {
            if (ui == null) continue;

            if (ui.Pool_ID == pool_ID && ui.gameObject.activeSelf) 
            {
                currnet_UI = ui;
                return true;
            }
        }
        currnet_UI = null;
        return false;
    }
}
