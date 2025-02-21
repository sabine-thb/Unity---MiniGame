using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LifeManager : MonoBehaviour
{
    public int lives = 3;
    public Image[] lifeIcons;
    public UnityEvent OnGameOver;


    // void Start()
    // {
    //      GameObject[] donuts = GameObject.FindGameObjectsWithTag("donut");
    //      Debug.Log("Donuts trouvés : " + donuts.Length);
    // }

    // void Update()
    // {
    //     GameObject[] donuts = GameObject.FindGameObjectsWithTag("donut");
    //      Debug.Log("Donuts trouvés : " + donuts.Length);
    // }


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

    

    public void RestoreLives()
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

        RestoreLives();

        ScoreManager.instance.ResetScore();

        Debug.Log("Game Over !");
    }
}