using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {

#pragma warning disable
    [SerializeField]
    private float flyForceMultiplier;
    [SerializeField]
    private float flapForceMultiplier;
    [SerializeField]
    private float windForceMultiplier;
    [SerializeField]
    private float torqueMultiplier;
    [SerializeField]
    private float speedDecay;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Text flapsText;
    [SerializeField]
    private Text currentScoreText;
    [SerializeField]
    private float gameOverDelay;
#pragma warning restore

    private bool paused = false;
    private Rigidbody rb;
    private Vector2 movementInput;
    private float speed;
    private bool flapping = false;
    private int maxFlaps = 1;
    private int flaps = 1;
    private int currentScore;

    void Start() {
        rb = GetComponent<Rigidbody>();
        speed = 25f;
        currentScore = (int) transform.position.y;
        if (currentScoreText != null) {
            currentScoreText.text = "Current score: " + currentScore.ToString();
        }
    }

    void Update() {
        if (paused) return;

        // Get movement inputs
        movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // Flap wings input
        if (Input.GetButtonDown("Jump")) {
            flapping = true;
        }

        // Update highscore
        if (transform.position.y > currentScore) {
            currentScore = (int) transform.position.y;
            if (currentScoreText != null) {
                currentScoreText.text = "Current score: " + currentScore.ToString();
            }
            Debug.Log("New score: " + currentScore.ToString());
        }
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
        //Debug.Log(speed);
        speed += Mathf.Clamp(((transform.rotation.eulerAngles.x > 90f ? 0f : transform.rotation.eulerAngles.x + 1f) / 90f), 0f, 1f) * flyForceMultiplier;
        rb.AddForce(transform.forward * speed);
        speed -= speedDecay * (transform.rotation.eulerAngles.x > 90f ? Mathf.Clamp(360f - transform.rotation.eulerAngles.x, 1f, 90f) : 0f);
        speed = Mathf.Clamp(speed, 0f, maxSpeed);
        if (speed < 5f) {
            rb.AddForce(transform.up * -2f * flyForceMultiplier);
        }

        // Flap wings physics
        if (flapping) {
            if (flaps > 0) {
                animator.SetTrigger("FlyFlap");
                rb.AddForce(Vector3.up * flapForceMultiplier);
                rb.AddForce(transform.forward * flapForceMultiplier / 4);
                flaps--;
                if (flapsText != null) {
                    flapsText.text = "Flaps: " + flaps.ToString();
                }
            }
            flapping = false;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Fries") {
            flaps = maxFlaps;
            flapsText.text = "Flaps: " + flaps.ToString();
            Destroy(other.gameObject);
        } else if (other.CompareTag("Fog")) {
            StartCoroutine(GameOverSequence(0.0f));
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Building")) {
            StartCoroutine(GameOverSequence(gameOverDelay));
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.tag == "Wind") {
            rb.AddForce(Vector3.up * windForceMultiplier);
        }
    }

    private IEnumerator GameOverSequence(float delay) {
        paused = true;

        // Update scores.
        PlayerPrefs.SetInt("latestScore", currentScore);
        if ((PlayerPrefs.HasKey("highscore") && currentScore > PlayerPrefs.GetInt("highscore")) ||
            !PlayerPrefs.HasKey("highscore")) {
            PlayerPrefs.SetInt("highscore", currentScore);
        }

        // Wait then load game over scene.
        yield return new WaitForSecondsRealtime(delay);
        SceneManager.LoadScene("GameOverScene");
    }
}
