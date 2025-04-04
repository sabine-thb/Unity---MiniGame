using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LifeManager : MonoBehaviour
{
    public int lives = 3;
    public Image[] lifeIcons;
    public UnityEvent OnGameOver;

    public UnityEvent OnReplay;

    public PlayerController playerController;


    public void LoseLife()
    {
        if (lives > 0)
        {
            lives--; 

            lifeIcons[lives].enabled = false; 

            if (lives == 0)
            {
                GameOver();
            }
        }
    }

    

    private void RestoreLives()
    {
        lives = 3;

        foreach (Image icon in lifeIcons)
        {
            icon.enabled = true;
        }

        Debug.Log("Lives restored !");
    }  
    
     

    private void GameOver()
    {

        OnGameOver?.Invoke();

        foreach (GameObject donut in GameObject.FindGameObjectsWithTag("donut"))
        {
            Destroy(donut);
        }

        SoundManager.instance.PlayGameOverSound();


        Debug.Log("Game Over !");
    }


    public void Replay()
    {
        RestoreLives();

        foreach (GameObject donut in GameObject.FindGameObjectsWithTag("donut"))
        {
            Destroy(donut);
        }

         if (playerController != null)
        {
            playerController.Respawn();
        }
        else
        {
            Debug.LogError("PlayerController n'est pas assigné dans l'inspecteur !");
        }

        ScoreManager.instance.ResetScore();
        OnReplay?.Invoke();

        Debug.Log("Replay !");
    }
}
