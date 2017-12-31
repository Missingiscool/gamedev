using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Rocket : NetworkBehaviour
{

    [SerializeField] float radius;

    [SerializeField] float power;
    [SerializeField] float detonationTime;
    [SerializeField] float upPower;
    public float timer = 0;

    void Start()
    {
        //timer = Time.time;
        Debug.Log("Bullet detonating in " + detonationTime + " seconds.");
        Invoke("CmdDetonate", detonationTime);
        //Invoke("CmdToss", detonationTime);
    }

    public void OnCollisionEnter(Collision collision)
    {
        CmdDetonate();
    }




    // Applies an explosion force to all nearby rigidbodies
    [Command]
    public void CmdDetonate()
    {
        Vector3 explosionPos = GetComponent<Transform>().position;

        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

        //GameObject[] values = new GameObject[2];

        //objectsToSmash.Add(values);                    // add an item to the end of the array
        //objectsToSmash[i] = newValue;                  // change the value stored at position i
        //GameObject thisItem = (GameObject)myArray[i];    // retrieve an item from position i
        //myArray.RemoveAt(i);                        // remove an item from position i
        //var howBig = myArray.Count;                 // get the number of items in the ArrayList



        //ArrayList objectsToSmashArray = new ArrayList();
        //ArrayList vectorArray = new ArrayList();

        foreach (Collider hit in colliders)
        {

            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                //RpcApplyForce(hit.gameObject);

                Debug.Log("Rocket Explosion hit a rigidbody: " + rb.tag);
                Vector3 direction = rb.transform.position - (explosionPos + (upPower * Vector3.down));

                Vector3 velocity = (direction.normalized * power);

                if (hit.GetComponent<PlayerHit>())
                {
                PlayerHit playerhit = hit.GetComponent<PlayerHit>();

                playerhit.RpcApplyForceToPlayer(velocity, power);
                }

                //hit.gameObject.RpcApplyForce(hit.gameObject, velocity, power);

                //GameObject[] values = new GameObject[2];
                //values[0]
                //objectsToSmashArray.Add(rb);
                //vectorArray.Add(velocity);
            }

        }

        //RpcApplyForces(objectsToSmashArray, vectorArray, gameObject);

        Destroy(gameObject);

    }

    //I think clients need to handle their own velocity??
    /// <summary>
    /// aaplies forces and kills the rocket
    /// </summary>
    /// <param name="objectsToSmashArray"></param>
    /// <param name="vectorArray"></param>
    /// <param name="rocket"></param>
    //[ClientRpc]
    //cant clientrpc for arraylists
    public void RpcApplyForces(ArrayList objectsToSmashArray, ArrayList vectorArray, GameObject rocket)
    {
        for (int i = 0; i < objectsToSmashArray.Count; ++i)
        {
            Debug.Log("RpcApplyForces to object");
            GameObject go = (GameObject)objectsToSmashArray[i];
            Vector3 direction = (Vector3)vectorArray[i];
            Rigidbody rb = go.GetComponent<Rigidbody>();
            rb.velocity = (direction.normalized * power);
        }
        
        Destroy(gameObject);
    }


    //I think clients need to handle their own velocity??
    [ClientRpc]
    public void RpcApplyForce(GameObject go, Vector3 velocity)
    {
        Rigidbody rb = go.GetComponent<Rigidbody>();
        rb.velocity = (velocity.normalized * power);

    }
}
