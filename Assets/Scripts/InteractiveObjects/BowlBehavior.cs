using System.Collections;
using UnityEngine;

public class BowlBehavior : MonoBehaviour, IInteractable
{
    public Animator bowlAnimator; // �������� �����
    public Animator catAnimator; // �������� ����
    public Transform cat; // ������ �� ����
    public CatBehavior catBehavior; // ������ �� ��������� ����
    public Transform bowlPosition; // ������� �����
    public Transform carpetPosition; // ������� �����
    public float eatingDuration = 5.0f; // ������������ �������� ���
    public float startMovingDelay = 2.0f;
    public float stopDistance = 0.5f;
    public float startEatingDelay = 2f;
    public float carpetStopDistance = 0.5f;

    public string catReadyToEatSound = "cute"; // ���� ��� ������ ���������� ���
    public string bowlFillingSound = "food"; // ���� �� ����� ���������� �����
    public string catEatingSound = "cateat"; // ���� �� ����� ���
    public string catAfterEatingSound = "urring"; // ���� ����� ���

    private bool isFoodAdded = false; // ����, ������������, ��� ��� � �����

    private bool canAddFood = false; // ����� ���� ��� �������� ����������� �������� ����

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



        // ������������� ���� ���������� �����
        if (!string.IsNullOrEmpty(bowlFillingSound))
        {
            AudioManager.instance.Play(bowlFillingSound);
        }

        // �������� ���������� �����
        if (bowlAnimator != null)
        {
            bowlAnimator.SetTrigger("Fill");
        }

        // ������������� ���� ���������� �����, ���� �� ����������
        StartCoroutine(StopFillingSoundAfterDelay(2.0f)); // �������������� ������������ �����

        // ������������� ���� � ���������� ��� � �����
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
        // ������������� ���� ���������� ���
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

        // ������������� ���� ���
        if (!string.IsNullOrEmpty(catEatingSound))
        {
            AudioManager.instance.Play(catEatingSound);
        }

        yield return new WaitForSeconds(eatingDuration);

        // ������������� ���� ���, ���� �� ����������
        AudioManager.instance.Stop(catEatingSound);

        // ������������� ���� ����� ���
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

        // �������� ���� � ������� �����
        Vector3 directionToBowl = (bowlPosition.position - cat.position).normalized;
        directionToBowl.y = 0; // ������� ������������ ������������
        Quaternion targetRotation = Quaternion.LookRotation(directionToBowl);

        float rotationSpeed = 5.0f; // �������� ���������
        while (Quaternion.Angle(cat.rotation, targetRotation) > 0.1f)
        {
            cat.rotation = Quaternion.Slerp(cat.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        if (catBehavior != null)
        {
            catBehavior.enabled = false; // ������ ���� ���������
        }
    }
}
