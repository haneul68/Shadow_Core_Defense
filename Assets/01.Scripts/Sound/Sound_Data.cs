using System;
using UnityEngine;

[Serializable]
public class BGM_Data
{
    public BGM_Type bgm_Type;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1f;
    public bool loop = true;
}

[Serializable]
public class SFX_Data
{
    public SFX_Type sfx_Type;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1f;
}