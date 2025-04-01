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
    }

    public void AddScore(int value)
    {
        score += value;
        scoreText.text = "Score : " + score + " / 10";
        Debug.Log("AddPoint");

         if (score >= 1)
        {
            LoadNextLevel();
        }
    }

    public void ResetScore()
    {
        score = 0;
        scoreText.text = "Score : " + score+ " / 10";
    }

    private void LoadNextLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Déterminer la scène suivante
        string nextSceneName = "";

        if (currentSceneName == "Level 1")
        {
            nextSceneName = "Level 2";
        }
        else if (currentSceneName == "Level 2")
        {
            nextSceneName = "Level 3";
        }
        else if (currentSceneName == "Level 3")
        {
       
            {
                onWin.Invoke();
                Debug.Log("Vous avez terminé tous les niveaux !");
            }
            return;
        }
        else
        {
            Debug.LogError("Nom de scène inconnu !");
            return;
        }

            // Charger la scène suivante
            SceneManager.LoadScene(nextSceneName);
        }
}
