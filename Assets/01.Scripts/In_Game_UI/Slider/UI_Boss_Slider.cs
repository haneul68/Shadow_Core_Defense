using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Boss_Slider : UI_Base
{
    [Header("Fill (Front)")]
    [SerializeField] private Image[] front_Fills; 

    [Header("Fill (Back)")]
    [SerializeField] private Image[] back_Fills; 

    [Header("Speed")]
    [SerializeField] private float smooth_Speed = 3f;

    [SerializeField]
    private TextMeshProUGUI hp_Text;

    private IStat_Provider stat;

    private Coroutine[] back_Coroutines = new Coroutine[3];

    private double hp_1;
    private double hp_2;
    private double hp_3;

    public void Init(IStat_Provider stat)
    {
        this.stat = stat;

        double max = stat.Max;

        hp_1 = max * 0.3;
        hp_2 = max * 0.3;
        hp_3 = max * 0.4;

        transform.localScale = Vector3.one;

        stat.On_Value_Changed += Update_HP;

        Update_HP(stat.Current, stat.Max);
    }

    private void OnDisable()
    {
        if (stat != null)
            stat.On_Value_Changed -= Update_HP;
    }

    private void Update_HP(double current, double max)
    {
        double remain = current;

        float f1 = 0;
        float f2 = 0;
        float f3 = 0;

        if (remain > hp_2 + hp_3)
        {
            f1 = (float)((remain - (hp_2 + hp_3)) / hp_1);
            f2 = 1f;
            f3 = 1f;
        }
        else if (remain > hp_3)
        {
            f1 = 0f;
            f2 = (float)((remain - hp_3) / hp_2);
            f3 = 1f;
        }
        else
        {
            f1 = 0f;
            f2 = 0f;
            f3 = (float)(remain / hp_3);
        }

        f1 = Mathf.Clamp01(f1);
        f2 = Mathf.Clamp01(f2);
        f3 = Mathf.Clamp01(f3);

        Set_Fill(0, f1);
        Set_Fill(1, f2);
        Set_Fill(2, f3);

        if (hp_Text != null)
        {
            hp_Text.text = $"{(int)current}/{(int)max}";
        }
    }

    private void Set_Fill(int index, float value)
    {
        front_Fills[index].fillAmount = value;

        if (back_Coroutines[index] != null)
            StopCoroutine(back_Coroutines[index]);

        back_Coroutines[index] = StartCoroutine(Smooth_Back(index, value));
    }

    private IEnumerator Smooth_Back(int index, float target)
    {
        while (Mathf.Abs(back_Fills[index].fillAmount - target) > 0.001f)
        {
            back_Fills[index].fillAmount =
                Mathf.Lerp(back_Fills[index].fillAmount, target, Time.deltaTime * smooth_Speed);

            yield return null;
        }

        back_Fills[index].fillAmount = target;
    }
}