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
    private int currentTime = 0;
    private int bestTime = 0;
    private float timeElapsed = 0f;

    [Header("Scrolling Settings")]
    public float speedScroller = 5f;
    public float speedIncreaseRate = 0.15f;
    private float maxSpeed = 15f;

    [Header("Ground Settings")]
    public GameObject[] grounds;
    private float groundWidth;

    private bool isGameStarted = false;
    private bool isGameOver = false;

    void Start()
    {
        Time.timeScale = 0; // Dừng game khi bắt đầu
        messageText.text = "Press SPACE to Start"; // Hiển thị hướng dẫn bắt đầu

        bestTime = PlayerPrefs.GetInt("BestTime", 0);
        bestTimeText.text = bestTime.ToString("D5");

        // ✅ Kiểm tra mảng grounds để tránh lỗi NullReferenceException
        if (grounds.Length > 0)
        {
            groundWidth = grounds[0].GetComponent<SpriteRenderer>().bounds.size.x;
        }
        else
        {
            Debug.LogError("⚠ Lỗi: Mảng 'grounds' rỗng hoặc chưa được gán trong Inspector!");
        }
        //groundWidth = grounds[0].GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        if (!isGameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartGame();
            }
            return;
        }

        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RestartGame();
            }
            return;
        }

        UpdateTimer();
        MoveGround();

        // 🚀 Tăng tốc nhưng không vượt quá maxSpeed & dừng khi game over
        if (!isGameOver)
        {
            speedScroller = Mathf.Min(speedScroller + Time.deltaTime * speedIncreaseRate, maxSpeed);
        }
    }

    private void UpdateTimer()
    {
        timeElapsed += Time.deltaTime;
        currentTime = (int)timeElapsed;
        currentTimeText.text = currentTime.ToString("D5");
    }

    private void StartGame()
    {
        isGameStarted = true;
        isGameOver = false;
        messageText.gameObject.SetActive(false); // Ẩn thông báo khi game bắt đầu
        Time.timeScale = 1f; // Chạy game
    }

    public void RestartGame()
    {
        if (isGameOver)
        {
            speedScroller = 5f; // 🔄 Chỉ reset tốc độ khi game đã kết thúc
        }

        timeElapsed = 0f; // 🕒 Reset thời gian về 0
        currentTime = 0;  // ✅ Reset thời gian hiện tại để tránh hiển thị sai
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //private void MoveObstacles()
    //{
    //    foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
    //    {
    //        obstacle.transform.position += Vector3.left * speedScroller * Time.deltaTime;

    //        if (obstacle.transform.position.x < -10f)
    //        {
    //            Destroy(obstacle);
    //        }
    //    }
    //}

    private void MoveGround()
    {
        foreach (GameObject ground in grounds)
        {
            ground.transform.position += Vector3.left * speedScroller * Time.deltaTime;

            if (ground.transform.position.x < -groundWidth)
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

        if (currentTime > bestTime)
        {
            bestTime = currentTime;
            PlayerPrefs.SetInt("BestTime", bestTime);
            PlayerPrefs.Save();
            bestTimeText.text = bestTime.ToString("D5");
        }
    }
}
