using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    private IPlayerMove _playerMoving;
    private ICameraMove _cameraMoving;
    private PlayerControl _playerControl;
    private PlayerInputObserver _playerInputObserver;
    private FOVControl _fovControl;

    private WeaponManager _weaponManager;
    private InputSystem_Actions _inputSystem;
    private Sounds _sounds;
    [SerializeField] private GameObject _weapon;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private GameObject _player;
    [SerializeField] float _lookSpeed = 0.5f;
    [SerializeField] private PoolObjects _poolObjects;
    private CharacterController _controller;

    private void Awake()
    {
        _sounds = GetComponent<Sounds>();
        _playerControl = _player.GetComponent<PlayerControl>();
        _controller = _player.GetComponent<CharacterController>();
        _weaponManager = _weapon.GetComponent<WeaponManager>();

        _inputSystem = new InputSystem_Actions();
        _fovControl = new FOVControl();
        _inputSystem.Enable();
        _cameraMoving = new CameraMoving(_mainCamera, _player.transform, _lookSpeed, _inputSystem);
        _playerMoving = new PlayerMoving(_inputSystem, _mainCamera, _controller, _player, _fovControl);        
        
        _playerInputObserver = new PlayerInputObserver();
                
        _playerInputObserver.Init(_inputSystem);
        _fovControl.Init(_mainCamera);        
        _sounds.Init(GetComponent<AudioSource>());
        _weaponManager.Init(_playerInputObserver, _sounds, _fovControl, _poolObjects);
        _playerControl.Init(_cameraMoving, _playerMoving);
        _poolObjects.CreatePool();
    }
    private void Update()
    {
        
        _weaponManager.Control();
        _playerInputObserver.CheckInput();
        _fovControl.UpdateFOV();
        _cameraMoving.CameraMove();
        _playerMoving.OnMove();
        _playerMoving.Jump(this);
        _playerMoving.Gravity();
    }
}
