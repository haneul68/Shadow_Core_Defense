using UnityEngine;

[CreateAssetMenu(fileName = "Mana_Potion", menuName = "Item/Consumable/Mana Potion", order = int.MaxValue)]
public class Mana_Potion : Item_Scriptable
{
    public override void Use(Character target)
    {
        if (target == null) return;

        Mana_Manager mana = target.GetComponent<Mana_Manager>();

        if (mana == null)
        {
            Debug.Log("mana == null");
            return;
        }

        Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Use_Potion);

        float percent = item_Value / 100f;
        float recover_Amount = (float)mana.Max * percent;

        mana.Recover_Mana(recover_Amount);
    }
}
