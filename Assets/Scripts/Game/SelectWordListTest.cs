using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectWordListTest : MonoBehaviour
{
    public TextAsset m_TextFile;

    private void Start()
    {
        var words = m_TextFile.text.Split('\n');
        Debug.Log($"Words count: {words.Length}");
        Debug.Log($"Word 0: {words[0]}");
    }
}
