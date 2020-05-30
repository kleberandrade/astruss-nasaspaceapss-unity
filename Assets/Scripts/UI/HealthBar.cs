using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("UI")]
    public Image m_Bar;
    public Color m_CoolColor;
    public Color m_HotColor;

    public void SetValue(float value)
    {
        m_Bar.fillAmount = value;
        m_Bar.color = Color.Lerp(m_CoolColor, m_HotColor, value);
    }
}