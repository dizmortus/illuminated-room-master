using UnityEngine;
    
public class FoodCanBehavior : MonoBehaviour, IInteractable
{
    public Animator canAnimator; // �������� �����
    public BowlBehavior bowl; // ������ �� �����
    public string openSound = "can_open"; // ���� ��������
    public string takeSound = "can_take"; // ���� ������ �����

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

        // �������� �������� �����
        if (canAnimator != null)
        {
            canAnimator.SetTrigger("Open");
        }

        // ���� �������� �����
        if (!string.IsNullOrEmpty(openSound))
        {
            AudioManager.instance.Play(openSound);
        }

        Debug.Log("The can is now opened.");
    }

    private void TakeCan()
    {
        state = FoodCanState.Gone;

        // ���� ������ �����
        if (!string.IsNullOrEmpty(takeSound))
        {
            AudioManager.instance.Play(takeSound);
        }

        // ���������� �����, ��� ����� �������� ����
        if (bowl != null)
        {
            bowl.AllowFoodAddition();
        }

        // ������� ����� �� �����
        gameObject.SetActive(false);

        Debug.Log("The can is taken, and food can now be added to the bowl.");
    }
}
