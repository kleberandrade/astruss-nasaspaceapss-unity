using UnityEngine;

public class MovePingPongUI : MonoBehaviour, BehaviorUI
{
    public enum MovePingPongUIState { None, Running }

    public float m_Range = 0.05f;
    public float m_SmoothTime = 10.0f;
    public Vector3 m_Axis = Vector3.up;
    public float m_EnableTime = 0.75f;

    private Vector3 m_StartPosition;
    private MovePingPongUIState m_State = MovePingPongUIState.None;

    private void OnEnable()
    {
        Invoke("Enable", m_EnableTime);
    }

    public void Enable()
    {
        m_StartPosition = transform.position;
        m_State = MovePingPongUIState.Running;
    }

    public void OnDisable()
    {
        Disable();
    }

    public void Disable()
    {
        m_State = MovePingPongUIState.None;
    }

    private void Update()
    {
        if (m_State == MovePingPongUIState.None)
            return;

        transform.position = m_StartPosition + m_Axis * Mathf.Sin(Time.time * m_SmoothTime) * m_Range;
    }
}
