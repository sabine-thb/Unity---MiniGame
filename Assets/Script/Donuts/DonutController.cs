using UnityEngine;

public class DonutController : MonoBehaviour
{
    public float startSpeed = 2f; // Vitesse initiale de chute
    public float endSpeed = 4f; // Vitesse maximale de chute
    public float fallDuration = 1f; // Temps pour atteindre la vitesse maximale
    public bool isMoldy = false; // Indique si le donut est moisi ou normal

    private float elapsedTime = 0f; // Temps écoulé depuis la création

    private void Update()
    {
        // Calculer la vitesse de chute actuelle en fonction du temps écoulé
        float fallSpeed = Mathf.Lerp(startSpeed, endSpeed, Mathf.Clamp01(elapsedTime / fallDuration));

        // Appliquer la translation vers le bas
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        // Incrémenter le temps écoulé
        elapsedTime += Time.deltaTime;

        // Détruire l'objet si sa position est trop basse
        if (transform.position.y <= -2)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Ajouter des points seulement si le donut n'est pas moisi
            if (isMoldy)
            {
                Debug.Log("Donut moisi collecté. Pas de points.");
            }
            else
            {
                // Ajouter des points au joueur pour un donut normal
                if (ScoreManager.instance != null)
                {
                    ScoreManager.instance.AddScore(1);
                }
                else
                {
                    Debug.LogWarning("ScoreManager n'est pas défini.");
                }
            }

            // Détruire le donut après qu'il ait été collecté
            Destroy(gameObject);
        }
    }
}
