using System.Collections;
using UnityEngine;

public class WeaponParticleSystem : MonoBehaviour
{
    private InputSystem_Actions _inputSystem;
    private GameObject _spawnedEffect;
    private Sounds _sounds;
    private Coroutine _shootCoroutine;
    private Vector3 _spawnPointLclRot;
    private Camera _camera;
    private Vector3 _startWpnPos;
    private float _rawFieldOfView = 60f;
    private float _fieldOfView
    {
        get => _rawFieldOfView;
        set => _rawFieldOfView = Mathf.Clamp(value, 35, 60);
    }
    [SerializeField] private Transform _aimPoint;
    [SerializeField] private GameObject _effects;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _aimSpeed;

    public void Initialize(InputSystem_Actions inputSystem, Sounds sounds, Camera camera)
    {
        _camera = camera;
        _sounds = sounds;
        _inputSystem = inputSystem;
        _startWpnPos = transform.localPosition;
    }
    public void Aiming()
    {
        if (_inputSystem.Player.Aim.ReadValue<float>() > 0)
        {
            _fieldOfView -= Time.deltaTime * 150;
            transform.position = Vector3.MoveTowards(transform.position, _aimPoint.position, _aimSpeed * Time.deltaTime);            
        }
        else
        {
            _fieldOfView += Time.deltaTime * 150;
            if (transform.localPosition != _startWpnPos)
            {
                
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, _startWpnPos, _aimSpeed * Time.deltaTime);
            }
        }
        _camera.fieldOfView = _fieldOfView;
    }

    public void Shoot()
    {
        if (_inputSystem.Player.Attack.ReadValue<float>() > 0)
        {
            if (_shootCoroutine == null)
            {
                _shootCoroutine = StartCoroutine(Shooting());
            }
        }
    }

    private void Start()
    {
        _spawnPointLclRot = _spawnPoint.localRotation.eulerAngles;
        _spawnedEffect = Instantiate(_effects, _spawnPoint.position, _spawnPoint.rotation, _spawnPoint);
        _spawnedEffect.SetActive(false);
    }
    
    private IEnumerator Shooting()
    {
        _spawnPoint.transform.localRotation = Quaternion.Euler(Random.Range(0, 180), _spawnPointLclRot.y, _spawnPointLclRot.z);
        _spawnedEffect.SetActive(true);
        _sounds.PlaySound(0);

        yield return new WaitForSeconds(0.11f);
        _spawnedEffect.SetActive(false);
        _shootCoroutine = null;
        yield return null;
    }
}
