using UnityEngine;
using TMPro;

public class StartGameManager : MonoBehaviour
{
    public TextMeshProUGUI startText; // Text hiển thị "Press SPACE to Start"
    private bool gameStarted = false;

    void Start()
    {
        Time.timeScale = 0; // Dừng game khi bắt đầu
        startText.gameObject.SetActive(true); // Hiển thị dòng chữ
    }

    void Update()
    {
        if (!gameStarted && Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
    }

    void StartGame()
    {
        gameStarted = true;
        Time.timeScale = 1; // Chạy game
        startText.gameObject.SetActive(false); // Ẩn dòng chữ
    }
}
