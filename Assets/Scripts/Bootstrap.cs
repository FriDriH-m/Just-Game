using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private GameObject _weapon;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private GameObject _player;
    [SerializeField] private float _lookSpeed = 0.5f;
    [SerializeField] private PoolObjects _poolObjects;
    //------------For Flashlight---------------
    [SerializeField] private Flashlight _flashlight;
    [SerializeField] private GameObject _light;
    //-----------------------------------------
    //------------For CameraLook---------------
    private CameraLook _cameraLook;
    //-----------------------------------------
    private IPlayerMove _playerMoving;
    private ICameraMove _cameraMoving;
    private PlayerControl _playerControl;
    private PlayerInputObserver _playerInputObserver;
    private FOVControl _fovControl;
    private CharacterController _controller;
    private WeaponManager _weaponManager;
    private InputSystem_Actions _inputSystem;
    private Sounds _sounds;  

    private void Awake()
    {
        _sounds = GetComponent<Sounds>();
        _playerControl = _player.GetComponent<PlayerControl>();
        _controller = _player.GetComponent<CharacterController>();
        _weaponManager = _weapon.GetComponent<WeaponManager>();

        _cameraLook = new CameraLook();
        _inputSystem = new InputSystem_Actions();
        _fovControl = new FOVControl();
        _inputSystem.Enable();
        _cameraMoving = new CameraMoving(_mainCamera, _player.transform, _lookSpeed, _inputSystem);
        _playerMoving = new PlayerMoving(_inputSystem, _mainCamera, _controller, _player, _fovControl);        
        
        _playerInputObserver = new PlayerInputObserver();

        _playerInputObserver.Initialize(_inputSystem);
        _fovControl.Initialize(_mainCamera);        
        _sounds.Initialize(GetComponent<AudioSource>());
        _weaponManager.Initialize(_sounds, _fovControl, _poolObjects);
        _playerControl.Initialize(_cameraMoving, _playerMoving);
        _poolObjects.CreatePool();

        //------------For Flashlight---------------
        _flashlight.Initialize(_light);
        //-----------------------------------------
        //------------For CameraLook---------------
        _cameraLook.Initialize(_mainCamera);
        //-----------------------------------------
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
        _cameraLook.CameraLooking();
    }
}
