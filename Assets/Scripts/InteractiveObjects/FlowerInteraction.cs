using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerInteraction : MonoBehaviour, IInteractable
{
    private Animator m_animator;
    private bool m_animationPlayed = false; // Флаг для проверки, проигрывалась ли анимация

    [SerializeField]
    private string m_flowerAnimationTrigger; // Имя триггера для анимации

    [SerializeField]
    private string m_interactionSound; // Имя звука для воспроизведения при взаимодействии

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

        // Запуск анимации
        if (m_animator != null)
        {
            m_animator.SetTrigger(m_flowerAnimationTrigger);
            Debug.Log("Flower animation triggered.");
        }

        // Воспроизведение звука взаимодействия
        if (!string.IsNullOrEmpty(m_interactionSound))
        {
            AudioManager.instance.Play(m_interactionSound);
            Debug.Log("Flower interaction sound played.");
        }
    }

}
