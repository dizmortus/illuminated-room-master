using UnityEngine;

public class BookAnimator : MonoBehaviour
{
    [SerializeField]
    private string m_animationTrigger = "OpenBook"; // Имя триггера для анимации книги

    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();

        if (m_animator == null)
        {
            Debug.LogError("Animator component is missing on the Book object.");
        }
    }

    private void Update()
    {
        HandleAnimationInput();
    }

    private void HandleAnimationInput()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Клавиша для активации анимации
        {
            PlayAnimation();
        }
    }

    private void PlayAnimation()
    {
        if (m_animator != null)
        {
            m_animator.SetTrigger(m_animationTrigger);
            Debug.Log("Book animation triggered.");
        }
    }
}