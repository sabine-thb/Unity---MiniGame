using UnityEngine;

public class DonutSpawner : MonoBehaviour
{
    public GameObject donutPrefab;
    public GameObject spawnSurface; // Référence à la surface de spawn
    public float spawnInterval = 0.5f; // Temps entre chaque spawn
    public float spawnHeight = 10f; // Hauteur à laquelle les donuts apparaissent

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

        InvokeRepeating("SpawnDonut", spawnInterval, spawnInterval);
    }

    private void SpawnDonut()
    {
        // Position aléatoire de spawn sur l'axe X et Z, limitée par la surface de spawn
        float randomX = Random.Range(-spawnRangeX, spawnRangeX) + spawnSurface.transform.position.x;
        float randomZ = Random.Range(-spawnRangeZ, spawnRangeZ) + spawnSurface.transform.position.z;
        Vector3 spawnPosition = new Vector3(randomX, spawnHeight, randomZ);
        Instantiate(donutPrefab, spawnPosition, Quaternion.identity);
    }
}
