using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public List<GameObject> targets;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI gameOverText;
    public Slider volumeSlider;
    public Button restartButton;
    public GameObject titleScreen;
    public GameObject pauseScreen;
    public GameObject blade;

    public int lives;
    private int score;
    private float spawnRate = 1.0f;

    public bool isGameActive;
    public bool isPaused;

    void Start()
    {
        
    }

    void Update()
    {
        if(lives == 0)
        {
            GameOver();
        }

        if(Input.GetKeyDown(KeyCode.Escape) && isGameActive == true)
        {
            isPaused = !isPaused;
            Debug.Log(isPaused);
            Time.timeScale = isPaused ? 0 : 1;
        }

        if(isPaused)
        {
            PauseScreenOn();
        }
        else
        {
            PauseScreenOff();
        }
    }

    IEnumerator SpawnTarget()
    {
        while(isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targets.Count);
            Instantiate(targets[index]);
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    public void UpdateLives()
    {
        if(lives > 0)
        {
            lives -= 1;
            livesText.text = "Lives: " + lives;
        }
    }

    public void StartGame(int difficulty)
    {
        spawnRate /= difficulty;
        isGameActive = true;
        score = 0;
        StartCoroutine(SpawnTarget());
        UpdateScore(0);
        titleScreen.gameObject.SetActive(false);
        volumeSlider.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        livesText.gameObject.SetActive(true);
        blade.SetActive(true);

        livesText.text = "Lives: " + lives;

        Button botonRestart = restartButton.GetComponent<Button>();
        botonRestart.onClick.AddListener(RestartGame);
    }

    public void GameOver()
    {
        isGameActive = false;
        blade.SetActive(false);
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseScreenOn()
    {
        blade.SetActive(false);
        pauseScreen.gameObject.SetActive(true);
    }

    public void PauseScreenOff()
    {
        blade.SetActive(true);
        pauseScreen.gameObject.SetActive(false);
    }
}
