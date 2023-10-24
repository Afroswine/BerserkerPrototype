using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimatorHash
{
    // Substates
    public static int SubstateID { get { return Animator.StringToHash("SubstateID"); } }
    public static int SubstateChange { get { return Animator.StringToHash("SubstateChange"); } }

    // Movement
    public static int IsMoving { get { return Animator.StringToHash("IsMoving"); } }
    public static int IsSprinting { get { return Animator.StringToHash("IsSprinting"); } }
    public static int IsJumping { get { return Animator.StringToHash("IsJumping"); } }
    public static int IsFalling { get { return Animator.StringToHash("IsFalling"); } }
    public static int SmoothMoveX { get { return Animator.StringToHash("SmoothMoveX"); } }
    public static int SmoothMoveY { get { return Animator.StringToHash("SmoothMoveY"); } }
    public static int Speed { get { return Animator.StringToHash("Speed"); } }

    // Combat
    public static int IsCombat { get { return Animator.StringToHash("IsCombat"); } }
    public static int IsAttacking { get { return Animator.StringToHash("IsAttacking"); } }
}
