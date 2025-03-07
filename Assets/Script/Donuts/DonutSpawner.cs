using UnityEngine;

public class DonutSpawner : MonoBehaviour
{
    public GameObject donutPrefab; // Préfabriqué du donut normal
    public GameObject moldyDonutPrefab; // Préfabriqué du donut moisi
    public GameObject spawnSurface; // Référence à la surface de spawn
    public float spawnInterval = 1.5f; // Augmenté légèrement pour espacer les spawns
    public float spawnHeight = 12f; // Hauteur à laquelle les donuts apparaissent
    public int numberOfDonutsPerSpawn = 3; // Réduit le nombre de donuts générés par spawn
    public float spawnSpacingFactor = 1.5f; // Facteur pour espacer les apparitions

    private float spawnRangeX;
    private float spawnRangeZ;
    private bool isSpawning = false; // Vérifie si le spawn est actif

    private void Start()
    {
        MeshRenderer surfaceRenderer = spawnSurface.GetComponent<MeshRenderer>();
        if (surfaceRenderer != null)
        {
            spawnRangeX = (surfaceRenderer.bounds.size.x / 2) * spawnSpacingFactor;
            spawnRangeZ = (surfaceRenderer.bounds.size.z / 2) * spawnSpacingFactor;
        }
        else
        {
            spawnRangeX = 15f;
            spawnRangeZ = 15f;
            Debug.LogWarning("Surface de spawn non trouvée ou sans MeshRenderer. Utilisation des valeurs par défaut.");
        }
    }

    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            InvokeRepeating(nameof(SpawnDonuts), spawnInterval, spawnInterval);
        }
    }

    public void StopSpawning()
    {
        if (isSpawning)
        {
            isSpawning = false;
            CancelInvoke(nameof(SpawnDonuts));
        }
    }

    private void SpawnDonuts()
    {
        if (!isSpawning) return;

        for (int i = 0; i < numberOfDonutsPerSpawn; i++)
        {
            SpawnSingleDonut();
        }
    }

    private void SpawnSingleDonut()
    {
        float randomX = Random.Range(-spawnRangeX, spawnRangeX) + spawnSurface.transform.position.x;
        float randomZ = Random.Range(-spawnRangeZ, spawnRangeZ) + spawnSurface.transform.position.z;
        Vector3 spawnPosition = new Vector3(randomX, spawnHeight, randomZ);

        GameObject donutToSpawn = Random.Range(0f, 1f) > 0.2f ? donutPrefab : moldyDonutPrefab;
        
        // Rotation aléatoire : X entre 0 et -90, Z entre -30 et 30
        float randomRotationX = Random.Range(-90f, 0f);
        float randomRotationZ = Random.Range(-30f, 30f);
        Quaternion randomRotation = Quaternion.Euler(randomRotationX, 0, randomRotationZ);

        Instantiate(donutToSpawn, spawnPosition, randomRotation);
    }
}
