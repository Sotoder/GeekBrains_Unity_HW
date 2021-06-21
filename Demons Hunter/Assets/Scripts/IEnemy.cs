using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IEnemy
{
    bool IsChangeKinematic { set; }
    Transform[] PatrolPoints { get; }
    void StopPatrol();
    void StartPatrol();
}
