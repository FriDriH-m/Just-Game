using UnityEngine;
using System.Collections;
public interface ICameraMove
{
    void CameraMove();
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
public interface IPlayerMove
{
    void OnMove();
    void Jump(MonoBehaviour _playerControl);
    void Gravity();
}
public class PlayerMoving : IPlayerMove
{
    private MonoBehaviour _playerControl;
    private readonly float _speed;
    private float _rawFieldOfView = 60;
    private float fieldOfView
    {
        get => _rawFieldOfView;
        set => _rawFieldOfView = Mathf.Clamp(value, 60f, 80f);
    }
    private Vector2 _move;
    private readonly Camera _camera;
    private InputSystem_Actions _inputSystem;
    private CharacterController _controller;
    private GameObject _player;
    private bool _isGrounded;
    private float force = 20f;
    private bool _isJumping = false;
    public PlayerMoving(float speed, InputSystem_Actions inputSystem, Camera camera, CharacterController controller, GameObject player)
    {
        _player = player;
        _speed = speed;
        _inputSystem = inputSystem;
        _camera = camera;
        _controller = controller;
    }


    public void OnMove()
    {
        _move = _inputSystem.Player.Move.ReadValue<Vector2>();
        if (_move.magnitude != 0)
        {
            fieldOfView += Time.deltaTime * 100f;
        }
        else fieldOfView -= Time.deltaTime * 100f;

        _camera.fieldOfView = fieldOfView;

        Vector3 movement = new Vector3(_move.x, 0, _move.y) * Time.deltaTime * _speed;
        movement = Vector3.ClampMagnitude(movement, 2f);
        movement = _player.transform.TransformDirection(movement);

        _controller.Move(movement);
    }

    public void Gravity()
    {
        if (!_isGrounded && !_isJumping)
        {
            Vector3 gravity = new Vector3(0, -9.81f, 0) * Time.deltaTime;
            _controller.Move(gravity);
            _isGrounded = _controller.isGrounded;
        }
    }

    public void Jump(MonoBehaviour _playerControl)
    {
        if (_isGrounded && _inputSystem.Player.Jump.triggered)
        {
            _isJumping = true;
            _playerControl.StartCoroutine(Jumping());
            _isGrounded = _controller.isGrounded;
        }
    }

    public IEnumerator Jumping()
    {
        float smoothing = 0f;
        for (int i = 0; i < 30; i++)
        {
            _controller.Move(Vector3.up * (force - smoothing) * Time.deltaTime);
            smoothing += 0.7f;
            yield return new WaitForSeconds(0.01f);
        }
        smoothing = 16f;
        for (int i = 0; i < 40; i++)
        {
            _controller.Move(-(Vector3.up * (force - smoothing) * Time.deltaTime));
            smoothing -= 0.4f;
            yield return new WaitForSeconds(0.01f);
        }
        _isJumping = false;
        yield return null;
    }
}
public class PlayerControl : MonoBehaviour
{
    ICameraMove cameraMove;
    IPlayerMove playerMove;

    public void Init(ICameraMove cameraMove, IPlayerMove playerMove)
    {
        this.cameraMove = cameraMove;
        this.playerMove = playerMove;
    }
}
