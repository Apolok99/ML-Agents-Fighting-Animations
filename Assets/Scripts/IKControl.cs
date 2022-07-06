using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))]
[ExecuteInEditMode]
public class IKControl : MonoBehaviour
{

    protected Animator animator;

    public bool ikActive = false;
    public Transform rightHandObj = null;
    public Transform rightElbowObj = null;
    public Transform leftHandObj = null;
    public Transform leftElbowObj = null;
    public Transform lookObj = null;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // A callback for calculating IK
    void OnAnimatorIK()
    {
        //if the IK is active, set the position and rotation directly to the goal. 
        if (ikActive)
        {
            // Set the look target position, if one has been assigned
            if (lookObj != null)
            {
                animator.SetLookAtWeight(1);
                animator.SetLookAtPosition(lookObj.position);
            }

            // Set the right hand target position and rotation and the right elbow target position and rotation, if one has been assigned
            if (rightHandObj != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
                animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, 1f);

                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
                animator.SetIKHintPosition(AvatarIKHint.RightElbow, rightElbowObj.position);
            }

            // Set the left hand target position and rotation and the left elbow target position and rotation, if one has been assigned
            if (leftHandObj != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
                animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, 1);

                animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObj.rotation);
                animator.SetIKHintPosition(AvatarIKHint.LeftElbow, leftElbowObj.position);
            }
        }

        //if the IK is not active, set the position and rotation of everything to the original position
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);

            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
            animator.SetLookAtWeight(0);
        }
        
    }
}