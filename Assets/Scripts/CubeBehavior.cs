using System.Collections;
using UnityEngine;

public class CubeBehavior : MonoBehaviour, IPoolable
{
    [SerializeField] private float _minLifeTime = 2f;
    [SerializeField] private float _maxLifeTime = 5f;
    [SerializeField] private Color _defaultColor = Color.white;

    private bool _hasTouchedPlatform = false;
    private Renderer _renderer;
    private CubePool _cubePool;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_hasTouchedPlatform && collision.gameObject.TryGetComponent<Platform>(out _))
        {
            _hasTouchedPlatform = true;

            _renderer.material.color = GetRandomColor();

            float lifeTime = Random.Range(_minLifeTime, _maxLifeTime);

            StartCoroutine(WaitAndReturn(lifeTime));
        }
    }

    private IEnumerator WaitAndReturn(float delay)
    {
        yield return new WaitForSeconds(delay);

        ReturnToPool();
    }

    private Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value);
    }

    public void ReturnToPool()
    {
        _hasTouchedPlatform = false;
        _renderer.material.color = _defaultColor;
        _cubePool.ReturnCube(this);
    }

    public void SetPool(CubePool pool)
    {
        _cubePool = pool;
    }
}
