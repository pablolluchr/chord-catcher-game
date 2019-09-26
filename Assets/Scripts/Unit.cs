using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{
    public int animationState;//0: idle, 1: running, 2: attacking
    protected Animator anim;
    public float animationLength;
    // Use this for initialization
   

    public void Idle()
    {
        anim.CrossFade("Idle", 0.1f);
        anim.speed = 1f;
        animationState = 0;
    }

    public void Run()
    {
        anim.CrossFade("Run", 0.1f);
        anim.speed = 1f;
        animationState = 1;
    }

    public void Attack(float attackSpeed)
    {
        anim.CrossFade("Attack", 0.1f);
        anim.speed = attackSpeed / animationLength;
        animationState = 2;
    }

}
