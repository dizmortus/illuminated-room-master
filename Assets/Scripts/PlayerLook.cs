using System.Collections;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField]
    private float m_mouseSensitivity = 150.0f;

    [SerializeField]
    private Transform m_playerBody;

    private float m_xAxisClamp = 0.0f;
    private bool isFocusing = false; // Флаг для блокировки ручного управления

    private void Awake()
    {
        LockCursor();
    }

    void Update()
    {
        if (!isFocusing)
        {
            CameraRotation();
        }
    }

    public IEnumerator FocusOnTarget(Transform target, float focusDuration = 2.0f, float focusSpeed = 5.0f)
    {
        isFocusing = true;

        float elapsedTime = 0.0f;
        Quaternion initialCameraRotation = transform.rotation;
        Quaternion initialBodyRotation = m_playerBody.rotation;

        // Направление на цель
        Vector3 directionToTarget = (target.position - transform.position).normalized;

        // Целевые повороты
        Quaternion targetBodyRotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0, directionToTarget.z));
        Quaternion targetCameraRotation = Quaternion.LookRotation(directionToTarget);

        while (elapsedTime < focusDuration)
        {
            elapsedTime += Time.deltaTime;

            // Плавное поворачивание тела игрока
            m_playerBody.rotation = Quaternion.Slerp(initialBodyRotation, targetBodyRotation, elapsedTime / focusDuration);

            // Плавное поворачивание камеры
            transform.rotation = Quaternion.Slerp(initialCameraRotation, targetCameraRotation, elapsedTime / focusDuration);

            yield return null;
        }

        // Гарантируем финальное положение
        m_playerBody.rotation = targetBodyRotation;
        transform.rotation = targetCameraRotation;

        // Обновляем m_xAxisClamp для синхронизации с новым углом
        Vector3 cameraEulerAngles = transform.localEulerAngles;
        m_xAxisClamp = cameraEulerAngles.x > 180 ? cameraEulerAngles.x - 360 : cameraEulerAngles.x;

        isFocusing = false; // Разрешаем управление камерой после фокусировки
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void CameraRotation()
    {
        float _mouseX = Input.GetAxisRaw("Mouse X") * m_mouseSensitivity * Time.deltaTime;
        float _mouseY = Input.GetAxisRaw("Mouse Y") * m_mouseSensitivity * Time.deltaTime;

        // Проверяем накопленное значение m_xAxisClamp
        m_xAxisClamp -= _mouseY; // Уменьшаем из-за перевёрнутой оси Y
        m_xAxisClamp = Mathf.Clamp(m_xAxisClamp, -90.0f, 90.0f);

        transform.localEulerAngles = new Vector3(m_xAxisClamp, 0, 0); // Управление камерой
        m_playerBody.Rotate(Vector3.up * _mouseX); // Вращение тела игрока
    }
}
