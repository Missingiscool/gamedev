using UnityEngine;
using UnityEngine.Networking;

public class PlayerHit : NetworkBehaviour {


    //I think clients need to handle their own velocity??
    [ClientRpc]
    public void RpcApplyForceToPlayer(Vector3 velocity, float power)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        Debug.Log("command add force");
        Debug.Log("ForceVector" + velocity.normalized * power);

        Ray ray = new Ray(gameObject.transform.position, velocity.normalized);
        Debug.DrawRay(ray.origin, ray.direction * power, Color.red, 2f);
        rb.velocity = (velocity.normalized * power);

        rb.AddForce(velocity.normalized * power, ForceMode.Impulse);
        //rb.AddForce(velocity.normalized * power, ForceMode.Force);
        //rb.AddForce(velocity.normalized * power, ForceMode.VelocityChange);

        //rb.AddForce(velocity.normalized * power, ForceMode.Acceleration);

        //rb.velocity = (velocity.normalized * power);

    }
}
