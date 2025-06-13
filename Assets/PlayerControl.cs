using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] float _speed = 5f;
    [SerializeField] float lookSpeed = 0.5f;
    private float _xRotation;
    private float _yRotation;
    private InputSystem_Actions _control;
    private Rigidbody _rigidbody;
    private Camera _camera;

    private void Awake()
    {
        _control = new InputSystem_Actions();
        _camera = Camera.main;
        _rigidbody = GetComponent<Rigidbody>();
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
        OnMove(move);
        MoveCamera(cameraMove);
    }

    public void OnMove(Vector2 move)
    {
        Vector3 movement = new Vector3(move.x, 0, move.y);
        transform.GetComponent<Rigidbody>().AddRelativeForce(movement * _speed, ForceMode.Impulse);
        Debug.Log(_rigidbody.linearVelocity);
    }
    private void MoveCamera(Vector2 move)
    {
        _xRotation -= move.y * lookSpeed;
        _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);

        _yRotation += move.x * lookSpeed;

        _camera.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        transform.rotation = Quaternion.Euler(0, _yRotation, 0);
    }
}
