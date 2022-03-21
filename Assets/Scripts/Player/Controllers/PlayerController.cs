using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float m_acceleration = 3.0f;

    [SerializeField]
    private Rigidbody2D m_rigidbody;

    [SerializeField]
    private float m_maxVelocity = 15.0f;

    private IPlayerControls m_controls;

    private Vector3 m_velocity = Vector3.zero;

    private Transform m_transform;

    private void Awake()
    {
#if UNITY_ANDROID
        Application.targetFrameRate = 60;
#endif
        m_controls = GetComponent<IPlayerControls>();
        if (m_controls == null)
        {
            gameObject.AddComponent<ProtoControls>();
        }
        m_transform = transform;
    }

    private void Update()
    {
        if (m_controls.IsFlying)
        {
            m_velocity.y += m_acceleration * Time.deltaTime;
        }
        else
        {
            m_velocity.y -= m_acceleration * Time.deltaTime;
        }

        m_velocity.y = Mathf.Clamp(m_velocity.y, -m_maxVelocity, m_maxVelocity);
        m_transform.position += m_velocity * Time.deltaTime;//MovePosition(m_rigidbody.position + m_velocity);
    }

    private void FixedUpdate()
    {
        m_rigidbody.MovePosition(m_transform.position);
    }
}
