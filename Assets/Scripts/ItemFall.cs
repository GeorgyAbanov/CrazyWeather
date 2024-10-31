using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFall : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerScript playerScript;
    public bool isGood;
    [SerializeField] private ParticleSystem goodPlayerParticle;
    [SerializeField] private ParticleSystem badPlayerParticle;
    [SerializeField] private ParticleSystem goodGroundParticle;
    [SerializeField] private ParticleSystem badGroundParticle;

    Rigidbody itemRb;

    private float gMoon = 7f;  

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();
        itemRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        itemRb.AddForce(Vector3.up  * itemRb.mass * gMoon);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            //Debug.Log("Collision with ground "+isGood);
            if (isGood)
            {
                gameManager.PlaySoundEvent(GameManager.AudioEventType.GoodGround);
                gameManager.UpdateScore(0);
            }
            else
            {

            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Collision with player " + isGood);
            if (isGood)
            {
                gameManager.PlaySoundEvent(GameManager.AudioEventType.GoodPlayer);
                gameManager.UpdateScore(1);
            }
            else
            {
                playerScript.PlayCarExplosion();
                gameManager.PlaySoundEvent(GameManager.AudioEventType.BadPlayer);
                gameManager.UpdateScore(-3);
            }
            Destroy(gameObject);
        }
    }

}
