using UnityEngine;
    
public class FoodCanBehavior : MonoBehaviour, IInteractable
{
    public Animator canAnimator; // Аниматор банки
    public BowlBehavior bowl; // Ссылка на миску
    public string openSound = "can_open"; // Звук открытия
    public string takeSound = "can_take"; // Звук взятия банки

    public enum FoodCanState { Closed, Opened, Gone }
    public FoodCanState state = FoodCanState.Closed;

    public void Interact()
    {
        switch (state)
        {
            case FoodCanState.Closed:
                OpenCan();
                break;

            case FoodCanState.Opened:
                TakeCan();
                break;

            case FoodCanState.Gone:
                Debug.Log("The can is already gone.");
                break;
        }
    }

    private void OpenCan()
    {
        state = FoodCanState.Opened;

        // Анимация открытия банки
        if (canAnimator != null)
        {
            canAnimator.SetTrigger("Open");
        }

        // Звук открытия банки
        if (!string.IsNullOrEmpty(openSound))
        {
            AudioManager.instance.Play(openSound);
        }

        Debug.Log("The can is now opened.");
    }

    private void TakeCan()
    {
        state = FoodCanState.Gone;

        // Звук взятия банки
        if (!string.IsNullOrEmpty(takeSound))
        {
            AudioManager.instance.Play(takeSound);
        }

        // Уведомляем миску, что можно положить корм
        if (bowl != null)
        {
            bowl.AllowFoodAddition();
        }

        // Убираем банку из сцены
        gameObject.SetActive(false);

        Debug.Log("The can is taken, and food can now be added to the bowl.");
    }
}
