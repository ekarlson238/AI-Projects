using UnityEngine;
using System.Collections;
public class VehicleAvoidance : MonoBehaviour
{
    public float speed = 20.0f;
    public float mass = 5.0f;
    public float force = 50.0f;
    public float minimumDistToAvoid = 20.0f;
    private float curSpeed;
    private Vector3 targetPoint;

    void Start()
    {
        mass = 5.0f;
        targetPoint = Vector3.zero;
    }

    void OnGUI()
    {
        GUILayout.Label("Click anywhere to move the vehicle.");
    }

    //Update is called once per frame
    void Update() 
    {
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay
        (Input.mousePosition);
        if (Input.GetMouseButtonDown(0) &&
        Physics.Raycast(ray, out hit, 100.0f))
        {
            targetPoint = hit.point;
        }
        Vector3 dir = (targetPoint - transform.position);
        dir.Normalize();
        AvoidObstacles(ref dir);



        if (Vector3.Distance(targetPoint, transform.position) < 3.0f) return;

        curSpeed = speed * Time.deltaTime;

        var rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp
        (transform.rotation, rot, 5.0f *
        Time.deltaTime);
        transform.position += transform.forward *
         curSpeed;
    }

    public void AvoidObstacles(ref Vector3 dir)
    {
        RaycastHit hit;

        int layerMask = 1 << 8;

        if (Physics.Raycast(transform.position,
        transform.forward, out hit,
        minimumDistToAvoid, layerMask))
        {
            Vector3 hitNormal = hit.normal;
            hitNormal.y = 0.0f; 
            dir = transform.forward + hitNormal * force;
        }
    }
}
