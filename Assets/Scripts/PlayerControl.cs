using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;
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
    void Jump(float force);
}
public class PlayerMoving : IPlayerMove
{    
    private readonly Rigidbody _rigidbody;
    private readonly float _speed;
    private float _fieldOfView = 60;
    private Vector2 _move;
    private readonly Camera _camera;
    InputSystem_Actions _inputSystem;
    private CharacterController _controller;
    private GameObject _player;
    public PlayerMoving(Rigidbody rigidbody, float speed, InputSystem_Actions inputSystem, Camera camera, CharacterController controller, GameObject player)
    {
        _player = player;
        _rigidbody = rigidbody;
        _speed = speed;
        _inputSystem = inputSystem;
        _camera = camera;
        _controller = controller;
    }
    public void OnMove()
    {        
        //_camera.fieldOfView = _fieldOfView + _rigidbody.linearVelocity.magnitude * 2;
        _move = _inputSystem.Player.Move.ReadValue<Vector2>();

        Vector3 movement = new Vector3(_move.x, 0, _move.y) * Time.deltaTime * _speed;
        movement = Vector3.ClampMagnitude(movement, 2f);
        movement = _player.transform.TransformDirection(movement);

        _controller.Move(movement);
        //Debug.Log(_rigidbody.linearVelocity.magnitude);
    }
    public void Jump(float force)
    {
        if (_inputSystem.Player.Jump.triggered)
        {
            
            Debug.Log("Jump");
            _rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
        }
    }
}
public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float _jumpForce = 50f;
    private IPlayerMove _playerMoving;
    private ICameraMove _cameraMoving;   
    public void Init(IPlayerMove playerMove, ICameraMove cameraMove)
    {
        _playerMoving = playerMove;
        _cameraMoving = cameraMove;
    }
    void Update()
    {
        _playerMoving.OnMove();
        _playerMoving.Jump(_jumpForce);
        _cameraMoving.CameraMove();
    }
}
