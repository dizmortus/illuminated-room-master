using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour, IInteractable {

    Light m_light;

    private void Awake()
    {
        m_light = GetComponent<Light>();
    }

    public void Interact()
    {
        if (!m_light.enabled)
        {
            m_light.enabled = true;
        }
        else
        {
            m_light.enabled = false;
        }
        Debug.Log("Light interaction");
        
    }
    
}
