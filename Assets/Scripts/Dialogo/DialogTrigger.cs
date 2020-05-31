using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public Dialog m_Dialog;
    public bool m_CanAsk;

    private void Update()
    {
        if (m_CanAsk && Input.GetButtonDown("Jump"))
        {
            DialogManager.Instance.BeginDialog(m_Dialog);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player"))
            m_CanAsk = true;
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player"))
            m_CanAsk = false;
    }
}
