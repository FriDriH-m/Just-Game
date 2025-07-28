using System.Collections.Generic;
using UnityEngine;

public class FOVControl 
{
    private enum FOVType
    {
        Default, Aiming, Running
    }
    private Camera _camera;
    private Dictionary<string, int> _allowedFOV;
    private int _targetFOV = 60;
    private FOVType _type;
    

    public void Initialize(Camera camera)
    {
        _camera = camera;
        _allowedFOV = new Dictionary<string, int>
        {
            { "Default", 60 },
            { "Aiming", 30 },
            { "Running", 80 }            
        };
    }

    public void SetTargetFOW(string action)
    {
        _targetFOV = _allowedFOV[action];
    }

    public void UpdateFOV()
    {
        
        _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, _targetFOV, Time.deltaTime * 15);
    }
}
