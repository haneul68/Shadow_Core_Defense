using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound_Library", menuName = "Sound/Sound_Library", order = int.MaxValue)]
public class Sound_Library : ScriptableObject
{
    [Header("BGM")]
    public List<BGM_Data> bgm_List = new List<BGM_Data>();

    [Space(20)]
    [Header("SFX")]
    public List<SFX_Data> sfx_List = new List<SFX_Data>();

    public BGM_Data Get_BGM(BGM_Type type)
    {
        return bgm_List.Find(x => x.bgm_Type == type);
    }

    public SFX_Data Get_SFX(SFX_Type type)
    {
        return sfx_List.Find(x => x.sfx_Type == type);
    }
}