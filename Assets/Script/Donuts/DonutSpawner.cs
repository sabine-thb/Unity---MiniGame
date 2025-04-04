using UnityEngine;

public class DonutSpawner : MonoBehaviour

{
    public GameObject donutPrefab;
    public GameObject moldyDonutPrefab; 
    public GameObject spawnSurface; 
    public float spawnInterval = 3f; // time between spawns
    public float spawnHeight = 12f; // Height at which donuts appear
    public int numberOfDonutsPerSpawn = 1; 
    public float spawnSpacingFactor = 1f; // Facteur pour espacer les apparitions

    private float spawnRangeX;
    private float spawnRangeZ;
    private bool isSpawning = false; 


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
        
        // Random donut rotation: X between 0 and -90, Z between -30 and 30
        float randomRotationX = Random.Range(-90f, 0f);
        float randomRotationZ = Random.Range(-30f, 30f);
        Quaternion randomRotation = Quaternion.Euler(randomRotationX, 0, randomRotationZ);

        Instantiate(donutToSpawn, spawnPosition, randomRotation);
    }
}
