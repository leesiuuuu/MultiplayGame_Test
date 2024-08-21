using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Animator animator;
    public void EndAttack1Animation()
    {
        animator.SetBool("IsAttack", false);
    }
    public void StartAttackCount()
    {
        animator.SetBool("IsAttack", true);
    }
}
