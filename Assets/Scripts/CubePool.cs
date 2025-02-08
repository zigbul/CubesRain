using System.Collections.Generic;
using UnityEngine;

public class CubePool : MonoBehaviour
{
    [SerializeField] CubeBehavior _cubePrefab;
    [SerializeField] private int _poolSize = 20;

    private Queue<CubeBehavior> _pool = new();

    private void Awake()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            CubeBehavior cube = Instantiate(_cubePrefab);
            cube.SetPool(this);
            cube.gameObject.SetActive(false);
            _pool.Enqueue(cube);
        }
    }

    public CubeBehavior GetCube()
    {
        if (_pool.Count > 0)
        {
            CubeBehavior cube = _pool.Dequeue();
            cube.gameObject.SetActive(true);
            return cube;
        } 
        else
        {
            CubeBehavior newCube = Instantiate (_cubePrefab);
            newCube.SetPool(this);
            return newCube;
        }
    }

    public void ReturnCube(CubeBehavior cube)
    {
        cube.gameObject.SetActive(false);
        _pool.Enqueue(cube);
    }
}
