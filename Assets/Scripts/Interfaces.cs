using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: group functions into common interfaces
public interface ITakeDamage
{
    void TakeDamage(int Damage);
}

public interface IPauseMovement
{
    void PauseMovement(float knockoutTime);
}

public interface ITarget
{
    GameObject GetTarget();
    void SetTarget(GameObject target);
}

public interface IPlaySound
{
    void PlaySound(int mode);
}
