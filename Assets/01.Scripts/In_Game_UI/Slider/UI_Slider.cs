using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Slider_Type 
{
    Health, 
    Mana,
    Stamina
}

public class UI_Slider : UI_Base
{
    [Header("REF")]
    [SerializeField] private GameObject stat_Manager;
    private IStat_Provider stat_Provider;

    [Space(20)]
    [Header("Slider_BAR_Fill")]
    [SerializeField]
    private Image front_Fill;
    [SerializeField]
    private Image back_Fill;
    [SerializeField]
    private TextMeshProUGUI value_Text;

    [Space(20)]
    [Header("Type")]
    [SerializeField]
    private Slider_Type slider_Type;

    [Space(20)]
    [Header("Speed")]
    private float smooth_Speed = 3.0f;

    private Coroutine fill_Coroutine;

    [SerializeField]
    private Transform target= null;   
    private Camera main_Cam;

    private void Awake()
    {
        if (stat_Manager != null)
        {
            stat_Provider = Get_Provider_By_Type(stat_Manager);

            Set_IStat_Provider();
        }
    }
    public void Init(IStat_Provider stat_Provider, Transform target)
    {
        this.stat_Provider = stat_Provider;
        this.target = target;
        main_Cam = Camera.main;

        transform.localScale = Vector3.one;

        Set_IStat_Provider();
    }

    private void OnDisable()
    {
        if (stat_Provider != null)
        {
            stat_Provider.On_Value_Changed -= Update_Stat;
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 worldPos = target.position;
        Vector3 screenPos = main_Cam.WorldToScreenPoint(worldPos);

        transform.position = screenPos;
    }

    private IStat_Provider Get_Provider_By_Type(GameObject obj)
    {
        switch (slider_Type)
        {
            case Slider_Type.Health:
                return obj.GetComponent<Health_Manager>();

            case Slider_Type.Mana:
                return obj.GetComponent<Mana_Manager>();  

            case Slider_Type.Stamina:
                return obj.GetComponent<Stamina_Manager>(); 

            default:
                return null;
        }
    }

    private void Set_IStat_Provider() 
    {
        if (stat_Provider == null)
        {
            Debug.Log("stat_Provider == null");
            return;
        }
        stat_Provider.On_Value_Changed += Update_Stat;
    }

    private void Update_Stat(double current, double max)
    {
        float target_Fill = (float)(current / max);
        front_Fill.fillAmount = target_Fill;

        if (value_Text != null)
        {
            value_Text.text = $"{(int)current}/{(int)max}";
        }

        if (fill_Coroutine != null)
        {
            StopCoroutine(fill_Coroutine);
            fill_Coroutine = null;
        }

        fill_Coroutine = StartCoroutine(Smooth_Fill_Coroutine(target_Fill));
    }

    private IEnumerator Smooth_Fill_Coroutine(float target) 
    {
        while (Mathf.Abs(back_Fill.fillAmount - target) > 0.001f) 
        {
            back_Fill.fillAmount = Mathf.Lerp(back_Fill.fillAmount, target, Time.deltaTime * smooth_Speed);

            yield return null;
        }

        back_Fill.fillAmount = target;
    }

    public void Set_Player_Stat_Manager(GameObject player) 
    {
        if (player == null) 
        {
            Debug.Log("player == null");
            return;
        }

        if (stat_Provider != null)
        {
            stat_Provider.On_Value_Changed -= Update_Stat;
        }

        stat_Manager = player;

        stat_Provider = Get_Provider_By_Type(stat_Manager);

        Set_IStat_Provider();

        if (stat_Provider != null)
        {
            Update_Stat(stat_Provider.Current, stat_Provider.Max);
        }
    }
}
