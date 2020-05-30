using UnityEngine;

public class ShakeUI : MonoBehaviour
{
	public float m_Decay = 0.002f;
	public float m_Intensity = .3f;
	public float m_Power = 50.0f;

	private Quaternion m_OriginRotation;
	private float m_CurrentIntensity = 0;

	private RectTransform m_Transform;

	private void Awake()
	{
		m_Transform = GetComponent<RectTransform>();
		m_OriginRotation = Quaternion.identity;
	}

	private void Update()
	{
		if (m_CurrentIntensity > 0)
		{
			var angle = Random.Range(-m_CurrentIntensity, m_CurrentIntensity) * m_Power;
			var rotation = Quaternion.Euler(0, 0, angle);
			m_Transform.rotation = rotation;

			m_CurrentIntensity -= m_Decay;
		}
	}

	public void Shake()
	{
		m_CurrentIntensity = m_Intensity;
	}
}
