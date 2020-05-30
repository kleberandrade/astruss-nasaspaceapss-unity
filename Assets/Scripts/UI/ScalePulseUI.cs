using UnityEngine;

public class ScalePulseUI : MonoBehaviour
{
    public enum ScalePulseState { None, Running }
    private ScalePulseState m_State = ScalePulseState.None;

    public float m_Size = 0.5f;
    public float m_Time = 0.3f;
    public AnimationCurve m_Curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

    private Vector3 m_StartScale;
    private float m_StartTime;

    private void Start()
    {
        m_StartScale = transform.localScale;
    }

    private void Update()
    {
        if (m_State == ScalePulseState.None)
            return;

        float time = (Time.time - m_StartTime) / m_Time;

        transform.localScale = m_StartScale + Vector3.one * m_Curve.Evaluate(time) * m_Size;

        if (time >= 1.0f)
            m_State = ScalePulseState.None;
    }

    public void Pulse()
    {
        if (m_State == ScalePulseState.Running)
            return;

        m_State = ScalePulseState.Running;
        m_StartTime = Time.time;
    }
}
