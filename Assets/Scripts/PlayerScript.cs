using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody playerRb;
    private GameManager gameManager;
    [SerializeField] float carForce = 30;
    public float leftBound = 532.45f;
    public float rightBound = 544.42f;

    [SerializeField] private ParticleSystem badPlayerParticle;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public void PlayCarExplosion()
    {
        badPlayerParticle.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gameManager.isGameRunning)
        {
            return;
        }
        float horizontal = Input.GetAxis("Horizontal");
        //playerRb.AddForce(Vector3.forward * carForce * horizontal);
        transform.transform.position = new Vector3(transform.transform.position.x, transform.transform.position.y, transform.transform.position.z + carForce * horizontal * Time.deltaTime);
        if (transform.position.z<leftBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y,leftBound);
            playerRb.velocity = Vector3.zero;
        }
        else if (transform.position.z > rightBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, rightBound);
            playerRb.velocity = Vector3.zero;
        }
    }
}
