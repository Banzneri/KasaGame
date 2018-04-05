using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.CharacterController;
using System;

public class DamageHazard : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        HitPlayer(other);
    }

    void OnTriggerStay(Collider other)
    {
        HitPlayer(other);
    }

    void HitPlayer(Collider playerCollider)
    {
        if (playerCollider.gameObject.tag.Equals("Player"))
        {
            MyCharManager player = playerCollider.gameObject.GetComponent<MyCharManager>();
            vThirdPersonController controller = playerCollider.gameObject.GetComponent<vThirdPersonController>();
            Rigidbody rigidbody = controller.GetComponent<Rigidbody>();

            if (!player.Immune && player.Health > 0)
            {
                    controller.isFlying = true;
                    rigidbody.velocity = Vector3.zero;
                    rigidbody.AddForce(Vector3.up * 10, ForceMode.VelocityChange);
                    player.TakeDamage();
            }
        }
    }
}
