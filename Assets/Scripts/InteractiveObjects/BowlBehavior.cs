using System.Collections;
using UnityEngine;

public class BowlBehavior : MonoBehaviour, IInteractable
{
    public Animator bowlAnimator; // Аниматор миски
    public Animator catAnimator; // Аниматор кота
    public Transform cat; // Ссылка на кота
    public CatBehavior catBehavior; // Ссылка на поведение кота
    public Transform bowlPosition; // Позиция миски
    public Transform carpetPosition; // Позиция ковра
    public float eatingDuration = 5.0f; // Длительность процесса еды
    public float startMovingDelay = 2.0f;
    public float stopDistance = 0.5f;
    public float startEatingDelay = 2f;
    public float carpetStopDistance = 0.5f;

    public string catReadyToEatSound = "cute"; // Звук при начале добавления еды
    public string bowlFillingSound = "food"; // Звук во время заполнения миски
    public string catEatingSound = "cateat"; // Звук во время еды
    public string catAfterEatingSound = "urring"; // Звук после еды

    private bool isFoodAdded = false; // Флаг, показывающий, что еда в миске

    private bool canAddFood = false; // Новый флаг для проверки возможности добавить корм

    public void AllowFoodAddition()
    {
        canAddFood = true;
        Debug.Log("Food addition is now allowed.");
    }

    public void Interact()
    {
        if (!canAddFood)
        {
            Debug.Log("You need to prepare the food first.");
            return;
        }

        if (!isFoodAdded)
        {
            FillBowl();
        }
        else
        {
            Debug.Log("Bowl is already filled. Interaction ignored.");
        }
    }


    private void FillBowl()
    {
        isFoodAdded = true;



        // Воспроизводим звук заполнения миски
        if (!string.IsNullOrEmpty(bowlFillingSound))
        {
            AudioManager.instance.Play(bowlFillingSound);
        }

        // Анимация заполнения миски
        if (bowlAnimator != null)
        {
            bowlAnimator.SetTrigger("Fill");
        }

        // Останавливаем звук заполнения миски, если он длительный
        StartCoroutine(StopFillingSoundAfterDelay(2.0f)); // Предполагаемая длительность звука

        // Останавливаем кота и отправляем его к миске
        if (catBehavior != null)
        {
            catBehavior.enabled = false;
            StartCoroutine(CatEatsFood());
        }
    }

    private IEnumerator StopFillingSoundAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!string.IsNullOrEmpty(bowlFillingSound))
        {
            AudioManager.instance.Stop(bowlFillingSound);
        }
    }

    private IEnumerator CatEatsFood()
    {
        yield return new WaitForSeconds(startMovingDelay);
        // Воспроизводим звук добавления еды
        if (!string.IsNullOrEmpty(catReadyToEatSound))
        {
            AudioManager.instance.Play(catReadyToEatSound);
        }

        Vector3 targetPosition = bowlPosition.position;
        if (bowlPosition.TryGetComponent<Collider>(out Collider bowlCollider))
        {
            targetPosition = bowlCollider.ClosestPoint(cat.position);
        }

        if (catAnimator != null)
        {
            catAnimator.SetBool("isRunning", true);
        }

        if (catBehavior != null)
        {
            catBehavior.FollowTarget(targetPosition);
            while (Vector3.Distance(cat.position, targetPosition) > stopDistance)
            {
                yield return null;
            }
        }

        if (catAnimator != null)
        {
            catAnimator.SetBool("isRunning", false);
        }

        yield return new WaitForSeconds(startEatingDelay);

        if (catAnimator != null)
        {
            catAnimator.SetTrigger("Eat");
        }
        if (bowlAnimator != null)
        {
            bowlAnimator.SetTrigger("Empty");
        }

        // Воспроизводим звук еды
        if (!string.IsNullOrEmpty(catEatingSound))
        {
            AudioManager.instance.Play(catEatingSound);
        }

        yield return new WaitForSeconds(eatingDuration);

        // Останавливаем звук еды, если он длительный
        AudioManager.instance.Stop(catEatingSound);

        // Воспроизводим звук после еды
        if (!string.IsNullOrEmpty(catAfterEatingSound))
        {
            AudioManager.instance.Play(catAfterEatingSound);
        }


        StartCoroutine(MoveToCarpet());
    }

    private IEnumerator MoveToCarpet()
    {
        Vector3 targetPosition = carpetPosition.position;

        if (catAnimator != null)
        {
            catAnimator.SetBool("isRunning", true);
        }

        if (catBehavior != null)
        {
            catBehavior.FollowTarget(targetPosition);
            while (Vector3.Distance(cat.position, targetPosition) > carpetStopDistance)
            {
                yield return null;
            }
        }

        if (catAnimator != null)
        {
            catAnimator.SetBool("isRunning", false);
            catAnimator.SetTrigger("Sit");
        }

        // Разворот кота в сторону миски
        Vector3 directionToBowl = (bowlPosition.position - cat.position).normalized;
        directionToBowl.y = 0; // Убираем вертикальную составляющую
        Quaternion targetRotation = Quaternion.LookRotation(directionToBowl);

        float rotationSpeed = 5.0f; // Скорость разворота
        while (Quaternion.Angle(cat.rotation, targetRotation) > 0.1f)
        {
            cat.rotation = Quaternion.Slerp(cat.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        if (catBehavior != null)
        {
            catBehavior.enabled = false; // Делаем кота пассивным
        }
    }
}
