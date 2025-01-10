using UnityEngine;

public class DonutController : MonoBehaviour
{
    public float fallSpeed = 2f; // Vitesse de chute des donuts

    private void Update()
    {
        // Faire tomber le donut
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        // Détruire le donut quand il atteint y=0
        if (transform.position.y <= -2)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.instance.AddScore(1);
            Destroy(gameObject);
        }
    }
}
