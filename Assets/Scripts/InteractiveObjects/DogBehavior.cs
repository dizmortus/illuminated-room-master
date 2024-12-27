using System.Collections;
using UnityEngine;

public class DogBehavior : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2.0f; // Скорость движения собаки
    public float followDistance = 3.0f; // Дистанция, на которой собака начинает следовать

    private Animator animator;
    private bool isInteracting = false; // Флаг для предотвращения повторных взаимодействий

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on the Dog object.");
        }

        if (player == null)
        {
            Debug.LogError("Player transform is not assigned.");
        }
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (isInteracting)
            return;

        if (distance > followDistance)
        {
            FollowPlayer();
        }
        else if (distance <= followDistance)
        {
            LookAtPlayer();
            if (animator != null)
                animator.SetBool("isWalking", false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(InteractWithObject());
        }
    }

    private void FollowPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Убираем вертикальную составляющую

        transform.position += direction * moveSpeed * Time.deltaTime;

        LookAtPlayer();

        if (animator != null)
            animator.SetBool("isWalking", true);
    }

    private void LookAtPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        directionToPlayer.y = 0; // Убираем вертикальную составляющую
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5.0f * Time.deltaTime);
    }

    private IEnumerator InteractWithObject()
    {
        isInteracting = true;

        // Запускаем анимацию взаимодействия
        if (animator != null)
        {
            animator.SetBool("Interact", true);
        }

        // Ждём 2 секунды
        yield return new WaitForSeconds(2.0f);

        // Отключаем анимацию взаимодействия
        if (animator != null)
        {
            animator.SetBool("Interact", false);

            // Принудительно сбрасываем позицию и состояние в анимации
            animator.Play("Idle", -1, 0f); // Указываем точное состояние Idle
        }

        isInteracting = false;
    }
}