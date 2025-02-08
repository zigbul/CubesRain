using System.Collections;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private CubePool _cubePool;
    [SerializeField] private float _spawnRangeX = 6f;
    [SerializeField] private float _spawnRangeZ = 6f;
    [SerializeField] private float _spawnHeight = 10f;
    [SerializeField] private float _spawnInterval = 0.5f;

    private bool _isWorking = true;

    private void Start()
    {
        StartCoroutine(SpawnCubes());
    }

    private IEnumerator SpawnCubes()
    {
        while(_isWorking)
        {
            yield return new WaitForSeconds(_spawnInterval);

            SpawnCube();
        }
    }

    private void SpawnCube()
    {
        CubeBehavior cube = _cubePool.GetCube();

        float randomX = Random.Range(-_spawnRangeX, _spawnRangeX);
        float randomZ = Random.Range(-_spawnRangeZ, _spawnRangeZ);
        Vector3 spawnPosition = new Vector3(randomX, _spawnHeight, randomZ);

        cube.transform.position = spawnPosition;
    }
}
