using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponParticleSystem : MonoBehaviour
{
    InputSystem_Actions _inputSystem;
    GameObject _spawnedEffect;
    Sounds _sounds;
    Coroutine _shootCoroutine;
    [SerializeField] private GameObject _effects;
    [SerializeField] private Transform _spawnPoint;

    public void Initialize(InputSystem_Actions inputSystem, Sounds sounds)
    {
        _sounds = sounds;
        _inputSystem = inputSystem;
    }

    private void Start()
    {
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
        Debug.Log(_spawnPoint.transform.rotation.y);
        _spawnedEffect.transform.rotation = Quaternion.Euler(Random.Range(0, 180), Quaternion.identity.y, 0);
        _spawnedEffect.SetActive(true);
        _sounds.PlaySound(0);
        yield return new WaitForSeconds(0.11f);
        _spawnedEffect.SetActive(false);
        _shootCoroutine = null;
        yield return null;
    }
}
