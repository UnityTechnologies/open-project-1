using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    private Animator animator;

    #region Animator_Hashes

    private int isWalking = Animator.StringToHash("IsWalking");
    private int isGrounded = Animator.StringToHash("IsGrounded");
    private int receivedHit = Animator.StringToHash("ReceivedHit");
    private int isAttacking = Animator.StringToHash("IsAttacking");

    #endregion
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetIsWalking(bool value)
    {
        animator.SetBool(isWalking, value);
    }

    public void SetIsIdle(bool value)
    {
        animator.SetBool(isGrounded, value);
    }

    public void AnimateHit()
    {
        animator.SetTrigger(receivedHit);
    }

    public void AnimateAttack()
    {
        animator.SetTrigger(isAttacking);
    }
}
