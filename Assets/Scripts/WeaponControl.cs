using UnityEngine;

public class WeaponControl : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private void Update()
    {
        transform.rotation = Quaternion.Euler(_camera.transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
