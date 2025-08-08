using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum StateType
{
    Shooting, Reloading, Idle
}
public class StateSwitcher 
{
    private List<StateType> _states;
    private Dictionary<StateType, (IBaseState, int)> _prioriteList;
    private StateType _currentStateType;
    private WeaponManager _weaponManager;

    private Shooting _shooting = new();
    private Reloading _reloading = new();
    private Idle _idle = new();
    public StateSwitcher(WeaponManager weaponManager)
    {
        _weaponManager = weaponManager;
        _currentStateType = StateType.Idle;
        _weaponManager.SwitchState(_idle);
        _prioriteList = new Dictionary<StateType, (IBaseState, int)>
        {
            {StateType.Shooting, (_shooting, 1)},
            {StateType.Idle, (_idle, 2) },
            {StateType.Reloading, (_reloading, 0) }
        };
        _states = new();
        _states.Add(StateType.Idle);
    }
    public void RequestStateChange(StateType state)
    {
        if (!_states.Contains(state))
        {
            _states.Add(state);
        }
        
        StateType _highestPriorityState = _states[0];

        foreach(StateType type in _states)
        {
            if (_prioriteList[type].Item2 < _prioriteList[_highestPriorityState].Item2)
            {
                _highestPriorityState = type;
            }
        }
        if (_currentStateType == _highestPriorityState)
        {
            return;
        }
        else
        {
            _currentStateType = _highestPriorityState;
            _weaponManager.SwitchState(_prioriteList[_highestPriorityState].Item1);
        }
    }
    public void RemoveState(StateType state)
    {        
        _states.Remove(state);
    }
    public void SeeStates()
    {
        string _string = "";
        foreach (StateType type in _states)
        {
            _string += " " + type.ToString();
        }
        Debug.Log(_string);
    }
}
