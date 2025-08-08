using UnityEngine;

public interface IBaseState
{
    public void Enter(WeaponManager manager);
    public void Exit(WeaponManager manager);
    public void Update(WeaponManager manager);
}

public class Idle : IBaseState
{
    public void Enter(WeaponManager manager)
    {
        
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

public class Shooting : IBaseState
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
public class Reloading : IBaseState
{
    public void Enter(WeaponManager manager)
    {
        manager.SetAnimation("Reloading", true);
    }
    public void Exit(WeaponManager manager)
    {

    }
    public void Update(WeaponManager manager)
    {
        
    }
}