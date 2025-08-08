using UnityEngine;
using System.Collections;
public interface ICameraMove
{
    void CameraMove();
}
public interface IPlayerMove
{
    void OnMove();
    void Jump();
    void Gravity();
}
public class CameraMoving : ICameraMove
{
    private readonly Camera _camera;
    private readonly Transform _playerTransform;
    private readonly float _speed;
    private float _xRotation;
    private float _yRotation;
    private Vector2 _cameraMove;
    private InputSystem_Actions _inputSystem;

    public CameraMoving(Camera camera, Transform playerTransform, float speed, InputSystem_Actions inputSystem)
    {
        _camera = camera;
        _playerTransform = playerTransform;
        _speed = speed;
        _inputSystem = inputSystem;
    }
    public void CameraMove()
    {
        _cameraMove = _inputSystem.Player.Look.ReadValue<Vector2>();
        _xRotation -= _cameraMove.y * _speed;
        _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);

        _yRotation += _cameraMove.x * _speed;

        _camera.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        _playerTransform.rotation = Quaternion.Euler(0, _yRotation, 0);
    }
}

public class PlayerMoving : IPlayerMove
{
    private readonly float _speed = 12;
    private Vector2 _move;
    private InputSystem_Actions _inputSystem;
    private CharacterController _controller;
    private FOVControl _fovControl;
    private GameObject _player;
    private bool _isGrounded;
    private float force = 0.2f;
    private Coroutine _jumpCoroutine;
    private PlayerControl _playerControl;
    private PlayerInputObserver _playerInputObserver;
    public PlayerMoving(InputSystem_Actions inputSystem, CharacterController controller, GameObject player, FOVControl fovControl, PlayerControl playerControl)
    {
        _fovControl = fovControl;
        _player = player;
        _inputSystem = inputSystem;
        _controller = controller;
        _playerControl = playerControl;
        _playerInputObserver = PlayerInputObserver.Instance;
        _playerInputObserver.SubscribeToEvent(Jump, "Jump");
    }

    public void OnMove()
    {
        _isGrounded = _controller.isGrounded;
        //Debug.Log(_isGrounded);
        _move = _inputSystem.Player.Move.ReadValue<Vector2>();
        if(_move.magnitude > 0.5f )
        {
            _fovControl.SetTargetFOW("Running");
        } else _fovControl.SetTargetFOW("Default");
        Vector3 movement = new Vector3(_move.x, 0, _move.y) * Time.deltaTime * _speed;
        movement = Vector3.ClampMagnitude(movement, 2f);
        movement = _player.transform.TransformDirection(movement);

        _controller.Move(movement);
    }

    public void Gravity()
    {        
        if (_jumpCoroutine == null)
        {
            Vector3 gravity = new Vector3(0, -9.81f, 0) * Time.deltaTime;
            _controller.Move(gravity);
        }
    }

    public void Jump()
    {
        if (_jumpCoroutine != null)
        {
            return;
        }
        _jumpCoroutine = _playerControl.StartCoroutine(Jumping());
    }

    public IEnumerator Jumping()
    {
        float smoothing = 0f;
        _isGrounded = false;
        for (int i = 0; !_isGrounded; i++)
        {            
            _controller.Move(Vector3.up * (force - smoothing));
            smoothing += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        _jumpCoroutine = null;
        yield break;
    }
}
public class PlayerControl : MonoBehaviour
{
    ICameraMove cameraMove;
    IPlayerMove playerMove;

    public void Initialize(ICameraMove cameraMove, IPlayerMove playerMove)
    {
        this.cameraMove = cameraMove;
        this.playerMove = playerMove;
    }
}
