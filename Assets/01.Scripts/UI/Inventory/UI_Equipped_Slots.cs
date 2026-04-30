using UnityEngine;

public class UI_Equipped_Slots : MonoBehaviour
{
    [SerializeField] private Inventory_Item_Slot[] equipped_Slots_UI;

    [SerializeField] private Transform des_Content;

    private void OnEnable()
    {
        Draw_Equipped_Slots();

        if (Base_Manager.Inventory_Mng.inventory_Logic != null)
        {
            Base_Manager.Inventory_Mng.inventory_Logic.On_Equip_Changed += On_Equip_Changed;
            Base_Manager.Inventory_Mng.inventory_Logic.On_Item_Changed += On_Item_Changed;
        }
    }

    private void OnDisable()
    {
        if (Base_Manager.Inventory_Mng.inventory_Logic != null)
        {
            Base_Manager.Inventory_Mng.inventory_Logic.On_Equip_Changed -= On_Equip_Changed;
            Base_Manager.Inventory_Mng.inventory_Logic.On_Item_Changed -= On_Item_Changed;
        }
    }

    private void Draw_Equipped_Slots()
    {
        var equippedSlots = Base_Manager.Inventory_Mng.inventory_Data.Equipped_Slots;

        for (int i = 0; i < equipped_Slots_UI.Length; i++)
        {
            var uiSlot = equipped_Slots_UI[i];

            if (uiSlot == null) continue;

            var dataSlot = equippedSlots[i];

            if (!dataSlot.IsEmpty)
            {
                int quantity = Base_Manager.Inventory_Mng.inventory_Data.Get_Quantity(dataSlot.item);

                uiSlot.Init(
                    dataSlot.item,
                    quantity,
                    des_Content,
                    i,
                    Item_Slot_Type.Equipped
                );
            }
            else
            {
                uiSlot.Init(
                    null,
                    0,
                    des_Content,
                    i,
                    Item_Slot_Type.Equipped
                );
            }
        }
    }

    private void On_Equip_Changed(Item_Scriptable item)
    {
        Draw_Equipped_Slots();
    }

    private void On_Item_Changed(Item_Scriptable item)
    {
        Draw_Equipped_Slots();
    }
}