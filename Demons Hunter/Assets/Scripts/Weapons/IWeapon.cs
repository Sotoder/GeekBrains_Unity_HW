using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public float FireRate { get; }
    public void Fire();
    public void DestroyWeapon();
}
