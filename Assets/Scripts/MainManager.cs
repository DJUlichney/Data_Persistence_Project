using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text topScoreText;
    public GameObject GameOverText;
    private string playerName;
    private string topPlayerName;
    
    private bool m_Started = false;
    private int m_Points;
    private int topScore;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        playerName = MenuManager.playerName;

        // Loads top score data and initializes scoreboards.
        LoadTopScore();
        topScoreText.text = "Best Score : " + topPlayerName + " : " + topScore;
        ScoreText.text = playerName + "'s Score : " + m_Points;

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
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
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
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = playerName + "'s Score : " + m_Points;

        // Checks for a new high score and updates accordingly.
        if (topScore < m_Points){
            UpdateTopScore();
        }
    }

    void UpdateTopScore(){
        topScore = m_Points;
        topPlayerName = playerName;
        topScoreText.text = "Best Score : " + topPlayerName + " : " + topScore;
    }

    public void GameOver()
    {
        SaveTopScore();
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    // Establishes a class for storing top score information.
    [System.Serializable] class TopScoreData{
        public string topPlayerName;
        public int topScore;
    }    

    private void LoadTopScore(){
        string path = Application.persistentDataPath + "/topscore.json";
        if(File.Exists(path)){
            string json = File.ReadAllText(path);
            TopScoreData data = JsonUtility.FromJson<TopScoreData>(json);

            topPlayerName = data.topPlayerName;
            topScore = data.topScore;
        }
    }

    private void SaveTopScore(){
        TopScoreData data = new TopScoreData();
        data.topPlayerName = topPlayerName;
        data.topScore = topScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/topscore.json", json);
    }
}
