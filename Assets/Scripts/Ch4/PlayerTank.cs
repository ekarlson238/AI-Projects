using UnityEngine;
public class PlayerTank : MonoBehaviour
{
    public Transform targetTransform;
    public float movementSpeed, rotSpeed;
    void Update()
    {
        //Stop once you reached near the target position
        if (Vector3.Distance(transform.position,
        targetTransform.position) < 0.5f)
            return;
        //Calculate direction vector from current position to target
        //position
        Vector3 tarPos = targetTransform.position;
        tarPos.y = transform.position.y;
        Vector3 dirRot = tarPos - transform.position;
        //Build a Quaternion for this new rotation vector
        //using LookRotation method
        Quaternion tarRot = Quaternion.LookRotation(dirRot);
        //Move and rotate with interpolation
        transform.rotation = Quaternion.Slerp(transform.rotation,
        tarRot, rotSpeed * Time.deltaTime);
        transform.Translate(new Vector3(0, 0,
        movementSpeed * Time.deltaTime));
    }
}