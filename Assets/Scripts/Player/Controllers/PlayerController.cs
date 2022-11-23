using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
  [SerializeField]
  private float flightPower = 3.0f;

  [SerializeField]
  private float forwardMovementSpeed = 3.0f;

  [SerializeField]
  private float maxForwardSpeed = 15.0f;

  [SerializeField]
  private float speedIncrease = 0.1f;

  [SerializeField]
  private Rigidbody2D playerRigidbody;

  public GameObject gameOverCanvas;

  private uint magicEnergy = 0;

  public Text energyCollectedLabel;

  private bool isDead = false;

  private void Start()
  {
    playerRigidbody = GetComponent<Rigidbody2D>();
    Time.timeScale = 1;
    StartCoroutine(IncreaseFlyingSpeed());
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
    else
    {
      isDead = true;
      PauseGame();
      gameOverCanvas.SetActive(true);
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
    energyCollectedLabel.text = magicEnergy.ToString();
    Destroy(energyCollider.gameObject);

  }

  IEnumerator IncreaseFlyingSpeed()
  {
    for(;;)
    {
      if (forwardMovementSpeed <= maxForwardSpeed)
      {
        forwardMovementSpeed += speedIncrease;
      }
      yield return new WaitForSeconds(1.0f);
    }
    
  }

  void Restart()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

  void PauseGame()
  {
    Time.timeScale = 0;
  }
}
