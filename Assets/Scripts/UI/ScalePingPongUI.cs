using UnityEngine;

public class ScalePingPongUI : MonoBehaviour
{
    public float m_Range = 0.05f;
    public float m_SmoothTime = 10.0f;
    private Vector3 m_Scale;

    private void Start()
    {
        m_Scale = transform.localScale;
    }

    private void LateUpdate()
    {
        transform.localScale = m_Scale + Vector3.one * Mathf.Sin(Time.time * m_SmoothTime) * m_Range;
    }
}
