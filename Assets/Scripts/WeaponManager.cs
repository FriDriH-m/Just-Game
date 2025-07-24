using TMPro;
using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour
{    
    [SerializeField] private Transform _aimPoint;
    [SerializeField] private GameObject _effects;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _aimSpeed;
    [SerializeField] private Animator _animator;
    [SerializeField] private TextMeshProUGUI _textMesh;

    private int _ammo;
    private BaseState _currentState;
    private PlayerInputObserver _inputObserver;
    private InputSystem_Actions _inputSystem;
    private Sounds _sounds;
    public FOVControl _fovControl { get; private set; }

    private GameObject _spawnedEffect;    
    private Camera _camera;
    private Vector3 _spawnPointLclRot;
    private Vector3 _startWpnPos;
    private Coroutine _shootCoroutine;

    private Idle _idle = new();
    private Shooting _shooting = new();
    private Reloading _reloading = new();
    
    public void Init(PlayerInputObserver inpputObserver, InputSystem_Actions inputSystem, Sounds sounds, Camera camera, FOVControl fovControl)
    {
        _fovControl = fovControl;
        _inputObserver = inpputObserver;
        _camera = camera;
        _sounds = sounds;
        _inputSystem = inputSystem;
        _startWpnPos = transform.localPosition;
        _spawnPointLclRot = _spawnPoint.localRotation.eulerAngles;
        _spawnedEffect = Instantiate(_effects, _spawnPoint.position, _spawnPoint.rotation, _spawnPoint);
        _spawnedEffect.SetActive(false);
        _currentState = _idle;
        _ammo = 30;
    }
    public void SwitchState(BaseState newState)
    {
        if (_currentState != null) _currentState.Exit(this);

        _currentState = newState;
        _currentState.Enter(this);
    }
    public void Control()
    {
        _currentState.Update(this);

        if (_inputObserver.GetInput("Attack") && _ammo != 0) SwitchState(_shooting);
        else if (_ammo == 0) SwitchState(_reloading);
        else SwitchState(_idle);
    }
    public void SetAnimation(string animationName, bool state)
    {
        _animator.SetBool(animationName, state);
    }
    public void Aiming()
    {        
        if (_inputObserver.GetInput("Aim"))
        {
            _fovControl.SetTargetFOW("Aiming");
            SetAnimation("Aiming", true);
        }
        else
        {
            SetAnimation("Aiming", false);
        }        
    }
    public void EndReloading()
    {
        Debug.Log("Ивент сработал");
        SetAnimation("Reloading", false);
        _ammo = 30;
        SwitchState(_idle);
    }
    public void Shoot()
    {  
        if (_shootCoroutine != null) return;
        if (_inputObserver.GetInput("Attack"))
        {            
            _shootCoroutine = StartCoroutine(Shooting());
        }
    }
    private IEnumerator Shooting()
    {
        _spawnPoint.transform.localRotation = Quaternion.Euler(Random.Range(0, 180), _spawnPointLclRot.y, _spawnPointLclRot.z);
        _spawnedEffect.SetActive(true);
        _ammo--;
        _textMesh.text = _ammo.ToString();
        _sounds.PlaySound(0);

        yield return new WaitForSeconds(0.11f);
        _spawnedEffect.SetActive(false);        
        _shootCoroutine = null;
        
        yield break;
    }
}
