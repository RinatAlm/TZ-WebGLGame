using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("SceneState")]
    public bool gameOver = false;
    [Space(10)]

    [Header("SceneSettings")]
    public float background1Speed;
    public float background2Speed;
    public float background3Speed;
    public float obstacleSpeed;
    public float spawnDelay;
    public float scoreUpdateRate = 0.25f;
    [Space(10)]

    [Header("SpawnManager")]
    public List<GameObject> obstaclePrefabs = new();//List of prefabs to spanw
    public int difficultyCounter;
    public Vector2 spawnPos;
    public float destroyPosX;
    [Space(10)]

    [Header("Refereces")]
    public MovingBackground background1;
    public MovingBackground background2;
    public MovingBackground background3;
    //Player ref
    public Text maxScoreText;
    public Text scoreText;

    private long  _maxScore;
    private long _currentScore;
    private void Awake()
    {
        instance = this;
        SetSettings();
    }

    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
        StartCoroutine(ScoreCoroutine());
    }
    public void GameOver()
    {
        gameOver = true;
        Debug.Log("GameOver");
    }
       
    private void SetSettings()
    {
        if(background1)
        background1.movingSpeed = background1Speed;
        if(background2)
        background2.movingSpeed = background2Speed;
        if(background3)
        background3.movingSpeed = background3Speed;
        maxScoreText.text = _maxScore.ToString();
        //SetPlayer
    }
    private IEnumerator SpawnCoroutine()//Spawner
    {
        while(!gameOver)//Delay before Spawn
        {
            SpawnObstacle();
            yield return new WaitForSeconds(Random.Range(spawnDelay, spawnDelay + 1));
        }    
    }

    private IEnumerator ScoreCoroutine()
    {
        while(!gameOver)
        {
            _currentScore += 1;
            scoreText.text = _currentScore.ToString();
            yield return new WaitForSeconds(scoreUpdateRate);
        }
    }
    private void SpawnObstacle()
    {
        if(obstaclePrefabs.Count!=0)
        {
            int index = Random.Range(0, difficultyCounter);
            MovingObstacle obstacle = Instantiate(obstaclePrefabs[index], spawnPos, Quaternion.identity).GetComponent<MovingObstacle>();
            obstacle.speed = obstacleSpeed;
        }     
    }

    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }

}
