using DG.Tweening;
using TMPro;
using UnityEngine;

public class Damage_Text : UI_Base
{
    [SerializeField] private TextMeshProUGUI damage_Text;
    [SerializeField] private CanvasGroup canvas_Group;

    [SerializeField] private float follow_Update_Interval = 0.03f;

    private RectTransform rect;
    private Sequence seq;

    private Transform target;
    private RectTransform canvasRect;
    private Canvas canvas;
    private Camera cam;

    private float random_X;
    private float stack_Y_Offset;
    private float rise_Offset;

    private float follow_Timer;
    private bool is_Playing;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void Init(string text, Transform target, Transform canvas_Transform, Color color, float y_Offset = 0f)
    {
        if (damage_Text == null) return;
        if (canvas_Group == null) return;
        if (canvas_Transform == null) return;
        if (rect == null) return;
        if (target == null) return;

        canvas = canvas_Transform.GetComponentInParent<Canvas>();
        canvasRect = canvas_Transform as RectTransform;

        if (cam == null)
            cam = Camera.main;

        if (canvas == null) return;
        if (canvasRect == null) return;
        if (cam == null) return;

        if (seq != null && seq.IsActive())
        {
            seq.Kill();
            seq = null;
        }

        this.target = target;
        stack_Y_Offset = y_Offset;
        rise_Offset = 0f;
        random_X = Random.Range(-12f, 12f);
        follow_Timer = 0f;
        is_Playing = true;

        transform.SetParent(canvas_Transform, false);

        rect.localScale = Vector3.one;
        rect.localRotation = Quaternion.identity;

        damage_Text.text = text;
        damage_Text.color = color;
        canvas_Group.alpha = 1f;

        Update_Position(true);
        Play_Animation();
    }

    private void LateUpdate()
    {
        if (!is_Playing) return;
        if (target == null) return;

        follow_Timer += Time.deltaTime;
        if (follow_Timer < follow_Update_Interval) return;

        follow_Timer = 0f;
        Update_Position(false);
    }

    private void Update_Position(bool forceUpdate)
    {
        if (target == null || canvasRect == null || cam == null) return;

        Vector3 screenPos = cam.WorldToScreenPoint(target.position);

        if (screenPos.z < 0f)
        {
            if (!forceUpdate)
                return;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out Vector2 localPos
        );

        rect.anchoredPosition = localPos + new Vector2(random_X, stack_Y_Offset + rise_Offset);
    }

    private void Play_Animation()
    {
        if (seq != null && seq.IsActive())
        {
            seq.Kill();
            seq = null;
        }

        seq = DOTween.Sequence();

        seq.Append(rect.DOScale(1.2f, 0.08f));
        seq.Append(rect.DOScale(1f, 0.08f));

        seq.Join( DOTween.To(() => rise_Offset, x => rise_Offset = x, 80f, 0.7f).SetEase(Ease.OutCubic));

        seq.Join(canvas_Group.DOFade(0f, 0.7f).SetEase(Ease.InQuad));

        seq.OnComplete(() =>
        {
            is_Playing = false;
            seq = null;
            Base_Manager.Pool_Mng.pool_Dictionary[Pool_ID.Damage_Text].Return(gameObject);
        });
    }

    private void OnDisable()
    {
        if (seq != null && seq.IsActive())
        {
            seq.Kill();
            seq = null;
        }

        is_Playing = false;
        target = null;
        stack_Y_Offset = 0f;
        rise_Offset = 0f;
        random_X = 0f;
        follow_Timer = 0f;

        if (rect != null)
        {
            rect.localScale = Vector3.one;
            rect.localRotation = Quaternion.identity;
            rect.anchoredPosition = Vector2.zero;
        }

        if (canvas_Group != null)
        {
            canvas_Group.alpha = 1f;
        }
    }
}