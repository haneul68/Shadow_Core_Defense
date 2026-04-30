using System.Collections.Generic;
using UnityEngine;

public class Shop_Data_Manager : MonoBehaviour
{
    public static Shop_Data_Manager Instance { get; private set; }

    [SerializeField]
    private List<Shop_Item_Data> shop_Item_List = new List<Shop_Item_Data>();

    private Dictionary<string, Shop_Item_Data> shop_Item_Datas = new Dictionary<string, Shop_Item_Data>();

    public IReadOnlyDictionary<string, Shop_Item_Data> Shop_Item_Datas => shop_Item_Datas;

    public bool Is_Interactable { get; set; }

    private void Awake()
    {
        Instance = this;

        Set_Item_Data();
    }

    private void Set_Item_Data()
    {
        shop_Item_Datas.Clear();

        foreach (var item in shop_Item_List)
        {
            var copy = new Shop_Item_Data
            {
                item_ID = item.item_ID,
                total_Amount = item.total_Amount,
                price = item.price,
                data = null
            };

            if (Base_Manager.Data_Mng.Get_Item_Data(item.item_ID, out Item_Scriptable item_Data))
            {
                copy.data = item_Data;
            }

            shop_Item_Datas[copy.item_ID] = copy;
        }
    }
}
