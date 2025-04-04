using UnityEngine;

public class DonutController : MonoBehaviour
{
    public float startSpeed = 2f; 
    public float endSpeed = 4f; 
    public float fallDuration = 1f; // Time to reach maximum speed
    public bool isMoldy = false; 

    private float elapsedTime = 0f; 
    private LifeManager lifeManager;

    private void Start()
    {
        lifeManager = FindObjectOfType<LifeManager>();

        if (lifeManager == null)
        {
            Debug.LogError("LifeManager introuvable dans la scène !");
        }
    }

    private void Update()
    {
        // Calculate current falling speed as a function of elapsed time
        float fallSpeed = Mathf.Lerp(startSpeed, endSpeed, Mathf.Clamp01(elapsedTime / fallDuration));

        // Apply downward translation
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        elapsedTime += Time.deltaTime;

        // Destroy the object if its position is too low
        if (transform.position.y <= -1)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isMoldy)
            {
                Debug.Log("Donut moisi collecté. Perte d'une vie !");
                if (lifeManager != null)
                {
                    lifeManager.LoseLife(); 
                }

                SoundManager.instance.PlayBadDonutSound(); 
            }
            else
            {
                if (ScoreManager.instance != null)
                {
                    ScoreManager.instance.AddScore(1);
                    SoundManager.instance.PlayCollectSound(); 
                }
                else
                {
                    Debug.LogWarning("ScoreManager n'est pas défini.");
                }
            }

        }

        Destroy(gameObject);
    }
}
