using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public bool IsReload { get; }
    public void Fire();

    public void Fire(int modifer);
    public void DestroyWeapon();
}
