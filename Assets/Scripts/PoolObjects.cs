using UnityEngine;
using UnityEngine.UIElements;

public class PoolObjects : MonoBehaviour
{
    [SerializeField] private GameObject _bulletHoleDecal;
    [SerializeField] private GameObject _bulletEffect;
    private GameObject[] _effects = new GameObject[30];
    private GameObject[] _bulletHolePool = new GameObject[30];
    private int _poolIndex;
    private int _index
    {
        get => _poolIndex;
        set
        {
            _poolIndex = value >= _bulletHolePool.Length ? 0 : value;
        }
    }

    public void CreatePool()
    {
        for (int i = 0; i < _bulletHolePool.Length; i++)
        {
            _effects[i] = Instantiate(_bulletEffect, transform);
            _effects[i].SetActive(false);
            _bulletHolePool[i] = Instantiate(_bulletHoleDecal, transform);
            _bulletHolePool[i].SetActive(false);
        }
    }

    public void ActiveFromPool(Ray ray, RaycastHit hit)
    {
        Vector3 position = hit.point;
        Quaternion quaternion = Quaternion.LookRotation(ray.direction, hit.normal);
        Quaternion _quaternion = Quaternion.LookRotation(-ray.direction, hit.normal);
        _index++;
        GameObject poolObject = _bulletHolePool[_index];
        GameObject _effect = _effects[_index];

        _effect.transform.position = position;
        _effect.transform.rotation = _quaternion;
        _effect.SetActive(true);

        poolObject.transform.position = position;
        poolObject.transform.rotation = quaternion;
        poolObject.transform.parent = hit.transform;
        poolObject.SetActive(true);
    }
}
