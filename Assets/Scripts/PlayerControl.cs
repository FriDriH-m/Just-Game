using UnityEngine;
interface ICameraMove
{
    void CameraMove(Vector2 move);
}
public class CameraMoving : ICameraMove
{
    private readonly Camera _camera;
    private readonly Transform _playerTransform;
    private readonly float _speed;
    private float _xRotation;
    private float _yRotation;

    public CameraMoving(Camera camera, Transform playerTransform, float speed)
    {
        _camera = camera;
        _playerTransform = playerTransform;
        _speed = speed;
    }
    public void CameraMove(Vector2 move)
    {
        _xRotation -= move.y * _speed;
        _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);

        _yRotation += move.x * _speed;

        _camera.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        _playerTransform.rotation = Quaternion.Euler(0, _yRotation, 0);
    }
}
interface IPlayerMove
{
    void OnMove(Vector2 move);
}
public class PlayerMoving : IPlayerMove
{    
    private readonly Rigidbody _rigidbody;
    private readonly float _speed;
    public PlayerMoving(Rigidbody rigidbody, float speed)
    {
        _rigidbody = rigidbody;
        _speed = speed;
    }
    public void OnMove(Vector2 move)
    {
        Vector3 movement = new Vector3(move.x, 0, move.y);
        _rigidbody.AddRelativeForce(movement * _speed, ForceMode.Impulse);
    }
}
public class PlayerControl : MonoBehaviour
{
    [SerializeField] float _speed = 5f;
    [SerializeField] float _lookSpeed = 0.5f;
    private InputSystem_Actions _control;
    private IPlayerMove _playerMoving;
    private ICameraMove _cameraMoving;
    
    private void Awake()
    {
        _control = new InputSystem_Actions();
        _playerMoving = new PlayerMoving(GetComponent<Rigidbody>(), _speed);
        _cameraMoving = new CameraMoving(Camera.main, transform, _lookSpeed);
    }
    private void OnEnable()
    {
        _control.Enable();
    }
    private void OnDisable()
    {
        _control.Disable();
    }
    void Update()
    {
        Vector2 move = _control.Player.Move.ReadValue<Vector2>();
        Vector2 cameraMove = _control.Player.Look.ReadValue<Vector2>();
        _playerMoving.OnMove(move);
        _cameraMoving.CameraMove(cameraMove);
    }
}
