using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float flightPower = 3.0f;

    [SerializeField]
    private float forwardMovementSpeed = 3.0f;

    [SerializeField]
    private Rigidbody2D playerRigidbody;

    private uint magicEnergy = 0;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        Application.targetFrameRate = 60;
#endif
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Energy"))
        {
            CollectEnergy(collider);
        }
    }

    // flying
    private void FixedUpdate()
    {
        bool isFlying = Input.GetMouseButton(0);
        
        if (isFlying)
        {
            playerRigidbody.AddForce(new Vector2(0, flightPower));
        }

        Vector2 newVelocity = playerRigidbody.velocity;
        newVelocity.x = forwardMovementSpeed;
        playerRigidbody.velocity = newVelocity;
    }

    void CollectEnergy(Collider2D energyCollider)
    {
        magicEnergy++;
        Destroy(energyCollider.gameObject);
    }
}
