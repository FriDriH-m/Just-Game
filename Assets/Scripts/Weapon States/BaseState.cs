using UnityEngine;

public interface BaseState
{
    public void Enter(WeaponManager manager);
    public void Exit(WeaponManager manager);
    public void Update(WeaponManager manager);
}

public class Idle : BaseState
{
    public void Enter(WeaponManager manager)
    {
        Debug.Log("Idle");
    }
    public void Exit(WeaponManager manager)
    {

    }
    public void Update(WeaponManager manager)
    {
        manager.Aiming();
        manager.Shoot();
    }
}

public class Shooting : BaseState
{
    public void Enter(WeaponManager manager)
    {
        manager.SetAnimation("Shoot", true);
    }
    public void Exit(WeaponManager manager)
    {
        manager.SetAnimation("Shoot", false);
    }
    public void Update(WeaponManager manager)
    {
        manager.Aiming();
        manager.Shoot();
    }
}
public class Reloading : BaseState
{
    public void Enter(WeaponManager manager)
    {
        Debug.Log("Reloading");
        manager.SetAnimation("Reloading", true);
    }
    public void Exit(WeaponManager manager)
    {

    }
    public void Update(WeaponManager manager)
    {
        
    }
}