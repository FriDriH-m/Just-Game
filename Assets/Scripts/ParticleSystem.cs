using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponParticleSystem : MonoBehaviour
{
    private InputSystem_Actions _inputSystem;
    private GameObject _spawnedEffect;
    private Sounds _sounds;
    private Coroutine _shootCoroutine;
    private Vector3 _spawnPointLclRot;
    [SerializeField] private GameObject _effects;
    [SerializeField] private Transform _spawnPoint;

    public void Initialize(InputSystem_Actions inputSystem, Sounds sounds)
    {
        _sounds = sounds;
        _inputSystem = inputSystem;
    }

    private void Start()
    {
        _spawnPointLclRot = _spawnPoint.localRotation.eulerAngles;
        _spawnedEffect = Instantiate(_effects, _spawnPoint.position, _spawnPoint.rotation, _spawnPoint);
        _spawnedEffect.SetActive(false);
    }

    private void Update()
    {
        
        if (_inputSystem.Player.Attack.ReadValue<float>() > 0)
        {
            
            if (_shootCoroutine == null)
            {
                _shootCoroutine = StartCoroutine(Shoot());
            }
        }
    }
    private IEnumerator Shoot()
    {
        _spawnPoint.transform.localRotation = Quaternion.Euler(Random.Range(0, 180), _spawnPointLclRot.y, _spawnPointLclRot.z);
        _spawnedEffect.SetActive(true);
        _sounds.PlaySound(0);
        Debug.Log(_spawnedEffect.transform.rotation.eulerAngles);
        yield return new WaitForSeconds(0.11f);
        _spawnedEffect.SetActive(false);
        _shootCoroutine = null;
        yield return null;
    }
}
