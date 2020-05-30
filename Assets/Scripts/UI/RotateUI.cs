using UnityEngine;

public class RotateUI : MonoBehaviour
{
    private enum RotateTypeUI { None, Continuous, Discreet }
    private RotateTypeUI m_Type = RotateTypeUI.None;
    public Vector3 m_RotateAxis = Vector3.forward;

    [Header("Continuous")]
    public bool m_UseContinuous = false;
    public float m_Speed = 180.0f;
    
    [Header("Discreet")]
    public float m_Time = 0.25f;
    public float m_ToAngle = 720.0f;
    public AnimationCurve m_Curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

    private float m_Angle = 0.0f;
    private float m_StartTime = 0.0f;
    private RectTransform m_Transform;

    private void Awake()
    {
        m_Transform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        m_Type = m_UseContinuous ? RotateTypeUI.Continuous : RotateTypeUI.None;
    }

    private void Update()
    {
        if (m_Type == RotateTypeUI.None)
            return;

        if (m_Type == RotateTypeUI.Continuous)
        {
            m_Transform.Rotate(m_RotateAxis * m_Speed * Time.deltaTime);
        } 
       
        if (m_Type == RotateTypeUI.Discreet)
        {
            float rate = (Time.time - m_StartTime) / m_Time;
            m_Angle = Mathf.Lerp(0.0f, m_ToAngle, m_Curve.Evaluate(rate));
            m_Transform.rotation = Quaternion.Euler(m_RotateAxis * m_Angle);

            if (m_Angle >= m_ToAngle)
            {
                m_Type = RotateTypeUI.None;
                m_Transform.rotation = Quaternion.identity;
            }
        }
    }

    public void Rotate()
    {
        m_StartTime = Time.time;    
        m_Type = RotateTypeUI.Discreet;
    }
}
