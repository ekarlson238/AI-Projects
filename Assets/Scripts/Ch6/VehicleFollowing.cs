using UnityEngine;
using System.Collections;
public class VehicleFollowing : MonoBehaviour
{
    public Path path;
    public float speed = 20.0f;
    public float mass = 5.0f;
    public bool isLooping = true;

    private float curSpeed;
    private int curPathIndex;
    private float pathLength;
    private Vector3 targetPoint;
    Vector3 velocity;

    void Start()
    {
        pathLength = path.Length;
        curPathIndex = 0;

        velocity = transform.forward;
    }

    void Update()
    {
        curSpeed = speed * Time.deltaTime;
        targetPoint = path.GetPoint(curPathIndex);

        if (Vector3.Distance(transform.position, targetPoint) <
        path.Radius)
        {

            if (curPathIndex < pathLength - 1) curPathIndex++;
            else if (isLooping) curPathIndex = 0;
            else return;
        }

        if (curPathIndex >= pathLength) return;

        if (curPathIndex >= pathLength - 1 && !isLooping)
            velocity += Steer(targetPoint, true);
        else velocity += Steer(targetPoint);

        transform.position += velocity;

        transform.rotation = Quaternion.LookRotation(velocity);
    }

    public Vector3 Steer(Vector3 target,
    bool bFinalPoint = false)
    {
        Vector3 desiredVelocity = (target - transform.position);
        float dist = desiredVelocity.magnitude;

        desiredVelocity.Normalize();

        if (bFinalPoint && dist < 10.0f) desiredVelocity *=
            (curSpeed * (dist / 10.0f));
        else desiredVelocity *= curSpeed;

        Vector3 steeringForce = desiredVelocity - velocity;
        Vector3 acceleration = steeringForce / mass;
        return acceleration;
    }
}