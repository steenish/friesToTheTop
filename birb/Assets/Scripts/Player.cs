using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float forceMultiplier;

    private Rigidbody rb;
    private Vector2 input;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Gliding physics
        rb.AddForce(transform.up * forceMultiplier * (Mathf.Clamp((90f - transform.rotation.eulerAngles.x), 0, 90f) / 90f));
        rb.AddForce(transform.forward * forceMultiplier * Mathf.Clamp((transform.rotation.eulerAngles.x / 90f), 0, 1));
    }
}
