using System.Collections;
using UnityEngine;

public class CatBehavior : MonoBehaviour
{
    public Transform player;
    public PlayerMove playerMoveScript; // Ссылка на PlayerMove
    public PlayerLook playerLookScript;
    public float moveSpeed = 2.0f; // Скорость движения кота
    public float followDistance = 5.0f; // Дистанция, на которой кот начинает следовать
    public float interactionDistance = 2.0f; // Дистанция для взаимодействия
    public float interactionCooldown = 5.0f; // Задержка между взаимодействиями
    public string meowSound = "Meow";

    private Animator animator;
    private bool isInteracting = false;
    private float lastInteractionTime = -Mathf.Infinity; // Время последнего взаимодействия

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (isInteracting)
        {
            // Если идёт взаимодействие, кот не двигается
            if (animator != null)
                animator.SetBool("isRunning", false);
            return;
        }

        if (distance > followDistance)
        {
            // Если игрок далеко, кот бежит за ним
            FollowPlayer();
        }
        else if (distance <= interactionDistance && Time.time > lastInteractionTime + interactionCooldown)
        {
            // Если игрок вблизи и cooldown истёк, начинается взаимодействие
            StartCoroutine(InteractWithPlayer());
        }
        else if (distance <= followDistance)
        {
            // Если игрок в зоне, но не в зоне взаимодействия, кот смотрит на игрока
            LookAtPlayer();
            if (animator != null)
                animator.SetBool("isRunning", false);
        }
    }

    private void FollowPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Убираем вертикальную составляющую

        transform.position += direction * moveSpeed * Time.deltaTime;

        // Поворачиваем кота к игроку
        LookAtPlayer();

        if (animator != null)
            animator.SetBool("isRunning", true);
    }

    private void LookAtPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        directionToPlayer.y = 0; // Убираем вертикальную составляющую
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        // Плавный поворот к игроку
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5.0f * Time.deltaTime);
    }

    private IEnumerator InteractWithPlayer()
    {
        isInteracting = true;

        // Останавливаем кота
        if (animator != null)
            animator.SetBool("isRunning", false);

        // Блокируем движение и управление камерой
        if (playerMoveScript != null) playerMoveScript.isMovementBlocked = true;
        if (playerLookScript != null) playerLookScript.enabled = false;

        // Звуковое взаимодействие
        Debug.Log("Cat: Feed me!");
        AudioManager.instance.Play(meowSound);

        // Фокусируем камеру на кота
        if (playerLookScript != null)
        {
            yield return playerLookScript.FocusOnTarget(transform, focusDuration: 1.0f, focusSpeed: 5.0f);
        }

        // Ждём 2 секунды, чтобы симулировать взаимодействие
        yield return new WaitForSeconds(2.0f);

        // Разблокируем управление
        if (playerMoveScript != null) playerMoveScript.isMovementBlocked = false;
        if (playerLookScript != null) playerLookScript.enabled = true;

        // Фиксируем время последнего взаимодействия
        lastInteractionTime = Time.time;

        isInteracting = false; // Разрешаем новое взаимодействие
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

            // Поворот к цели
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5.0f * Time.deltaTime);

            yield return null;
        }
    }



}
