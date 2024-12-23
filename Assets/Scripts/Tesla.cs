using UnityEngine;

public class Tesla : MonoBehaviour
{
    [SerializeField]
    private float m_turnSpeed = 200f; // Скорость поворота

    private Animator m_animator;

    [SerializeField]
    private string m_driveAnimationTrigger; // Имя триггера для анимации движения
    [SerializeField]
    private string m_interactionSound; // Имя звука для воспроизведения при взаимодействии

    private bool isMouseOver = false; // Флаг для отслеживания, находится ли курсор над объектом

    private void Awake()
    {
        m_animator = GetComponent<Animator>();

        if (m_animator == null)
        {
            Debug.LogError("Animator component is missing on the Tesla object.");
        }
    }

    private void Update()
    {
        HandleInteraction();
    }

    private void HandleInteraction()
    {
        // Проверка нажатия клавиши "E" только если курсор над объектом
        if (isMouseOver && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void OnMouseEnter()
    {
        isMouseOver = true; // Установить флаг, когда курсор над объектом
    }

    private void OnMouseExit()
    {
        isMouseOver = false; // Сбросить флаг, когда курсор покидает объект
    }

    public void Interact()
    {
        Debug.Log("Tesla interaction successful");

        // Запуск анимации движения
        if (m_animator != null)
        {
            m_animator.SetTrigger(m_driveAnimationTrigger);
            Debug.Log("Drive animation triggered.");
        }

        // Воспроизведение звука взаимодействия
        if (!string.IsNullOrEmpty(m_interactionSound))
        {
            AudioManager.instance.Play(m_interactionSound);
            Debug.Log("Tesla interaction sound played.");
        }
    }
}