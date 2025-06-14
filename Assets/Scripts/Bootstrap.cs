using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    DIContainer _diContainer;
    private void Awake()
    {
        _diContainer = new DIContainer();
        //IPlayerMove playerMove = new PlayerMoving(GetComponent<Rigidbody>(), 5f, InputSystem_Actions.Instance);
    }
}
