using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    private CameraMoving _cameraMoving;
    private PlayerMoving _playerMoving;
    private PlayerControl _playerControl;
    private WeaponParticleSystem _particleSystem;
    private InputSystem_Actions _inputSystem;
    private Sounds _sounds;
    [SerializeField] private GameObject _weapon;
    [SerializeField] private Rigidbody _playerRigidbody;
    [SerializeField] private Camera _mainCamera;    
    [SerializeField] private GameObject _player;
    [SerializeField] float _speed = 5f;
    [SerializeField] float _lookSpeed = 0.5f;
    private CharacterController _controller;

    private void Awake()
    {
        _playerControl = _player.GetComponent<PlayerControl>();
        _controller = _player.GetComponent<CharacterController>();
        _inputSystem = new InputSystem_Actions();

        _sounds = GetComponent<Sounds>();
        _sounds.Init(GetComponent<AudioSource>());

        _inputSystem.Enable();
        _particleSystem = _weapon.GetComponent<WeaponParticleSystem>();

        _particleSystem.Initialize(_inputSystem, _sounds);
        _cameraMoving = new CameraMoving(_mainCamera, _player.transform, _lookSpeed, _inputSystem);
        _playerMoving = new PlayerMoving(_playerRigidbody, _speed, _inputSystem, _mainCamera, _controller, _player);
        _playerControl.Init(_playerMoving, _cameraMoving);
    }
}
