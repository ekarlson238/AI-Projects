using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Flock : MonoBehaviour
{
    internal FlockController controller;
    [HideInInspector]
    public new Rigidbody rigidbody;
    private void Start()
    {
        this.rigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (controller)
        {
            Vector3 relativePos = steer() * Time.deltaTime;
            if (relativePos != Vector3.zero)
                rigidbody.velocity = relativePos;
            // enforce minimum and maximum speeds for the boids
            float speed = rigidbody.velocity.magnitude;
            if (speed > controller.maxVelocity)
            {
                rigidbody.velocity = rigidbody.velocity.normalized *
                controller.maxVelocity;
            }
            else if (speed < controller.minVelocity)
            {
                rigidbody.velocity = rigidbody.velocity.normalized *
                controller.minVelocity;
            }
        }
    }
    private Vector3 steer()
    {
        Vector3 center = controller.flockCenter -
        transform.localPosition; // cohesion
        Vector3 velocity = controller.flockVelocity -
        rigidbody.velocity; // alignment
        Vector3 follow = controller.target.localPosition -
        transform.localPosition; // follow leader
        Vector3 separation = Vector3.zero;
        foreach (Flock flock in controller.flockList)
        {
            if (flock != this)
            {
                Vector3 relativePos = transform.localPosition -
                flock.transform.localPosition;

                if (relativePos.sqrMagnitude != 0)
                    separation += relativePos / (relativePos.sqrMagnitude);
                else
                    separation += relativePos;
            }
        }
        // randomize
        Vector3 randomize = new Vector3((Random.value * 2) - 1,
        (Random.value * 2) - 1, (Random.value * 2) - 1);
        randomize.Normalize();
        return (controller.centerWeight * center +
        controller.velocityWeight * velocity +
        controller.separationWeight * separation +
        controller.followWeight * follow +
        controller.randomizeWeight * randomize);
    }
}
