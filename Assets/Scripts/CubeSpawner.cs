using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private CubePool _cubePool;
    [SerializeField] private float _spawnRangeX = 6f;
    [SerializeField] private float _spawnRangeZ = 6f;
    [SerializeField] private float _spawnHeight = 10f;
    [SerializeField] private float _startingTime = 0f;
    [SerializeField] private float _spawnInterval = 0.5f;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnCube), _startingTime, _spawnInterval);
    }

    private void SpawnCube()
    {
        GameObject cube = _cubePool.GetCube();

        float randomX = Random.Range(-_spawnRangeX, _spawnRangeX);
        float randomZ = Random.Range(-_spawnRangeZ, _spawnRangeZ);
        Vector3 spawnPosition = new Vector3(randomX, _spawnHeight, randomZ);

        cube.transform.position = spawnPosition;
    }
}
