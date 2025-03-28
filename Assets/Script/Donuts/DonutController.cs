using UnityEngine;

public class DonutController : MonoBehaviour
{
    public float startSpeed = 2f; // Vitesse initiale de chute
    public float endSpeed = 4f; // Vitesse maximale de chute
    public float fallDuration = 1f; // Temps pour atteindre la vitesse maximale
    public bool isMoldy = false; // Indique si le donut est moisi ou normal

    private float elapsedTime = 0f; // Temps écoulé depuis la création
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
        // Calculer la vitesse de chute actuelle en fonction du temps écoulé
        float fallSpeed = Mathf.Lerp(startSpeed, endSpeed, Mathf.Clamp01(elapsedTime / fallDuration));

        // Appliquer la translation vers le bas
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        // Incrémenter le temps écoulé
        elapsedTime += Time.deltaTime;

        // Détruire l'objet si sa position est trop basse
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

            Destroy(gameObject);
        }
    }
}
