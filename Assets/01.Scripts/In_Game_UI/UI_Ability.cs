using System.Collections.Generic;
using UnityEngine;

public class UI_Ability : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private int slot_Count = 3;
    [SerializeField] private GameObject lock_Image;


    private List<GameObject> garbage_Slot = new List<GameObject>();

    private void OnEnable()
    {
        Init();
    }

    private void OnDisable()
    {
        foreach (GameObject slot in garbage_Slot) 
        {
            if (slot == null) continue;

            Base_Manager.Pool_Mng.pool_Dictionary[Pool_ID.Ability_Slot].Return(slot, obj => 
            {
                slot.transform.localScale = Vector3.one;
            });
        }

        garbage_Slot.Clear();
    }

    private void Init() 
    {
        Create_Slot();
        Set_Active_Lock_Obj(false);
    }

    private void Create_Slot()
    {
        List<Ability> abilities = Ability_Manager.Instance.Get_Random_Abilities(slot_Count);

        for (int i = 0; i < abilities.Count; i++)
        {
            Ability ability = abilities[i];

            Base_Manager.Pool_Mng.Pooling_OBJ(Pool_ID.Ability_Slot).Get(slot =>
            {
                slot.transform.SetParent(content, false);

                UI_Ability_Slot ability_Slot = slot.GetComponent<UI_Ability_Slot>();

                ability_Slot.Set_Ability(ability);

                ability_Slot.Init(this);

                garbage_Slot.Add(slot);
            });
        }
    }

    public void Set_Active_Lock_Obj(bool active) 
    {
        lock_Image.SetActive(active);
    }
}
