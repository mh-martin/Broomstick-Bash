using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float m_acceleration = 3.0f;

    [SerializeField]
    private Rigidbody2D m_rigidbody;

    private IPlayerControls m_controls;

    private void Awake()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        Application.targetFrameRate = 60;
#endif
        m_controls = GetComponent<IPlayerControls>();
        if (m_controls == null)
        {
            gameObject.AddComponent<ProtoControls>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

    private void FixedUpdate()
    {
        if (m_controls.IsFlying)
        {
            m_rigidbody.AddForce(new Vector2(0, m_acceleration));
        }
    }
}
