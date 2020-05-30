using UnityEngine;
using UnityEngine.UI;

public class GradientUI : MonoBehaviour
{
    private RawImage m_Image;
    private Texture2D m_Texture;
    public Color m_StartColor;
    public Color m_EndColor;

    private void Start()
    {
        m_Image = GetComponent<RawImage>();

        m_Texture = new Texture2D(1, 2);
        m_Texture.wrapMode = TextureWrapMode.Clamp;
        m_Texture.filterMode = FilterMode.Bilinear;

        SetColor(m_StartColor, m_EndColor);
    }

    public void SetColor(Color color1, Color color2)
    {
        m_Texture.SetPixels(new Color[] { color1, color2 });
        m_Texture.Apply();
        m_Image.texture = m_Texture;
        m_Image.color = Color.white;
    }
}

