﻿using UnityEngine;
using System.Collections;

namespace Invector.CharacterController
{
    public abstract class vThirdPersonAnimator : vThirdPersonMotor
    {
        public MyCharManager mc;

        void Awake()
        {
            mc = GetComponent<MyCharManager>();    
        }
        public virtual void UpdateAnimator()
        {
            if (animator == null || !animator.enabled) return;

            animator.SetBool("IsStrafing", isStrafing);
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetFloat("GroundDistance", groundDistance);

            if (!isGrounded)
                animator.SetFloat("VerticalVelocity", verticalVelocity);

            if (isStrafing)
            {
                // strafe movement get the input 1 or -1
                animator.SetFloat("InputHorizontal", direction, 0.1f, Time.deltaTime);
            }

            if (!GetComponent<MyCharManager>().throwing)
            {
                bool canThrow = !GetComponent<MyCharManager>().climbing && !animator.GetBool("pushing") && GetComponent<MyCharManager>().Health > 0;
                if (Input.GetButtonDown("Throw") && canThrow)
                {
                    if (speed < 1 && isGrounded) 
                    {
                        animator.Play("ThrowStand");
                    }
                    else
                        animator.Play("Throw");
                }   
            }

            // fre movement get the input 0 to 1
            if (!isFlying)
            {
                animator.SetFloat("InputVertical", speed, 0.1f, Time.deltaTime);
            }
        }

        public void Attack()
        {
            if (speed < 1 && isGrounded && !animator.GetBool("pushing")) 
            {   
                animator.Play("AttackStand");
            }
            else if (!animator.GetBool("pushing"))
                animator.Play("Attack");
        }

        public void OnAnimatorMove()
        {
            // we implement this function to override the default root motion.
            // this allows us to modify the positional speed before it's applied.
            if (isGrounded)
            {
                transform.rotation = animator.rootRotation;

                var speedDir = Mathf.Abs(direction) + Mathf.Abs(speed);
                speedDir = Mathf.Clamp(speedDir, 0, 1);
                var strafeSpeed = (isSprinting ? 1.5f : 1f) * Mathf.Clamp(speedDir, 0f, 1f);
                
                // strafe extra speed
                if (isStrafing)
                {
                    if (strafeSpeed <= 0.5f)
                        ControlSpeed(strafeWalkSpeed);
                    else if (strafeSpeed > 0.5f && strafeSpeed <= 1f)
                        ControlSpeed(strafeRunningSpeed);
                    else
                        ControlSpeed(strafeSprintSpeed);
                }
                else if (!isStrafing)
                {
                    // free extra speed                
                    if (speed <= 0.5f)
                        ControlSpeed(freeWalkSpeed);
                    else if (speed > 0.5 && speed <= 1f)
                        ControlSpeed(freeRunningSpeed);
                    else
                        ControlSpeed(freeSprintSpeed);
                }
            }
        }
    }
}