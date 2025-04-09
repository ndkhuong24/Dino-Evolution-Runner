using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI currentTimeText;
    public TextMeshProUGUI bestTimeText;
    public TextMeshProUGUI messageText;

    [Header("Game Settings")]
    private float timeElapsed = 0f;
    private int bestTime = 0;

    [Header("Speed Settings")]
    public float globalSpeed = 5f;
    public float speedIncreaseRate = 0.15f;
    private float maxSpeed = 15f;

    [Header("Ground Settings")]
    public GameObject[] grounds;
    private float groundWidth;

    private bool isGameStarted = false;
    private bool isGameOver = false;

    void Start() => InitializeGame();

    void Update()
    {
        if (!isGameStarted && Input.GetKeyDown(KeyCode.Space))
            StartGame();
        else if (isGameOver && Input.GetKeyDown(KeyCode.Space))
            RestartGame();

        if (isGameStarted && !isGameOver)
        {
            UpdateTimer();
            MoveGround();
            globalSpeed = Mathf.Min(globalSpeed + Time.deltaTime * speedIncreaseRate, maxSpeed);  // ✅ Đồng bộ tăng tốc
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            RestartGame();
        }

    }

    private void InitializeGame()
    {
        Time.timeScale = 0;
        messageText.text = "Press SPACE to Start";

        bestTime = PlayerPrefs.GetInt("BestTime", 0);
        bestTimeText.text = bestTime.ToString("D5");

        if (grounds.Length > 0) groundWidth = grounds[0].GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void UpdateTimer()
    {
        timeElapsed += Time.deltaTime;
        currentTimeText.text = ((int)timeElapsed).ToString("D5");
    }

    private void StartGame()
    {
        isGameStarted = true;
        isGameOver = false;
        messageText.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        globalSpeed = 5f;
        timeElapsed = 0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void MoveGround()
    {
        foreach (GameObject ground in grounds)
        {
            ground.transform.position += Vector3.left * globalSpeed * Time.deltaTime;

            if (ground.transform.position.x <= -groundWidth)
            {
                float newX = ground.transform.position.x + groundWidth * grounds.Length;
                ground.transform.position = new Vector3(newX, ground.transform.position.y, ground.transform.position.z);
            }
        }
    }

    public void EndGame()
    {
        isGameOver = true;
        Time.timeScale = 0f;
        messageText.text = "Press SPACE to Restart";
        messageText.gameObject.SetActive(true);

        int finalTime = (int)timeElapsed;
        if (finalTime > bestTime)
        {
            bestTime = finalTime;
            PlayerPrefs.SetInt("BestTime", bestTime);
            PlayerPrefs.Save();
            bestTimeText.text = bestTime.ToString("D5");
        }
    }
}