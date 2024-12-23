using UnityEngine;

public class Tesla : MonoBehaviour
{
    [SerializeField]
    private float m_turnSpeed = 200f; // �������� ��������

    private Animator m_animator;

    [SerializeField]
    private string m_driveAnimationTrigger; // ��� �������� ��� �������� ��������
    [SerializeField]
    private string m_interactionSound; // ��� ����� ��� ��������������� ��� ��������������

    private bool isMouseOver = false; // ���� ��� ������������, ��������� �� ������ ��� ��������

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
        // �������� ������� ������� "E" ������ ���� ������ ��� ��������
        if (isMouseOver && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void OnMouseEnter()
    {
        isMouseOver = true; // ���������� ����, ����� ������ ��� ��������
    }

    private void OnMouseExit()
    {
        isMouseOver = false; // �������� ����, ����� ������ �������� ������
    }

    public void Interact()
    {
        Debug.Log("Tesla interaction successful");

        // ������ �������� ��������
        if (m_animator != null)
        {
            m_animator.SetTrigger(m_driveAnimationTrigger);
            Debug.Log("Drive animation triggered.");
        }

        // ��������������� ����� ��������������
        if (!string.IsNullOrEmpty(m_interactionSound))
        {
            AudioManager.instance.Play(m_interactionSound);
            Debug.Log("Tesla interaction sound played.");
        }
    }
}