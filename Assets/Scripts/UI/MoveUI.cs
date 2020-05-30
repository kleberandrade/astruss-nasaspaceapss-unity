using UnityEngine;


public class MoveUI : MonoBehaviour, BehaviorUI
{
    public enum MoveState { In, Idle, Out }
    private MoveState m_State = MoveState.In;

    [Header("Direction")]
    public float m_Range = 1000.0f;
    public Vector2 m_EnableDirection = Vector2.up;
    public Vector2 m_DisableDirection = Vector2.down;

    [Header("Speed")]
    public bool m_AutoEnable = true;
    public float m_EnableTime = 0.75f;
    public float m_DisableTime = 0.5f;
    public AnimationCurve m_XCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
    public AnimationCurve m_YCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

    private float m_ElapsedTime;
    private Vector2 m_HomePosition;
    private Vector2 m_DisablePosition;
    private Vector2 m_EnablePosition;
    private bool m_Completed;
    private RectTransform m_Transform;

    private void Awake()
    {
        m_Transform = GetComponent<RectTransform>();
        m_HomePosition = m_Transform.anchoredPosition;
        m_EnablePosition = m_HomePosition + m_DisableDirection * m_Range;
        m_DisablePosition = m_HomePosition + m_EnableDirection * m_Range;

        m_Transform.anchoredPosition = m_EnablePosition;
    }

    public void Enable()
    {
        m_ElapsedTime = 0.0f;
        m_State = MoveState.In;
    }

    public void Disable()
    {
        m_ElapsedTime = 0.0f;
        m_State = MoveState.Out;
    }

    private void OnEnable()
    {
        if (m_AutoEnable)
        {
            Enable();
        }
    }

    private void Update()
    {
        if (m_State == MoveState.Idle)
            return;

        if (m_State == MoveState.In)
        {
            m_Transform.anchoredPosition = Lerp(m_DisablePosition, m_HomePosition, m_ElapsedTime / m_EnableTime, out m_Completed);
            if (m_Completed) m_State = MoveState.Idle;
        }

        if (m_State == MoveState.Out)
        {
            m_Transform.anchoredPosition = Lerp(m_HomePosition, m_EnablePosition, m_ElapsedTime / m_DisableTime, out m_Completed);
            if (m_Completed) m_State = MoveState.Idle;
        }

        m_ElapsedTime += Time.deltaTime;
    }

    private Vector2 Lerp(Vector2 from, Vector2 to, float time, out bool completed)
    {
        float x = Mathf.LerpUnclamped(from.x, to.x, m_XCurve.Evaluate(time));
        float y = Mathf.LerpUnclamped(from.y, to.y, m_YCurve.Evaluate(time));

        var position = new Vector2(x, y);

        completed = Vector3.Distance(position, to) < 0.01f;

        return position;
    }
}