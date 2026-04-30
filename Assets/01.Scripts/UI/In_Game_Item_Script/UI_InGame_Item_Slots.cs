using UnityEngine;

public class UI_InGame_Item_Slots : MonoBehaviour
{
    public static UI_InGame_Item_Slots Instance;

    [SerializeField] private InGame_Item_Slot[] slots;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        Draw_Slots();

        if (Base_Manager.Inventory_Mng.inventory_Logic != null)
        {
            Base_Manager.Inventory_Mng.inventory_Logic.On_Equip_Changed += OnEquipChanged;
            Base_Manager.Inventory_Mng.inventory_Logic.On_Item_Changed += OnItemChanged;
        }
    }

    private void OnDisable()
    {
        if (Base_Manager.Inventory_Mng.inventory_Logic != null)
        {
            Base_Manager.Inventory_Mng.inventory_Logic.On_Equip_Changed -= OnEquipChanged;
            Base_Manager.Inventory_Mng.inventory_Logic.On_Item_Changed -= OnItemChanged;
        }
    }

    private void Update()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null)
                slots[i].Tick_Cooldown(Time.deltaTime);
        }
    }

    public void Draw_Slots()
    {
        var equippedSlots = Base_Manager.Inventory_Mng.inventory_Data.Equipped_Slots;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null) continue;

            Item_Scriptable item = null;
            int quantity = 0;

            if (i < equippedSlots.Count && !equippedSlots[i].IsEmpty)
            {
                item = equippedSlots[i].item;
                quantity = Base_Manager.Inventory_Mng.inventory_Data.Get_Quantity(item);
            }

            slots[i].Init(item, quantity);
        }
    }

    private void OnEquipChanged(Item_Scriptable item)
    {
        Draw_Slots();
    }

    private void OnItemChanged(Item_Scriptable item)
    {
        Draw_Slots();
    }

    public bool Try_Use_Slot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Length) return false;

        var slot = slots[slotIndex];
        if (slot == null) return false;

        var item = slot.Current_Item;
        if (item == null) return false;

        // 쿨타임 체크
        if (slot.Is_On_Cooldown())
        {
            Debug.Log("쿨타임 중");
            In_Game_Canvas.Instance.Get_Text_Pop_Up($"아직 사용할 수 없습니다", Color.red);
            return false;
        }

        // 인벤토리에서 사용
        bool success = Base_Manager.Inventory_Mng.inventory_Logic.Use_Item(item, 1);
        if (!success) return false;

        slot.Start_Cooldown();

        return true;
    }
}