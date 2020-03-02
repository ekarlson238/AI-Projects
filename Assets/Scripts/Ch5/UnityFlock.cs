using UnityEngine;
using System.Collections;
public class UnityFlock : MonoBehaviour
{
    public float minSpeed = 20.0f;
    public float turnSpeed = 20.0f;
    public float randomFreq = 20.0f;
    public float randomForce = 20.0f;
    //alignment variables
    public float toOriginForce = 50.0f;
    public float toOriginRange = 100.0f;
    public float gravity = 2.0f;
    //seperation variables
    public float avoidanceRadius = 50.0f;
    public float avoidanceForce = 20.0f;
    //cohesion variables
    public float followVelocity = 4.0f;
    public float followRadius = 40.0f;
    //these variables control the movement of the boid
    private Transform origin;
    private Vector3 velocity;
    private Vector3 normalizedVelocity;
    private Vector3 randomPush;
    private Vector3 originPush;
    private Transform[] objects;
    private UnityFlock[] otherFlocks;
    private Transform transformComponent;
    void Start()
    {
        randomFreq = 1.0f / randomFreq;
        //Assign the parent as origin
        origin = transform.parent;
        //Flock transform
        transformComponent = transform;
        //Temporary components
        Component[] tempFlocks = null;
        //Get all the unity flock components from the parent
        //transform in the group
        if (transform.parent)
        {
            tempFlocks = transform.parent.GetComponentsInChildren
            <UnityFlock>();
        }
        //Assign and store all the flock objects in this group
        objects = new Transform[tempFlocks.Length];
        otherFlocks = new UnityFlock[tempFlocks.Length];
        for (int i = 0; i < tempFlocks.Length; i++)
        {
            objects[i] = tempFlocks[i].transform;
            otherFlocks[i] = (UnityFlock)tempFlocks[i];
        }
        //Null Parent as the flock leader will be
        //UnityFlockController object
        transform.parent = null;
        //Calculate random push depends on the random frequency
        //provided
        StartCoroutine(UpdateRandom());
    }
    IEnumerator UpdateRandom()
    {
        while (true)
        {
            randomPush = Random.insideUnitSphere * randomForce;
            yield return new WaitForSeconds(randomFreq +
            Random.Range(-randomFreq / 2.0f, randomFreq / 2.0f));
        }
    }
    void Update()
    {
        //Internal variables
        float speed = velocity.magnitude;
        Vector3 avgVelocity = Vector3.zero;
        Vector3 avgPosition = Vector3.zero;
        float count = 0;
        float f = 0.0f;
        float d = 0.0f;
        Vector3 myPosition = transformComponent.position;
        Vector3 forceV;
        Vector3 toAvg;
        Vector3 wantedVel;
        for (int i = 0; i < objects.Length; i++)
        {
            Transform transform = objects[i];
            if (transform != transformComponent)
            {
                Vector3 otherPosition = transform.position;
                // Average position to calculate cohesion
                avgPosition += otherPosition;
                count++;
                //Directional vector from other flock to this flock
                forceV = myPosition - otherPosition;
                //Magnitude of that directional vector(Length)
                d = forceV.magnitude;
                //Add push value if the magnitude, the length of the
                //vector, is less than followRadius to the leader
                if (d < followRadius)
                {
                    //calculate the velocity, the speed of the object, based
                    //on the avoidance distance between flocks if the
                    //current magnitude is less than the specified
                    //avoidance radius
                    if (d < avoidanceRadius)
                    {
                        f = 1.0f - (d / avoidanceRadius);
                        if (d > 0) avgVelocity +=
                        (forceV / d) * f * avoidanceForce;
                    }
                    //just keep the current distance with the leader
                    f = d / followRadius;
                    UnityFlock otherSealgull = otherFlocks[i];
                    //we normalize the otherSealgull velocity vector to get
                    //the direction of movement, then we set a new velocity
                    avgVelocity += otherSealgull.normalizedVelocity * f *
                    followVelocity;
                }
            }
        }
        if (count > 0)
        {
            //Calculate the average flock velocity(Alignment)
            avgVelocity /= count;
            //Calculate Center value of the flock(Cohesion)
            toAvg = (avgPosition / count) - myPosition;
        }
        else
        {
            toAvg = Vector3.zero;
        }
        //Directional Vector to the leader
        forceV = origin.position - myPosition;
        d = forceV.magnitude;
        f = d / toOriginRange;
        //Calculate the velocity of the flock to the leader
        if (d > 0) //if this void is not at the center of the flock
            originPush = (forceV / d) * f * toOriginForce;
        if (speed < minSpeed && speed > 0)
        {
            velocity = (velocity / speed) * minSpeed;
        }
        wantedVel = velocity;
        //Calculate final velocity
        wantedVel -= wantedVel * Time.deltaTime;
        wantedVel += randomPush * Time.deltaTime;
        wantedVel += originPush * Time.deltaTime;
        wantedVel += avgVelocity * Time.deltaTime;
        wantedVel += toAvg.normalized * gravity * Time.deltaTime;
        //Final Velocity to rotate the flock into
        velocity = Vector3.RotateTowards(velocity, wantedVel,
        turnSpeed * Time.deltaTime, 100.00f);
        transformComponent.rotation =
        Quaternion.LookRotation(velocity);
        //Move the flock based on the calculated velocity
        transformComponent.Translate(velocity * Time.deltaTime,
        Space.World);
        //normalise the velocity
        normalizedVelocity = velocity.normalized;
    }
}
