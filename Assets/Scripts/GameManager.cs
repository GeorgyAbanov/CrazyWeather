using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    public enum AudioEventType { GoodPlayer, BadPlayer, GoodGround };

    public bool isGameRunning;
    public int score;
    public List<GameObject> itemsPrefabs;
    public List<GameObject> cloudsPrefabs;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    private GameObject playerObject;
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip goodPlayerSound;
    [SerializeField] private AudioClip badPlayerSound;
    [SerializeField] private AudioClip goodGroundSound;
    [SerializeField] private float volume = 1.0f;
    [SerializeField] private float timeLeft;
    //[SerializeField] private AudioClip badGroundSound;
    private AudioSource audioSource;


    private int spawnCloudSecsFrom = 1;
    private int spawnCloudSecsTo = 5;
    private float lastCloudSpawnTime;
    private int spawnItemSecsFrom = 1;
    private int spawnItemSecsTo = 5;
    private float lastItemSpawnTime;

    private float cloudMaxY = 124.49f;
    private float cloudMinY = 122.5f;
    private float cloudMinZ = 529f;
    private float cloudMaxZ = 547f;

    private PlayerScript player;
    private static bool wasGamePlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.Find("Player");
        player = playerObject.GetComponent<PlayerScript>();
        audioSource = GetComponent<AudioSource>();
        if (wasGamePlayed)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        wasGamePlayed = true;
        titleScreen.SetActive(false);
        isGameRunning = true;
        score = 0;
        lastCloudSpawnTime = 0;
        lastItemSpawnTime = Time.realtimeSinceStartup + spawnItemSecsTo;
        timeLeft = 100;
        UpdateScore(0);
        UpdateTimeLeft();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        isGameRunning = false;
    }

    public void UpdateTimeLeft()
    {
        int i = Mathf.FloorToInt(timeLeft);
        if (i<0)
        {
            i = 0;
        }
        timeText.SetText($"Time left: {i}");
    }

    public void UpdateScore(int count)
    {
        score += count;
        if (score < 0)
        {
            score = 0;
        }
        scoreText.SetText($"Score: {score}");
    }

    public void PlaySoundEvent(AudioEventType type)
    {
        switch (type)
        {
            case AudioEventType.BadPlayer:
                audioSource.PlayOneShot(badPlayerSound, volume);
                break;
            case AudioEventType.GoodPlayer:
                audioSource.PlayOneShot(goodPlayerSound, volume);
                break;
            case AudioEventType.GoodGround:
                audioSource.PlayOneShot(goodGroundSound, volume);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameRunning)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimeLeft();
            if (timeLeft <= 0)
            {
                GameOver();
                return;
            }
            if (lastItemSpawnTime < Time.realtimeSinceStartup)
            {
                if (SpawnItem())
                {
                    lastItemSpawnTime = Time.realtimeSinceStartup + Random.Range(spawnItemSecsFrom, spawnItemSecsTo);
                }
            }
            if (lastCloudSpawnTime < Time.realtimeSinceStartup)
            {
                SpawnCloud();
                lastCloudSpawnTime = Time.realtimeSinceStartup + Random.Range(spawnCloudSecsFrom, spawnCloudSecsTo);
            }
        }
    }

    void SpawnCloud()
    {
        int cloudIndex = Random.Range(0,cloudsPrefabs.Count);
        bool moveToLeft = Random.Range(0, 2) == 1;
        float z;
        if (moveToLeft)
        {
            z = cloudMaxZ;
        }
        else
        {
            z = cloudMinZ;
        }
        Vector3 startPos = new Vector3(playerObject.transform.position.x,Random.Range(cloudMinY,cloudMaxY),z);
        GameObject item = Instantiate(cloudsPrefabs[cloudIndex], startPos, cloudsPrefabs[cloudIndex].transform.rotation);
        CloudMove cloud = item.GetComponent<CloudMove>();
        cloud.moveToLeft = moveToLeft;
        cloud.minBoundZ = cloudMinZ;
        cloud.maxBoundZ = cloudMaxZ;
    }

    bool SpawnItem()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject[] clouds = GameObject.FindGameObjectsWithTag("Cloud");
            if (clouds.Length == 0)
            {
                return false;
            }
            int cloudIndex = Random.Range(0, clouds.Length);
            if (clouds[cloudIndex].transform.position.z < player.leftBound ||
                clouds[cloudIndex].transform.position.z > player.rightBound)
            {
                continue;
            }
            int itemIndex = Random.Range(0, itemsPrefabs.Count);
            GameObject item = Instantiate(itemsPrefabs[itemIndex], clouds[cloudIndex].transform.position, itemsPrefabs[itemIndex].transform.rotation);
            Rigidbody itemRb = item.GetComponent<Rigidbody>();
            itemRb.AddTorque(Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10), ForceMode.Impulse);
            return true;
        }
        return false;
    }

}
