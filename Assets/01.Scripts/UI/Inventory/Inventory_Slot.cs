public class Inventory_Slot
{
    public Item_Scriptable item;
    public int quantity;
    public bool IsEmpty => item == null;

    public Inventory_Slot(Item_Scriptable item = null, int quantity = 0)
    {
        this.item = item;
        this.quantity = quantity;
    }
}
public class Equipped_Slot
{
    public Item_Scriptable item;
    public int quantity;
    public bool IsEmpty => item == null;

    public Equipped_Slot(Item_Scriptable item = null, int quantity = 0)
    {
        this.item = item;
        this.quantity = quantity;
    }
}