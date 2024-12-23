using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerInteraction : MonoBehaviour, IInteractable
{
    private Animator m_animator;
    private bool m_animationPlayed = false; // ���� ��� ��������, ������������� �� ��������

    [SerializeField]
    private string m_flowerAnimationTrigger; // ��� �������� ��� ��������

    [SerializeField]
    private string m_interactionSound; // ��� ����� ��� ��������������� ��� ��������������

    private void Awake()
    {
        m_animator = GetComponent<Animator>();

        if (m_animator == null)
        {
            Debug.LogError("Animator component is missing on the flower object.");
        }
    }

    public void Interact()
    {
        Debug.Log("Flower interaction successful");

        // ������ ��������
        if (m_animator != null)
        {
            m_animator.SetTrigger(m_flowerAnimationTrigger);
            Debug.Log("Flower animation triggered.");
        }

        // ��������������� ����� ��������������
        if (!string.IsNullOrEmpty(m_interactionSound))
        {
            AudioManager.instance.Play(m_interactionSound);
            Debug.Log("Flower interaction sound played.");
        }
    }

}
