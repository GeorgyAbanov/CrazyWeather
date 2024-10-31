using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMove : MonoBehaviour
{
    public bool moveToLeft;
    public float minBoundZ;
    public float maxBoundZ;
    private int cloudSpeed;
    private int cloudSpeedMin = 1;
    private int cloudSpeedMax = 3;
    // Start is called before the first frame update
    void Start()
    {
        cloudSpeed = Random.Range(cloudSpeedMin, cloudSpeedMax);
    }

    // Update is called once per frame
    void Update()
    {
        float offset = cloudSpeed * Time.deltaTime;
        if (moveToLeft)
        {
            offset = -offset;
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + offset);
        if (moveToLeft)
        {
            if (transform.position.z < minBoundZ)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (transform.position.z > maxBoundZ)
            {
                Destroy(gameObject);
            }
        }
    }
}
