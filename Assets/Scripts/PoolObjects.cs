using UnityEngine;

public class PoolObjects : MonoBehaviour
{
    [SerializeField] private GameObject _bulletHoleDecal;
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
            _bulletHolePool[i] = Instantiate(_bulletHoleDecal, transform);
            _bulletHolePool[i].SetActive(false);
        }
    }

    public void ActivePool(Vector3 position, Quaternion quaternion)
    {
        _index++;
        GameObject poolObject = _bulletHolePool[_index];

        poolObject.transform.position = position;
        poolObject.transform.rotation = quaternion;
        poolObject.SetActive(true);
    }
}
