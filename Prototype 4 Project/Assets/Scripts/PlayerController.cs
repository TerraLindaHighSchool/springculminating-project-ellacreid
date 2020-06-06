using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private GameObject focalPoint;
    private float powerUpStrength = 15.0f;
    public float speed;
    public bool hasPowerUp;
    public bool hasPowerUpTwo;
    public GameObject powerupIndicator;
    public GameObject powerupIndicatorTwo;

    private int height = 5;
    private float floatForce = 25.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        powerupIndicatorTwo.transform.position = transform.position + new Vector3(0, -0.5f, 0);

        if (this.transform.position.y < height)
        {
            // While space is pressed and player is low enough, float up
            if (Input.GetKey(KeyCode.Space) && hasPowerUpTwo && this.transform.position.y < height)
            {
                playerRb.AddForce(Vector3.up * floatForce);
            }
        }
        else
        {
            transform.position = new Vector3(transform.position.x, height);
            playerRb.AddForce(-Vector3.up * floatForce / 10);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerUp = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerUpCountDownRoutine());
            powerupIndicator.gameObject.SetActive(true);
        }

        if (other.CompareTag("Powerup2"))
        {
            hasPowerUpTwo = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerUpCountDownRoutineTwo());
            powerupIndicatorTwo.gameObject.SetActive(true);
        }
    }

    IEnumerator PowerUpCountDownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerUp = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    IEnumerator PowerUpCountDownRoutineTwo()
    {
        yield return new WaitForSeconds(10);
        hasPowerUpTwo = false;
        powerupIndicatorTwo.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && hasPowerUp)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

            enemyRigidbody.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse);
            Debug.Log("Player collided with " + collision.gameObject + " with powerup set to " + hasPowerUp);
        }
    }
}
