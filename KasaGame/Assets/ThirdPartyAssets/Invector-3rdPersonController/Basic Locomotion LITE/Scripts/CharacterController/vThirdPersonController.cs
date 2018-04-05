using UnityEngine;
using System.Collections;

namespace Invector.CharacterController
{
    public class vThirdPersonController : vThirdPersonAnimator
    {
        private bool jumpedWhileJumping = false;
        protected virtual void Start()
        {
#if !UNITY_EDITOR
                Cursor.visible = false;

#endif
        }

        public virtual void Sprint(bool value)
        {                                   
            if (!isJumping)
            {
                isSprinting = value;   
            }            
        }

        public virtual void Strafe()
        {
            if (locomotionType == LocomotionType.OnlyFree) return;
            isStrafing = !isStrafing;
        }

        public virtual void Attack()
        {
            animator.SetTrigger("Attack");
        }

        public virtual void SpecialJump()
        {
            // conditions to do this action
            bool jumpConditions = !isJumping;
            // return if jumpCondigions is false
            if (!jumpConditions) return;
            // trigger jump behaviour
            jumpCounter = jumpTimer;
            isJumping = true;
            // trigger jump animations            
            if (_rigidbody.velocity.magnitude < 1)
                animator.CrossFadeInFixedTime("Jump", 0.05f);
            else
                animator.CrossFadeInFixedTime("JumpMove", 0.1f);
        }

        public virtual void Jump()
        {
            // conditions to do this action
            bool jumpConditions = !isJumping && isGrounded;
            // return if jumpCondigions is false
            if (!jumpConditions) return;
            // trigger jump behaviour
            jumpCounter = jumpTimer;            
            isJumping = true;
            // trigger jump animations            
            if (_rigidbody.velocity.magnitude < 1)
                animator.CrossFadeInFixedTime("Jump", 0.05f);
            else
                animator.CrossFadeInFixedTime("JumpMove", 0.1f);
        }

        public virtual void RotateWithAnotherTransform(Transform referenceTransform)
        {
            var newRotation = new Vector3(transform.eulerAngles.x, referenceTransform.eulerAngles.y, transform.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(newRotation), strafeRotationSpeed * Time.fixedDeltaTime);
            targetRotation = transform.rotation;
        }
    }
}