using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField]
    float m_maxInteractDist = 2f;
    [SerializeField]
    LayerMask m_interactLayers;

    CharacterController m_charCtrl;

    private void Awake()
    {
        m_charCtrl = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            Debug.Log("Interact command received.");

            RaycastHit m_hit;

            Vector3 m_pos = transform.position;

            if (Physics.SphereCast(m_pos, m_charCtrl.height / 2, transform.forward, out m_hit, m_maxInteractDist, m_interactLayers))
            {
                Debug.Log("Hit!");

                MonoBehaviour _hitObject = m_hit.collider.gameObject.GetComponent<MonoBehaviour>();
                Debug.Log("Hit object:" + _hitObject.name);
                if (_hitObject is IInteractable)
                {
                    IInteractable _interactiveObject = (IInteractable)_hitObject;
                    _interactiveObject.Interact();
                }
            }
 
        }
    }
}
