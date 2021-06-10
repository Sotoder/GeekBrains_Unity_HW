using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public void Fire();

    public void ChangePosition(Vector3 newPosition);

    public void DestroyWeapon();
}
