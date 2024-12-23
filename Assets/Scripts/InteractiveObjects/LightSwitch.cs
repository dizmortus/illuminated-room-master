using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable {

    Animator m_animator;
    bool m_switchOn = false;

    List<GameObject> m_lights = new List<GameObject>();

    [SerializeField]
    private string m_switchOnSound;
    [SerializeField]
    private string m_switchOffSound;

    private void Awake()
    {
        m_animator = GetComponentInChildren<Animator>();
        m_lights.AddRange(GameObject.FindGameObjectsWithTag("Light"));
    }

    public void Interact()
    {
        Debug.Log("Interaction successful");

        if (!m_switchOn)
        {
            m_switchOn = true;
            m_animator.SetBool("switchOn", m_switchOn);
            AudioManager.instance.Stop(m_switchOffSound);
            AudioManager.instance.Play(m_switchOnSound);
            Debug.Log("Switch on");

            foreach (GameObject _obj in m_lights)
            {
                MonoBehaviour _foundObj = _obj.GetComponent<MonoBehaviour>();
                if (_foundObj is IInteractable)
                {
                    IInteractable _interactiveObject = (IInteractable)_foundObj;
                    _interactiveObject.Interact();
                    GameObject.Find("Directional Light").GetComponent<Light>().enabled = false;
                }
            }
        }
        else
        {
            m_switchOn = false;
            m_animator.SetBool("switchOn", m_switchOn);
            AudioManager.instance.Play(m_switchOnSound);
            AudioManager.instance.Stop(m_switchOffSound);
            Debug.Log("Switch off");

            foreach (GameObject _obj in m_lights)
            {
                MonoBehaviour _foundObj = _obj.GetComponent<MonoBehaviour>();
                if (_foundObj is IInteractable)
                {
                    IInteractable _interactiveObject = (IInteractable)_foundObj;
                    _interactiveObject.Interact();
                    GameObject.Find("Directional Light").GetComponent<Light>().enabled = true;
                }
            }
        }
    }
}
