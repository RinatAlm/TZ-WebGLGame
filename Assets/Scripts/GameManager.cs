using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public struct Difficulty
    {
        public float background1Speed;
        public float background2Speed;
        public float background3Speed;
        public float obstacleSpeed;
        public float spawnDelay;
        public float scoreUpdateRate;
    };

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
    public List<int> scoreSwitch = new();

    public List<Difficulty> difficultySwitch = new();
    [Space(60)]

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
    public Text maxScoreText;
    public Text scoreText;
    public GameObject gameoverPanel;
    public PlayerController player;
    public Animator playerAnimator;

    [Header("Animations")]
    public Animator scoreTextAnimator;
    public float animationPlayTime;
   
    private long _currentScore;
    private bool isAnimationPlaying;
    
    private void Awake()
    {
        instance = this;
        SetSettings();
    }

    private void Start()
    {
        AudioManager.instance.Play("MainMusic");
        StartCoroutine(SpawnCoroutine());
        StartCoroutine(ScoreCoroutine());
    }

    private void FixedUpdate()
    {
        if(difficultyCounter < scoreSwitch.Count)//Change difficulty every point
        {
            if (_currentScore == scoreSwitch[difficultyCounter])
            {
                //Congratulate 
                Debug.Log("Score reached : " + _currentScore.ToString());
                AudioManager.instance.Play("Score");
                StartCoroutine(ScoreTextAnimation());
                difficultyCounter++;
                ChangeDifficulty();
            }
        }    
    }
    public void GameOver()
    {
        gameOver = true;
        gameoverPanel.SetActive(true);
        DisablePlayer();
        playerAnimator.SetTrigger("isDead");
        Debug.Log("GameOver");
        AudioManager.instance.Play("Dead");
        AudioManager.instance.Stop("MainMusic");
    }
       
    private void SetSettings()
    {
        //Disable gameOver panel
        gameoverPanel.SetActive(false);
        //Background settings
        if (background1)
        background1.movingSpeed = background1Speed;
        if(background2)
        background2.movingSpeed = background2Speed;
        if(background3)
        background3.movingSpeed = background3Speed;              
        
    }

    private void ChangeDifficulty()
    {
        Difficulty difficulty = difficultySwitch[difficultyCounter - 1];
        if (background1)
            background1.movingSpeed = difficulty.background1Speed;
        if (background2)
            background2.movingSpeed = difficulty.background2Speed;
        if (background3)
            background3.movingSpeed = difficulty.background3Speed;
        obstacleSpeed = difficulty.obstacleSpeed;
        spawnDelay = difficulty.spawnDelay;
        scoreUpdateRate = difficulty.scoreUpdateRate;
    }
    private IEnumerator SpawnCoroutine()//Spawner
    {
        while(!gameOver)//Delay before Spawn
        {
            SpawnObstacle();
            yield return new WaitForSeconds(Random.Range(spawnDelay, spawnDelay + 2));
        }    
    }

    private IEnumerator ScoreCoroutine()
    {
        while(!gameOver)
        {
            _currentScore += 1;
            if (!isAnimationPlaying)
            scoreText.text = "S : " + _currentScore.ToString();                   
            yield return new WaitForSeconds(scoreUpdateRate);
        }
    }
    private void SpawnObstacle()
    {
        if(obstaclePrefabs.Count!=0)
        {
            int index = Random.Range(0, 2);
            MovingObstacle obstacle = Instantiate(obstaclePrefabs[index], spawnPos, Quaternion.identity).GetComponent<MovingObstacle>();
            obstacle.speed = obstacleSpeed;
        }     
    }

    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
        AudioManager.instance.Play("Button");
    }

    private void DisablePlayer()
    {
        player.playerRigidBody.gravityScale = 0;
        player.playerRigidBody.velocity = Vector2.zero;
    }

    private IEnumerator ScoreTextAnimation()
    {
        isAnimationPlaying = true;
        scoreTextAnimator.SetBool("isTextAnimation", isAnimationPlaying);
        yield return new WaitForSeconds(animationPlayTime);
        isAnimationPlaying = false;
        scoreTextAnimator.SetBool("isTextAnimation", isAnimationPlaying);
        scoreText.text = _currentScore.ToString();
    }



    
}
