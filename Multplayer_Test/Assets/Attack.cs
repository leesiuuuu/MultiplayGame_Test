using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Animator animator;
    public void EndAttack1Animation()
    {
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
    }
}
