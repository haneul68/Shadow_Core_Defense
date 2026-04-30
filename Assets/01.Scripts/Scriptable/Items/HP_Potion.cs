using UnityEngine;

[CreateAssetMenu(fileName = "HP_Potion", menuName = "Item/Consumable/HP Potion", order = int.MaxValue)]
public class HP_Potion : Item_Scriptable
{
    public override void Use(Character target)
    {
        if (target == null) return;

        Health_Manager health = target.GetComponent<Health_Manager>();

        if (health == null)
        {
            Debug.Log("health == null");
            return;
        }

        Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Use_Potion);

        double percent = item_Value / 100.0;
        double heal_Amount = health.Max * percent;

        health.Heal(heal_Amount, true);
    }
}
