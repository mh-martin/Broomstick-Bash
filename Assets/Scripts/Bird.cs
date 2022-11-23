using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public float flyingSpeed = -3.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      transform.position = new Vector2(transform.position.x + flyingSpeed * Time.deltaTime, transform.position.y);
    }
}
