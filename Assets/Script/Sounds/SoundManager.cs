using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource gameOver;
    public AudioSource nextLevel;
    public AudioSource collectSource; 
    public AudioClip[] collectClips; 

    public AudioSource badDonut;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void PlayGameOverSound()
    {
        gameOver.Play();
    }

    public void PlayNextLevelSound()
    {
        nextLevel.Play();
    }

    public void PlayCollectSound()
    {
        if (collectClips.Length > 0)
        {
            int randomIndex = Random.Range(0, collectClips.Length);
            collectSource.clip = collectClips[randomIndex]; // Assigner un clip aléatoire
            collectSource.Play();
        }
    }

    public void PlayBadDonutSound()
    {
        badDonut.Play();
    }
}
