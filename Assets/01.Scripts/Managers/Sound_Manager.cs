using UnityEngine;

public class Sound_Manager
{
    private Sound_Library sound_Library;

    private AudioSource bgm_Source;
    private AudioSource sfx_Source;

    private float bgm_Volume = 1f;
    private float sfx_Volume = 1f;

    public float BGM_Volume => bgm_Volume;
    public float SFX_Volume => sfx_Volume;


    private BGM_Type current_BGM_Type = BGM_Type.None;

    public void Init()
    {
        sound_Library = Resources.Load<Sound_Library>("Scriptable/Sound/Sound_Library");

        if (sound_Library == null)
        {
            Debug.Log("sound_Library == null");
        }

        GameObject sound_Obj = new GameObject("Sound_Manager");
        Object.DontDestroyOnLoad(sound_Obj);

        bgm_Source = sound_Obj.AddComponent<AudioSource>();
        sfx_Source = sound_Obj.AddComponent<AudioSource>();

        bgm_Source.playOnAwake = false;
        bgm_Source.loop = true;

        sfx_Source.playOnAwake = false;
        sfx_Source.loop = false;
    }

    public void Play_BGM(BGM_Type type)
    {
        if (sound_Library == null)
        {
            Debug.Log("sound_Library == null");
            return;
        }

        if (bgm_Source == null)
        {
            Debug.Log("bgm_Source == null");
            return;
        }

        if (current_BGM_Type == type) return;

        BGM_Data data = sound_Library.Get_BGM(type);

        if (data == null)
        {
            Debug.Log($"BGM_Data 없음: {type}");
            return;
        }

        if (data.clip == null)
        {
            Debug.Log($"BGM Clip 없음: {type}");
            return;
        }

        current_BGM_Type = type;

        bgm_Source.clip = data.clip;
        bgm_Source.volume = data.volume * bgm_Volume;
        bgm_Source.loop = data.loop;
        bgm_Source.Play();
    }

    public void Stop_BGM()
    {
        if (bgm_Source == null)
        {
            Debug.Log("bgm_Source == null");
            return;
        }

        bgm_Source.Stop();
        current_BGM_Type = BGM_Type.None;
    }

    public void Play_SFX(SFX_Type type)
    {
        if (sound_Library == null)
        {
            Debug.Log("sound_Library == null");
            return;
        }

        if (sfx_Source == null)
        {
            Debug.Log("sfx_Source == null");
            return;
        }

        SFX_Data data = sound_Library.Get_SFX(type);

        if (data == null)
        {
            Debug.Log($"SFX_Data 없음: {type}");
            return;
        }

        if (data.clip == null)
        {
            Debug.Log($"SFX Clip 없음: {type}");
            return;
        }

        sfx_Source.PlayOneShot(data.clip, data.volume * sfx_Volume);
    }
    public void Set_BGM_Volume(float value)
    {
        bgm_Volume = Mathf.Clamp01(value);

        if (bgm_Source == null)
        {
            Debug.Log("bgm_Source == null");
            return;
        }

        bgm_Source.volume = bgm_Volume;
    }

    public void Set_SFX_Volume(float value)
    {
        sfx_Volume = Mathf.Clamp01(value);
    }
}