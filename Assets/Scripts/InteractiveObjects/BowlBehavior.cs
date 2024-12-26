using System.Collections;
using UnityEngine;

public class BowlBehavior : MonoBehaviour, IInteractable
{
    public Animator bowlAnimator; // �������� �����
    public Animator catAnimator; // �������� ����
    public Transform cat; // ������ �� ����
    public CatBehavior catBehavior; // ������ �� ��������� ����
    public Transform bowlPosition; // ������� �����
    public float eatingDuration = 5.0f; // ������������ �������� ���

    private bool isFoodAdded = false; // ����, ������������, ��� ��� � �����

    // ���������� ������ ���������� IInteractable
    public void Interact()
    {
        Debug.Log("Player interacted with the bowl.");
        if (!isFoodAdded)
        {
            Debug.Log("Food is being added to the bowl.");
            FillBowl(); // �������� ������� ���������� �����
        }
        else
        {
            Debug.Log("Bowl is already filled. Interaction ignored.");
        }
    }

    private void FillBowl()
    {
        isFoodAdded = true;

        // �������� ���������� �����
        if (bowlAnimator != null)
        {
            Debug.Log("Triggering bowl fill animation.");
            bowlAnimator.SetTrigger("Fill");
        }

        // ������������� ���� � ���������� ��� � �����
        if (catBehavior != null)
        {
            Debug.Log("Disabling cat behavior and moving cat to the bowl.");
            catBehavior.enabled = false; // ��������� ��������� ����
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

        // ���, ���� ��� �������
        while (Vector3.Distance(cat.position, bowlPosition.position) > 0.5f)
        {
            yield return null;
        }
        Debug.Log("Cat has reached the bowl.");

/*        // �������� �������� ���
        if (catAnimator != null)
        {
            Debug.Log("Triggering cat eating animation.");
            catAnimator.SetTrigger("Eat");
        }*/

        // �������� ����������� �����
        yield return new WaitForSeconds(1.0f); // ��������� ����� �������
        if (bowlAnimator != null)
        {
            Debug.Log("Triggering bowl empty animation.");
            bowlAnimator.SetTrigger("Empty");
        }

        // �������� ���������� ���
        yield return new WaitForSeconds(eatingDuration);

        // ������ ���� ���������
        if (catBehavior != null)
        {
            Debug.Log("Cat has finished eating. Disabling cat behavior.");
            catBehavior.enabled = false; // ��������� ������ ��������� ����
        }
    }
}
