using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TMP_Text scoreText; 
    private int score = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        scoreText.text = "Score : 0";
    }

    public void AddScore(int value)
    {
        score += value;
        scoreText.text = "Score : " + score;
        Debug.Log("AddPoint");
    }

    public void ResetScore()
    {
        score = 0;
        scoreText.text = "Score : " + score;
    }
}
