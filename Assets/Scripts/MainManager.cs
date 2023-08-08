using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText, BestScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points, b_Point = 0;
    private string playerName;
    private string path;

    private bool m_GameOver = false;

    [Serializable]
    class SaveData
    {
        public string playerName;
        public int b_Point;

        public SaveData(string playerName, int b_Point)
        {
            this.playerName = playerName;
            this.b_Point = b_Point;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        playerName = UIMenu.Instance.playerName;
        ScoreText.text = playerName + $" Score : {m_Points}";
        path = Application.persistentDataPath + "/best_score.json";

        LoadHighScore();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = UnityEngine.Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $" {playerName} : Score : {m_Points}";
        if (m_Points > b_Point)
        {
            BestScoreText.text = $"Best Score : {playerName} : {m_Points}";
        }
    }

    public void GameOver()
    {
        SaveHighScore();
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    private void LoadHighScore()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            b_Point = data.b_Point;
            BestScoreText.text = $"Best Score : {data.playerName} : {b_Point}";
        }
    }

    private void SaveHighScore()
    {
        if (m_Points > b_Point)
        {
            SaveData data = new(playerName, m_Points);
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(path, json);
        }
    }
}
