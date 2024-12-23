using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotebookInteraction : MonoBehaviour, IInteractable
{
    Animator m_animator;
    bool m_isOpen = true;

    [SerializeField]
    private string m_openNotebookSound;
    [SerializeField]
    private string m_closeNotebookSound;

    private void Awake()
    {
        m_animator = GetComponentInChildren<Animator>();
    }

    public void Interact()
    {
        Debug.Log("Notebook interaction successful");

        if (!m_isOpen)
        {
            m_isOpen = true;
            m_animator.SetBool("isOpen", m_isOpen);
            AudioManager.instance.Play(m_openNotebookSound);
            Debug.Log("Notebook opened");

            // Additional logic for when the notebook is opened
        }
        else
        {
            m_isOpen = false;
            m_animator.SetBool("isOpen", m_isOpen);
            AudioManager.instance.Play(m_closeNotebookSound);
            Debug.Log("Notebook closed");

            // Additional logic for when the notebook is closed
        }
    }
}
