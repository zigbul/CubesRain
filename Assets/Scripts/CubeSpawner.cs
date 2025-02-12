using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Cube _cube;
    [SerializeField] private float _spawnRangeX = 6f;
    [SerializeField] private float _spawnRangeZ = 6f;
    [SerializeField] private float _spawnHeight = 10f;
    [SerializeField] private float _spawnInterval = 0.5f;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 20;

    private bool _isWorking = true;
    private WaitForSeconds _waitTime;
    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
        createFunc: () => Instantiate(_cube),
        actionOnGet: (cube) => GetCubeFromPool(cube),
        actionOnRelease: (cube) => cube.gameObject.SetActive(false),
        actionOnDestroy: (cube) => Destroy(cube.gameObject),
        collectionCheck: true,
        defaultCapacity: _poolCapacity,
        maxSize: _poolMaxSize);

        _waitTime = new WaitForSeconds(_spawnInterval);
    }

    private void Start()
    {
        StartCoroutine(SpawnCubes());
    }

    private void GetCubeFromPool(Cube cube)
    {
        float randomX = Random.Range(-_spawnRangeX, _spawnRangeX);
        float randomZ = Random.Range(-_spawnRangeZ, _spawnRangeZ);
        Vector3 spawnPosition = new Vector3(randomX, _spawnHeight, randomZ);

        cube.transform.position = spawnPosition;
        cube.gameObject.SetActive(true);
        cube.OnReturnToPool += ReturnCubeToPool;
    }

    private IEnumerator SpawnCubes()
    {
        while(_isWorking)
        {
            yield return _waitTime;

            SpawnCube();
        }
    }

    private void SpawnCube()
    {
        _pool.Get();
    }

    private void ReturnCubeToPool(Cube cube)
    {
        cube.OnReturnToPool -= ReturnCubeToPool;
        _pool.Release(cube);
    }
}
