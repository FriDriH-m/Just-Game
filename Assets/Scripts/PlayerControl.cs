using UnityEditor.ShaderGraph;
using UnityEngine;
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

    public CameraMoving(Camera camera, Transform playerTransform, float speed, InputSystem_Actions inputSystem)
    {
        _camera = camera;
        _playerTransform = playerTransform;
        _speed = speed;
        _cameraMove = inputSystem.Player.Look.ReadValue<Vector2>();
    }
    public void CameraMove()
    {
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
}
public class PlayerMoving : IPlayerMove
{    
    private readonly Rigidbody _rigidbody;
    private readonly float _speed;
    private Vector2 _move;
    public PlayerMoving(Rigidbody rigidbody, float speed, InputSystem_Actions inputSystem)
    {
        _rigidbody = rigidbody;
        _speed = speed;
        _move = inputSystem.Player.Move.ReadValue<Vector2>();
    }
    public void OnMove()
    {
        Vector3 movement = new Vector3(_move.x, 0, _move.y);
        _rigidbody.AddRelativeForce(movement * _speed, ForceMode.Impulse);
    }
}
public class PlayerControl : MonoBehaviour
{
    [SerializeField] float _speed = 5f;
    [SerializeField] float _lookSpeed = 0.5f;
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
        _cameraMoving.CameraMove();
    }
}
