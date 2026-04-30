using System.Collections;
using UnityEngine;

public class Hit_Box_Spawn_Point : MonoBehaviour
{
    private Coroutine coroutine;

    private bool is_Returned;
    private void OnEnable()
    {
        is_Returned = false;
    }

    private void OnDisable()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        transform.localScale = Vector3.zero;
    }
    public void Init(Transform target, float size, float duration, bool re_Mind = true) 
    {
        if (coroutine != null) 
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        transform.localScale = Vector3.zero;
        coroutine = StartCoroutine(Spawn_Hit_Box_Spawn_Point(target, size, duration, re_Mind));
    }

    private IEnumerator Spawn_Hit_Box_Spawn_Point(Transform target, float size, float duration, bool re_Mind = true)
    {
        float time = 0f;

        Vector2 start_Scale = Vector2.zero;

        float re_Mind_Scale = re_Mind ? 0.22f : 1f;

        Vector2 end_Scale = Vector2.one * size * re_Mind_Scale;

        target.localScale = start_Scale;

        while (time < duration)
        {
            time += Time.deltaTime;

            float t = time / duration;
            t = Mathf.SmoothStep(0f, 1f, t);

            target.localScale = Vector2.Lerp(start_Scale, end_Scale, t);

            yield return null;
        }
        target.localScale = end_Scale;

        Force_Return();
    }
    public void Force_Return()
    {
        if (is_Returned) return;

        is_Returned = true;

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        Base_Manager.Pool_Mng.pool_Dictionary[Pool_ID.Hit_Box_Spawn_Point].Return(gameObject);
    }
}
