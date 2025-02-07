using UnityEngine;

public class DonutSpawner : MonoBehaviour
{
    public GameObject donutPrefab; // Préfabriqué du donut normal
    public GameObject moldyDonutPrefab; // Préfabriqué du donut moisi
    public GameObject spawnSurface; // Référence à la surface de spawn
    public float spawnInterval = 1f; // Temps entre chaque spawn
    public float spawnHeight = 12f; // Hauteur à laquelle les donuts apparaissent
    public int numberOfDonutsPerSpawn = 5; // Nombre de donuts générés à chaque spawn

    private float spawnRangeX;
    private float spawnRangeZ;

    private void Start()
    {
        // Calculer les limites de spawn en fonction de la surface
        MeshRenderer surfaceRenderer = spawnSurface.GetComponent<MeshRenderer>();
        if (surfaceRenderer != null)
        {
            spawnRangeX = surfaceRenderer.bounds.size.x / 2;
            spawnRangeZ = surfaceRenderer.bounds.size.z / 2;
        }
        else
        {
            spawnRangeX = 10f;
            spawnRangeZ = 10f;
            Debug.LogWarning("Surface de spawn non trouvée ou sans MeshRenderer. Utilisation des valeurs par défaut.");
        }

        // Lancer la génération de donuts à intervalles réguliers
        InvokeRepeating("SpawnDonuts", spawnInterval, spawnInterval);
    }

    private void SpawnDonuts()
    {
        // Générer plusieurs donuts à chaque spawn
        for (int i = 0; i < numberOfDonutsPerSpawn; i++)
        {
            SpawnSingleDonut();
        }
    }

    private void SpawnSingleDonut()
    {
        // Position aléatoire de spawn sur l'axe X et Z, limitée par la surface de spawn
        float randomX = Random.Range(-spawnRangeX, spawnRangeX) + spawnSurface.transform.position.x;
        float randomZ = Random.Range(-spawnRangeZ, spawnRangeZ) + spawnSurface.transform.position.z;
        Vector3 spawnPosition = new Vector3(randomX, spawnHeight, randomZ);

        // Choisir aléatoirement si c'est un donut normal ou moisi
        GameObject donutToSpawn = Random.Range(0f, 1f) > 0.2f ? donutPrefab : moldyDonutPrefab;

        // Instancier le donut à la position calculée
        Instantiate(donutToSpawn, spawnPosition, Quaternion.identity);
    }
}
