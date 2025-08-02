using TMPro;
using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour
{    
    [SerializeField] private Transform _aimPoint;
    [SerializeField] private GameObject _effects;
    [SerializeField] private Transform _effectSpawnPoint;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Animator _animator;
    [SerializeField] private TextMeshProUGUI _textMesh;

    private PoolObjects _poolObjects;    
    private StateSwitcher _stateSwitcher;
    private BaseState _currentState;
    private PlayerInputObserver _inputObserver;
    private Sounds _sounds;
    public FOVControl _fovControl { get; private set; }

    private GameObject _spawnedEffect;   
    private Vector3 _spawnPointLclRot;
    private Coroutine _shootCoroutine;
    private int _ammo;

    public void Initialize(Sounds sounds, FOVControl fovControl, PoolObjects poolObjects)
    {
        _poolObjects = poolObjects;
        _fovControl = fovControl;
        _inputObserver = PlayerInputObserver.Instance;
        _sounds = sounds;
        _spawnPointLclRot = _effectSpawnPoint.localRotation.eulerAngles;
        _spawnedEffect = _effects;
        _spawnedEffect.SetActive(false);
        _ammo = 30;
        _stateSwitcher = new StateSwitcher(this);
        _inputObserver.SubscribeToEvent(Shoot, "Attack");
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
        if (_ammo == 0)
        {
            _stateSwitcher.RequestStateChange(StateType.Reloading);
        }
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
        SetAnimation("Reloading", false);
        _ammo = 30;
        _textMesh.text = _ammo.ToString();
        _stateSwitcher.RemoveState(StateType.Reloading);
        _stateSwitcher.RequestStateChange(StateType.Idle);        
    }
    public void Shoot()
    {
        if (_inputObserver.GetInput("Reload"))
        {
            _stateSwitcher.RequestStateChange(StateType.Reloading);
            return;
        }
        if (_shootCoroutine != null) return;

        if (_inputObserver.GetInput("Attack"))
        {
            _stateSwitcher.RequestStateChange(StateType.Shooting);
            _shootCoroutine = StartCoroutine(Shooting());
            RayCastShoot();
        }
        else
        {
            _stateSwitcher.RemoveState(StateType.Shooting);
            _stateSwitcher.RequestStateChange(StateType.Idle);
        }
    }
    public void RayCastShoot()
    {
        Ray ray = new Ray(_shootPoint.position, _shootPoint.forward);
        RaycastHit hit;
        IShootReaction interactiveObject;

        if (Physics.Raycast(ray, out hit, 50f))
        {
            interactiveObject = hit.collider.GetComponent<IShootReaction>();
            if (interactiveObject != null )
            {
                interactiveObject.DoReaction();
            }
            _poolObjects.ActiveFromPool(ray, hit);
        }
    }
    private IEnumerator Shooting()
    {
        _effectSpawnPoint.transform.localRotation = Quaternion.Euler(Random.Range(0, 180), _spawnPointLclRot.y, _spawnPointLclRot.z);
        _spawnedEffect.SetActive(true);
        _ammo--;
        _textMesh.text = _ammo.ToString();
        _sounds.PlaySound(0);

        yield return new WaitForSeconds(0.11f);
        _spawnedEffect.SetActive(false);        
        _shootCoroutine = null;
        _stateSwitcher.RemoveState(StateType.Shooting);

        yield break;
    }
    private void Update()
    {
        //_stateSwitcher.SeeStates();
    }
}
