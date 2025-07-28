using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;

public class PlayerInputObserver
{
    private InputSystem_Actions _inputSystem;
    private InputActionMap _inputActionMap;
    private List<string> _inputs;

    public void Initialize(InputSystem_Actions input)
    {
        _inputSystem = input;
        _inputSystem.Enable();
        _inputActionMap = _inputSystem.Player;        
        _inputs = new List<string>();
    }
    public void SubscribeToEvent(Action func, string nameOfInput)
    {
        foreach(var action in _inputActionMap.actions)
        {
            if (action.name == nameOfInput)
            {
                action.performed += ctx => func();
            }
        }
    }
    public void CheckInput()
    {
        _inputs.Clear();

        foreach(var action in _inputActionMap.actions)
        {
            if (action.IsPressed())
            {
                _inputs.Add(action.name);
            }
        }
    }
    public bool GetInput(string name)
    {
        if (_inputs.Contains(name)) return true;        
        return false;
    }
}
