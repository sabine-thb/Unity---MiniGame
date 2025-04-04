using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; 
using UnityEngine.Events; 

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TMP_Text scoreText; 
    private int score = 0;
    public UnityEvent onWin;

    public UnityEvent onFinishLevel;

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
        scoreText.text = "Score : 0 / 10";

         if (onWin == null)
            onWin = new UnityEvent();

        if (onFinishLevel == null)
            onFinishLevel = new UnityEvent();
    }

     public void AddScore(int value)
    {
        score += value;
        scoreText.text = "Score : " + score + " / 10";
        Debug.Log("AddPoint");

        if (score >= 1)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;

            if (currentSceneName == "Level 1" || currentSceneName == "Level 2")
            {
                onFinishLevel.Invoke();
                foreach (GameObject donut in GameObject.FindGameObjectsWithTag("donut"))
                {
                    Destroy(donut);
                }
                Debug.Log("Vous avez terminé le niveau !"); 
            }
            else if (currentSceneName == "Level 3")
            {
                foreach (GameObject donut in GameObject.FindGameObjectsWithTag("donut"))
                {
                    Destroy(donut);
                }
                onWin.Invoke();
                Debug.Log("Vous avez terminé tous les niveaux !");
            }
        }
    }

    public void ResetScore()
    {
        score = 0;
        scoreText.text = "Score : " + score+ " / 10";
    }

    
}
