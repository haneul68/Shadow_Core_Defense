using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.UI;

public class UI_Setting : UI_Base
{
    [Header("Volume Slider")]
    [SerializeField] 
    private Slider bgm_Slider;
    [SerializeField] 
    private Slider sfx_Slider;
    [SerializeField]
    private Button give_Up_Button;

    [Space(20)]
    [Header("Camera_Shake")]
    [SerializeField] 
    private Toggle shake_Off_Toggle;
    [SerializeField]
    private Toggle shake_Weak_Toggle;
    [SerializeField] 
    private Toggle shake_Strong_Toggle;

    private void OnEnable()
    {
        Init_Slider();
        Init_Shake_Toggle();
    }

    private void Init_Slider()
    {
        if (Base_Manager.Sound_Mng == null)
        {
            Debug.Log("Base_Manager.Sound_Mng == null");
            return;
        }

        if (bgm_Slider != null)
        {
            bgm_Slider.onValueChanged.RemoveListener(On_BGM_Volume_Changed);
            bgm_Slider.value = Base_Manager.Sound_Mng.BGM_Volume;
            bgm_Slider.onValueChanged.AddListener(On_BGM_Volume_Changed);
        }

        if (sfx_Slider != null)
        {
            sfx_Slider.onValueChanged.RemoveListener(On_SFX_Volume_Changed);
            sfx_Slider.value = Base_Manager.Sound_Mng.SFX_Volume;
            sfx_Slider.onValueChanged.AddListener(On_SFX_Volume_Changed);
        }
    }
    private void On_BGM_Volume_Changed(float value)
    {
        if (Base_Manager.Sound_Mng == null)
        {
            Debug.Log("Base_Manager.Sound_Mng == null");
            return;
        }

        Base_Manager.Sound_Mng.Set_BGM_Volume(value);
    }

    private void On_SFX_Volume_Changed(float value)
    {
        if (Base_Manager.Sound_Mng == null)
        {
            Debug.Log("Base_Manager.Sound_Mng == null");
            return;
        }

        Base_Manager.Sound_Mng.Set_SFX_Volume(value);
    }

    public void Give_Up_Game()
    {
        Close_UI();

        InGame_State_Manager.State_Change(InGame_State.DEATH);
    }

    public void On_Give_Up_Button(bool on) 
    {
        give_Up_Button.gameObject.SetActive(on);
    }

    private void Init_Shake_Toggle()
    {
        if (Camera_Shake_Manager.Instance == null)
        {
            Debug.Log("Camera_Shake_Manager.Instance == null");
            return;
        }

        shake_Off_Toggle.onValueChanged.RemoveAllListeners();
        shake_Weak_Toggle.onValueChanged.RemoveAllListeners();
        shake_Strong_Toggle.onValueChanged.RemoveAllListeners();

        Camera_Shake_Level level = Camera_Shake_Manager.Instance.Shake_Level;

        shake_Off_Toggle.isOn = level == Camera_Shake_Level.Off;
        shake_Weak_Toggle.isOn = level == Camera_Shake_Level.Weak;
        shake_Strong_Toggle.isOn = level == Camera_Shake_Level.Strong;

        shake_Off_Toggle.onValueChanged.AddListener(On_Shake_Off_Toggle);
        shake_Weak_Toggle.onValueChanged.AddListener(On_Shake_Weak_Toggle);
        shake_Strong_Toggle.onValueChanged.AddListener(On_Shake_Strong_Toggle);
    }
    private void On_Shake_Off_Toggle(bool is_On)
    {
        if (!is_On) return;
        Set_Shake_Level(Camera_Shake_Level.Off);
    }

    private void On_Shake_Weak_Toggle(bool is_On)
    {
        if (!is_On) return;
        Set_Shake_Level(Camera_Shake_Level.Weak);
    }

    private void On_Shake_Strong_Toggle(bool is_On)
    {
        if (!is_On) return;
        Set_Shake_Level(Camera_Shake_Level.Strong);
    }

    private void Set_Shake_Level(Camera_Shake_Level level)
    {
        if (Camera_Shake_Manager.Instance == null)
        {
            Debug.Log("Camera_Shake_Manager.Instance == null");
            return;
        }

        Camera_Shake_Manager.Instance.Set_Shake_Level(level);

        shake_Off_Toggle.isOn = level == Camera_Shake_Level.Off;
        shake_Weak_Toggle.isOn = level == Camera_Shake_Level.Weak;
        shake_Strong_Toggle.isOn = level == Camera_Shake_Level.Strong;
    }
    public override void Close_UI()
    {
        Time.timeScale = 1f;
        On_Give_Up_Button(false);
        base.Close_UI();
    }
}