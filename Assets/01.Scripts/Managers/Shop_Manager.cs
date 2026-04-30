using System;
using System.Collections.Generic;
using UnityEngine;

public class Shop_Manager : MonoBehaviour
{
    public static Shop_Manager Instance { get; private set; }

    public Stack<GameObject> Shop_Acrtion_Panal_Holder = new Stack<GameObject>();

    public event Action<Item_Scriptable> On_Shop_Item_Changed;
    private void Awake()
    {
        Instance = this;
    }
    public bool Try_Buy_Item(string item_ID, int amount)
    {
        if (!Shop_Data_Manager.Instance.Shop_Item_Datas.ContainsKey(item_ID))
        {
            Debug.Log($"아이템 없음");
            return false;
        }

        var item = Shop_Data_Manager.Instance.Shop_Item_Datas[item_ID];

        if (!Base_Manager.Inventory_Mng.inventory_Data.Can_Add_Item(item.data, amount))
        {

            Lobby_Canvas.Instance.Get_Text_Pop_Up($"인벤토리 공간이 부족합니다", Color.red);
            return false;
        }

        if (item.total_Amount < amount)
        {

            Lobby_Canvas.Instance.Get_Text_Pop_Up($"재고가 부족합니다", Color.red);
            return false;
        }

        if (!Base_Manager.Data_Mng.Spend_Gold(item.price * amount))
        {
            Lobby_Canvas.Instance.Get_Text_Pop_Up($"재화가 부족합니다", Color.red);
            return false;
        }

        if (!Base_Manager.Inventory_Mng.inventory_Logic.Get_Item(item.data, amount))
        {
            //ebug.Log($"구매 실패");
            return false;
        }

        item.total_Amount -= amount;
        On_Shop_Item_Changed?.Invoke(item.data);
        return true;
    }
}
