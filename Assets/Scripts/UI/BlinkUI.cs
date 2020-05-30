using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class BlinkUI : MonoBehaviour
{
    public enum BlinkUIState { None, Running }

    public float m_SmoothTime = 8.0f;
    public float m_EnableTime = 0.75f;

    private CanvasGroup m_CanvasGroup;
    private BlinkUIState m_State = BlinkUIState.None;

    private void Awake()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();
        Invoke("Enable", m_EnableTime);
    }

    private void Enable()
    {
        m_State = BlinkUIState.Running;
}

    private void Update()
    {
        if (m_State != BlinkUIState.Running)
            return;

        m_CanvasGroup.alpha = (Mathf.Sin(Time.time * m_SmoothTime) + 1.0f) * 0.5f;
    }
}
