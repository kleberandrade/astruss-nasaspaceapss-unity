using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeIn : MonoBehaviour
{
    public float m_Time = 0.5f;
    private float m_StartTime;
    private CanvasGroup m_CanvasGroup;
    public AnimationCurve m_Curve;

    private void Awake()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        m_CanvasGroup.alpha = 1.0f;
        m_StartTime = Time.time;
    }

    private void Update()
    {
        m_CanvasGroup.alpha = Mathf.Lerp(1.0f, 0.0f, m_Curve.Evaluate((Time.time - m_StartTime) / m_Time));
    }

}
