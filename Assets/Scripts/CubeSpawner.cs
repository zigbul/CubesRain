using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _cube;
    [SerializeField] private float _spawnRangeX = 6f;
    [SerializeField] private float _spawnRangeZ = 6f;
    [SerializeField] private float _spawnHeight = 10f;
    [SerializeField] private float _spawnInterval = 0.5f;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 20;

    private bool _isWorking = true;
    private WaitForSeconds _waitTime;
    private ObjectPool<GameObject> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
        createFunc: () => CreateCube(),
        actionOnGet: (obj) => ActionOnGet(obj),
        actionOnRelease: (obj) => obj.SetActive(false),
        actionOnDestroy: (obj) => Destroy(obj),
        collectionCheck: true,
        defaultCapacity: _poolCapacity,
        maxSize: _poolMaxSize);

        _waitTime = new WaitForSeconds(_spawnInterval);
    }

    private void ActionOnGet(GameObject cube)
    {
        float randomX = Random.Range(-_spawnRangeX, _spawnRangeX);
        float randomZ = Random.Range(-_spawnRangeZ, _spawnRangeZ);
        Vector3 spawnPosition = new Vector3(randomX, _spawnHeight, randomZ);

        cube.transform.position = spawnPosition;
        cube.SetActive(true);
    }

    private void Start()
    {
        StartCoroutine(SpawnCubes());
    }

    private IEnumerator SpawnCubes()
    {
        while(_isWorking)
        {
            yield return _waitTime;

            SpawnCube();
        }
    }

    private GameObject CreateCube()
    {
        GameObject cube = Instantiate(_cube);
        cube.GetComponent<Cube>().SetReturnAction(ReturnCubeToPool);

        return cube;
    }

    private void SpawnCube()
    {
        _pool.Get();
    }

    private void ReturnCubeToPool(GameObject cube)
    {
        _pool.Release(cube);
    }
}
