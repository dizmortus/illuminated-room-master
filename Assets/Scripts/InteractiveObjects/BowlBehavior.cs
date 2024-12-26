using System.Collections;
using UnityEngine;

public class BowlBehavior : MonoBehaviour, IInteractable
{
    public Animator bowlAnimator; // Аниматор миски
    public Animator catAnimator; // Аниматор кота
    public Transform cat; // Ссылка на кота
    public CatBehavior catBehavior; // Ссылка на поведение кота
    public Transform bowlPosition; // Позиция миски
    public float eatingDuration = 5.0f; // Длительность процесса еды

    private bool isFoodAdded = false; // Флаг, показывающий, что еда в миске

    // Реализация метода интерфейса IInteractable
    public void Interact()
    {
        Debug.Log("Player interacted with the bowl.");
        if (!isFoodAdded)
        {
            Debug.Log("Food is being added to the bowl.");
            FillBowl(); // Начинаем процесс заполнения миски
        }
        else
        {
            Debug.Log("Bowl is already filled. Interaction ignored.");
        }
    }

    private void FillBowl()
    {
        isFoodAdded = true;

        // Анимация заполнения миски
        if (bowlAnimator != null)
        {
            Debug.Log("Triggering bowl fill animation.");
            bowlAnimator.SetTrigger("Fill");
        }

        // Останавливаем кота и отправляем его к миске
        if (catBehavior != null)
        {
            Debug.Log("Disabling cat behavior and moving cat to the bowl.");
            catBehavior.enabled = false; // Отключаем поведение кота
            StartCoroutine(CatEatsFood());
        }
        else
        {
            Debug.LogWarning("CatBehavior is not assigned. Cannot move cat to the bowl.");
        }
    }

    private IEnumerator CatEatsFood()
    {
        if (catBehavior != null)
        {
            Debug.Log("Cat is heading towards the bowl.");
            catBehavior.FollowTarget(bowlPosition.position);
        }

        // Ждём, пока кот добежит
        while (Vector3.Distance(cat.position, bowlPosition.position) > 0.5f)
        {
            yield return null;
        }
        Debug.Log("Cat has reached the bowl.");

/*        // Анимация поедания еды
        if (catAnimator != null)
        {
            Debug.Log("Triggering cat eating animation.");
            catAnimator.SetTrigger("Eat");
        }*/

        // Анимация опустошения миски
        yield return new WaitForSeconds(1.0f); // Подождите перед началом
        if (bowlAnimator != null)
        {
            Debug.Log("Triggering bowl empty animation.");
            bowlAnimator.SetTrigger("Empty");
        }

        // Ожидание завершения еды
        yield return new WaitForSeconds(eatingDuration);

        // Делаем кота пассивным
        if (catBehavior != null)
        {
            Debug.Log("Cat has finished eating. Disabling cat behavior.");
            catBehavior.enabled = false; // Отключаем скрипт поведения кота
        }
    }
}
