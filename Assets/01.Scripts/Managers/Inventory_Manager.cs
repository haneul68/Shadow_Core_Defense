using System.Collections.Generic;
using UnityEngine;

public class Inventory_Manager
{
    //public readonly int inv_Default_Size = 11;

    public Stack<GameObject> Acrtion_Panal_Holder = new Stack<GameObject>();

    public readonly Inventory_Data inventory_Data = new Inventory_Data();
    public Inventory_Logic inventory_Logic;

    public void Init()
    {
        inventory_Data.Init();
        inventory_Logic = new Inventory_Logic(inventory_Data);
    }
}
