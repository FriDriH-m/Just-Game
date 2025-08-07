using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;

public class PlayerInputObserver
{
    private static PlayerInputObserver _instance;
    private InputSystem_Actions _inputSystem;
    private InputActionMap _inputActionMap;
    private List<string> _inputs;
    private Dictionary<string, Action<InputAction.CallbackContext>> _subscribes;
    public static PlayerInputObserver Instance => _instance;

    public void Initialize(InputSystem_Actions input)
    {
        _instance = this;
        _inputSystem = input;
        _inputSystem.Enable();
        _inputActionMap = _inputSystem.Player;        
        _inputs = new List<string>();
        _subscribes = new Dictionary<string, Action<InputAction.CallbackContext>>();
    }
    public void SubscribeToEvent(Action func, string nameOfInput)
    {
        foreach (var action in _inputActionMap.actions)
        {
            if (action.name == nameOfInput)
            {
                Action<InputAction.CallbackContext> _method = context => func(); /* 
                                                                                  * для подписки на action.performed нужно передавать 
                                                                                  * параметр InputAction.CallbackContext в метод
                                                                                 */
                _subscribes[$"{func.Method.Name}-{nameOfInput}-{func.Target?.GetHashCode()}"] = _method;
                // регистрируем метод в словаре для последующей отписки (храним имя метода, имя события, на которое пописываемcя, объект, которому принадлежит метод)
                
                action.started += _method;
            }
        }
    }

    public void UnsubscribeFromEvent(Action func, string nameOfInput)
    {
        foreach (var action in _inputActionMap.actions)
        {
            if (action.name == nameOfInput)
            {
                Action<InputAction.CallbackContext> _method = _subscribes[$"{func.Method.Name}-{nameOfInput}-{func.Target.GetHashCode()}"];
                action.started -= _method;
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
