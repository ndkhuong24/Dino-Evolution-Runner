using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI currentTimeText; // Text hiển thị thời gian hiện tại
    public TextMeshProUGUI bestTimeText;    // Text hiển thị thời gian cao nhất

    private int currentTime = 0;
    private int bestTime = 0;
    private float timeElapsed = 0f;
    public float speedMultiplier = 5f; // Hệ số tăng tốc thời gian
    //private bool isPlaying = false;

    public GameObject[] grounds;
    public float speedScroller = 5f;
    private float groundWidth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Lấy thời gian cao nhất từ bộ nhớ(nếu có)
        bestTime = PlayerPrefs.GetInt("BestTime", 0);
        bestTimeText.text = bestTime.ToString("D5");

        //Lấy chiều rộng của ground
        groundWidth = grounds[0].GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        //if (isPlaying)
        //{
        timeElapsed += Time.deltaTime * speedMultiplier; // Thời gian chạy nhanh hơn
        currentTime = (int)timeElapsed; // Ép về số nguyên
        currentTimeText.text = currentTime.ToString("D5"); // Hiển thị dạng 
        //}

        MoveGround();
        speedScroller += Time.deltaTime * 0.1f;
    }

    private void MoveGround()
    {
        foreach (GameObject ground in grounds)
        {
            //Di chuyển sàn 
            ground.transform.position += Vector3.left * speedScroller * Time.deltaTime;

            //Nếu sàn trượt ra khỏi màn hình, đặt lại về vị trí phía sau
            if (ground.transform.position.x < -groundWidth)
            {
                float newX = ground.transform.position.x + groundWidth * grounds.Length;
                ground.transform.position = new Vector3(newX, ground.transform.position.y, ground.transform.position.z);
            }
        }
    }

    //public void StartGame()
    //{
    //    isPlaying = true;
    //    currentTime = 0;
    //}

    //public void EndGame()
    //{
    //    isPlaying = false;

    //    // Nếu thời gian hiện tại lớn hơn best time -> Cập nhật best time
    //    if (currentTime > bestTime)
    //    {
    //        bestTime = currentTime;
    //        PlayerPrefs.SetFloat("BestTime", bestTime);
    //        PlayerPrefs.Save();
    //        bestTimeText.text = "Best Time: " + bestTime.ToString("F2") + "s";
    //    }
    //}
}
