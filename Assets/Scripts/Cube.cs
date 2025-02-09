using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Cube : MonoBehaviour, IPoolable
{
    [SerializeField] private float _minLifeTime = 2f;
    [SerializeField] private float _maxLifeTime = 5f;
    [SerializeField] private Color _defaultColor = Color.white;

    private bool _hasTouchedPlatform = false;
    private Renderer _renderer;
    private WaitForSeconds _delay;
    private UnityAction<GameObject> _onReturnToPool;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasTouchedPlatform == false && collision.gameObject.TryGetComponent<Platform>(out _))
        {
            _hasTouchedPlatform = true;

            _renderer.material.color = GetRandomColor();

            float lifeTime = Random.Range(_minLifeTime, _maxLifeTime);
            _delay = new WaitForSeconds(lifeTime);

            StartCoroutine(WaitAndReturn());
        }
    }

    private IEnumerator WaitAndReturn()
    {
        yield return _delay;

        ReturnToPool();
    }

    private Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value);
    }

    public void SetReturnAction(UnityAction<GameObject> returnAction)
    {
        _onReturnToPool = returnAction;
    }

    public void ReturnToPool()
    {
        _hasTouchedPlatform = false;
        _renderer.material.color = _defaultColor;
        _onReturnToPool?.Invoke(gameObject);
    }
}
