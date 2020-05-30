using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeInOut : MonoBehaviour
{
    public enum TransitionType { In, None, Out }
    private TransitionType m_Type = TransitionType.None;

    public bool m_PlayOnAwake;
    public float m_Time = 0.5f;
    public AnimationCurve m_Curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

    private float m_StartTime;
    private CanvasGroup m_CanvasGroup;

    private void Awake()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        m_Type = TransitionType.None;
        if (m_PlayOnAwake)
            Hide();
    }

    public void Hide()
    {
        if (m_Type != TransitionType.None)
            return;

        m_CanvasGroup.alpha = 1.0f;
        m_Type = TransitionType.Out;
        m_StartTime = Time.time;
    }

    public void Show()
    {
        if (m_Type != TransitionType.None)
            return;

        gameObject.SetActive(true);
        m_CanvasGroup.alpha = 0.0f;
        m_Type = TransitionType.In;
        m_StartTime = Time.time;
    }

    private void Update()
    {
        if (m_Type == TransitionType.None)
            return;

        float rate = (Time.time - m_StartTime) / m_Time;

        if (m_Type == TransitionType.In)
        {
            m_CanvasGroup.alpha = Mathf.Lerp(0.0f, 1.0f, m_Curve.Evaluate(rate));
            if (rate >= 1.0f)
            {
                m_Type = TransitionType.None;
            }
        }

        if (m_Type == TransitionType.Out)
        {
            m_CanvasGroup.alpha = Mathf.Lerp(1.0f, 0.0f, m_Curve.Evaluate(rate));
            if (rate >= 1.0f)
            {
                m_Type = TransitionType.None;
                gameObject.SetActive(false);
            }
        }
    }
}
