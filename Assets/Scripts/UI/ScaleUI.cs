using UnityEngine;

public class ScaleUI : MonoBehaviour, BehaviorUI
{
    public enum ScaleState { In, Idle, Out }
    private ScaleState m_State = ScaleState.Idle;

    [Header("Speed")]
    public bool m_AutoEnable = true;
    public float m_FirstDelay = 0.5f;
    public float m_EnableTime = 0.75f;
    public float m_DisableTime = 0.5f;
    public AnimationCurve m_Curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

    private float m_ElapsedTime;
    private Vector2 m_OriginalScale;
    private bool m_Completed;
    private RectTransform m_Transform;

    private void Awake()
    {
        m_Transform = GetComponent<RectTransform>();
        m_OriginalScale = m_Transform.localScale;
        m_Transform.localScale = Vector2.zero;
    }

    public void Enable()
    {
        m_ElapsedTime = 0.0f;
        m_State = ScaleState.In;
    }

    public void Disable()
    {
        m_ElapsedTime = 0.0f;
        m_State = ScaleState.Out;
    }

    private void OnEnable()
    {
        if (m_AutoEnable)
        {
            Invoke("Enable", m_FirstDelay);
        }
        else
        {
            m_Transform.localScale = Vector2.zero;
        }
    }

    private void Update()
    {
        if (m_State == ScaleState.Idle)
            return;

        if (m_State == ScaleState.In)
        {
            m_Transform.localScale = Lerp(Vector2.zero, m_OriginalScale, m_ElapsedTime / m_EnableTime, out m_Completed);
            if (m_Completed) m_State = ScaleState.Idle;
        }

        if (m_State == ScaleState.Out && !m_Transform.localScale.Equals(Vector2.zero))
        {
            m_Transform.localScale = Lerp(m_OriginalScale, Vector2.zero, m_ElapsedTime / m_DisableTime, out m_Completed);
            if (m_Completed) m_State = ScaleState.Idle;
        }

        m_ElapsedTime += Time.deltaTime;
    }

    private Vector2 Lerp(Vector2 from, Vector2 to, float time, out bool completed)
    {
        var scale = Vector2.LerpUnclamped(from, to, m_Curve.Evaluate(time));

        completed = time >= 1.0f;

        if (completed)
            scale = to;

        return scale;
    }
}