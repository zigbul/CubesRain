using UnityEngine;

public class CubeBehavior : MonoBehaviour
{
    [SerializeField] private float _minLifeTime = 2f;
    [SerializeField] private float _maxLifeTime = 5f;
    [SerializeField] private Color _defaultColor = Color.white;

    private bool _hasTouchedPlatform = false;
    private Renderer _renderer;
    private string _collisionObjectTag = "Platform";
    private CubePool _cubePool;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _cubePool = FindObjectOfType<CubePool>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_hasTouchedPlatform && collision.gameObject.CompareTag(_collisionObjectTag))
        {
            _hasTouchedPlatform = true;

            _renderer.material.color = GetRandomColor();

            float lifeTime = Random.Range(_minLifeTime, _maxLifeTime);

            Invoke(nameof(ReturnToPool), lifeTime);
        }
    }

    private Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value);
    }

    private void ReturnToPool()
    {
        _hasTouchedPlatform = false;
        _renderer.material.color = _defaultColor;
        _cubePool.ReturnCube(gameObject);
    }
}
