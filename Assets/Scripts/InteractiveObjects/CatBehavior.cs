using System.Collections;
using UnityEngine;

public class CatBehavior : MonoBehaviour
{
    public Transform player;
    public PlayerMove playerMoveScript; // ������ �� PlayerMove
    public PlayerLook playerLookScript;
    public float moveSpeed = 2.0f; // �������� �������� ����
    public float followDistance = 5.0f; // ���������, �� ������� ��� �������� ���������
    public float interactionDistance = 2.0f; // ��������� ��� ��������������
    public float interactionCooldown = 5.0f; // �������� ����� ����������������
    public string meowSound = "Meow";

    private Animator animator;
    private bool isInteracting = false;
    private float lastInteractionTime = -Mathf.Infinity; // ����� ���������� ��������������

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (isInteracting)
        {
            // ���� ��� ��������������, ��� �� ���������
            if (animator != null)
                animator.SetBool("isRunning", false);
            return;
        }

        if (distance > followDistance)
        {
            // ���� ����� ������, ��� ����� �� ���
            FollowPlayer();
        }
        else if (distance <= interactionDistance && Time.time > lastInteractionTime + interactionCooldown)
        {
            // ���� ����� ������ � cooldown ����, ���������� ��������������
            StartCoroutine(InteractWithPlayer());
        }
        else if (distance <= followDistance)
        {
            // ���� ����� � ����, �� �� � ���� ��������������, ��� ������� �� ������
            LookAtPlayer();
            if (animator != null)
                animator.SetBool("isRunning", false);
        }
    }

    private void FollowPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // ������� ������������ ������������

        transform.position += direction * moveSpeed * Time.deltaTime;

        // ������������ ���� � ������
        LookAtPlayer();

        if (animator != null)
            animator.SetBool("isRunning", true);
    }

    private void LookAtPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        directionToPlayer.y = 0; // ������� ������������ ������������
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        // ������� ������� � ������
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5.0f * Time.deltaTime);
    }

    private IEnumerator InteractWithPlayer()
    {
        isInteracting = true;

        // ������������� ����
        if (animator != null)
            animator.SetBool("isRunning", false);

        // ��������� �������� � ���������� �������
        if (playerMoveScript != null) playerMoveScript.isMovementBlocked = true;
        if (playerLookScript != null) playerLookScript.enabled = false;

        // �������� ��������������
        Debug.Log("Cat: Feed me!");
        AudioManager.instance.Play(meowSound);

        // ���������� ������ �� ����
        if (playerLookScript != null)
        {
            yield return playerLookScript.FocusOnTarget(transform, focusDuration: 1.0f, focusSpeed: 5.0f);
        }

        // ��� 2 �������, ����� ������������ ��������������
        yield return new WaitForSeconds(2.0f);

        // ������������ ����������
        if (playerMoveScript != null) playerMoveScript.isMovementBlocked = false;
        if (playerLookScript != null) playerLookScript.enabled = true;

        // ��������� ����� ���������� ��������������
        lastInteractionTime = Time.time;

        isInteracting = false; // ��������� ����� ��������������
    }
    public void FollowTarget(Vector3 targetPosition)
    {
        StartCoroutine(MoveToTarget(targetPosition));
    }

    private IEnumerator MoveToTarget(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // ������� � ����
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5.0f * Time.deltaTime);

            yield return null;
        }
    }



}
