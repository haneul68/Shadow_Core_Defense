using System.Collections;
using UnityEngine;

public class Warrior_Skill_Tornado : Player_Skill_Base
{
    [Header("Tornado")]
    [SerializeField] 
    private Pool_ID tornado_Pool_Id;
    [SerializeField] 
    private float duration = 5f;
    [SerializeField] 
    private float damage_Per_Second_Percent = 60f;
    [SerializeField] 
    private float move_Speed = 4f;
    [SerializeField] 
    private float search_Radius = 8f;
    [SerializeField]
    private float damage_Tick_Interval = 0.4f;

    [Space(20)]
    [Header("Target")]
    [SerializeField] private LayerMask enemy_Layer;

    [Space(20)]
    [Header("Spawn")]
    [SerializeField] private Vector3 spawn_Offset = Vector3.zero;

    protected override object[] Get_Description_Values()
    {
        return new object[]
        {
            duration.ToString("F1"),
            damage_Tick_Interval.ToString("F1"),
            damage_Per_Second_Percent.ToString("F0")
        };
    }

    protected override IEnumerator Execute(Player_Skill_Manager owner)
    {
        if (owner == null)
            yield break;

        Character character = owner.GetComponent<Character>();
        if (character == null)
        {
            Debug.Log("character == null");
            yield break;
        }

        Base_Manager.Pool_Mng.Pooling_OBJ(tornado_Pool_Id).Get(obj =>
        {
            obj.transform.position = owner.transform.position + spawn_Offset;
            obj.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

            Skill_Tornado tornado = obj.GetComponent<Skill_Tornado>();
            if (tornado == null)
            {
                Debug.Log("Skill_Tornado == null");
                return;
            }

            Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Warrior_Skill_T);

            tornado.Init(
                owner,
                character.Final_ATK,
                damage_Per_Second_Percent,
                duration,
                move_Speed,
                search_Radius,
                enemy_Layer,
                damage_Tick_Interval,
                tornado_Pool_Id
            );

            owner.Register_Spawned_Object(obj, tornado_Pool_Id);
        });

        yield break;
    }
}