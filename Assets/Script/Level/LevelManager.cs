using UnityEngine;
using UnityEngine.SceneManagement; 

public class LevelManager : MonoBehaviour
{
    public void LoadNextLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        string nextSceneName = "";

        if (currentSceneName == "Level 1")
        {
            nextSceneName = "Level 2";
        }
        else if (currentSceneName == "Level 2")
        {
            nextSceneName = "Level 3";
        }
        else
        {
            Debug.LogError("Nom de scène inconnu !");
            return;
        }

            SceneManager.LoadScene(nextSceneName);
    }


    public void RestartGame()
    {
        string firstLevelScene = "Level 1";

        SceneManager.LoadScene(firstLevelScene);
    }


}