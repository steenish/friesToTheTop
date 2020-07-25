using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private float forceMultiplier;
    [SerializeField]
    private float torqueMultiplier;
    [SerializeField]
    private float speedDecay;
    [SerializeField]
    private float maxSpeed;

    private Rigidbody rb;
    private Vector2 movementInput;
    private float speed;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        // Get movement inputs
        movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    void FixedUpdate() {
        // Rotate player
        if (movementInput.y != 0f) {
            Quaternion rotation = rb.rotation;
            Vector3 eulerRotation = rotation.eulerAngles;
            eulerRotation.x += torqueMultiplier * movementInput.y;
            rotation.eulerAngles = eulerRotation;
            rb.rotation = rotation;
        }

        if (movementInput.x != 0f) {
            Quaternion rotation = rb.rotation;
            Vector3 eulerRotation = rotation.eulerAngles;
            eulerRotation.y += torqueMultiplier * movementInput.x;
            rotation.eulerAngles = eulerRotation;
            rb.rotation = rotation;
        }

        // Gliding physics
        //rb.AddForce(transform.up * forceMultiplier * (Mathf.Clamp((90f - transform.rotation.eulerAngles.x), 0, 90f) / 90f));
        Debug.Log(speed);
        speed += Mathf.Clamp(((transform.rotation.eulerAngles.x > 90f ? 0f : transform.rotation.eulerAngles.x) / 90f), 0f, 1f) * forceMultiplier;
        rb.AddForce(transform.forward * speed);
        speed -= speedDecay * (transform.rotation.eulerAngles.x > 90f ? Mathf.Clamp(360f - transform.rotation.eulerAngles.x, 1f, 90f) : 0f);
        speed = Mathf.Clamp(speed, 0f, maxSpeed);
    }
}
