using UnityEngine;
using UnityEngine.Networking;

public class Bomb : NetworkBehaviour {

    [SerializeField] float radius;
    
    [SerializeField] float power;
    [SerializeField] float detonationTime;
    [SerializeField] float upPower;
    public float timer = 0;

    
    void Start()
    {
        //timer = Time.time;
        Debug.Log("Bomb detonating in " + detonationTime + " seconds.");
        Invoke("CmdDetonate", detonationTime);
        //Invoke("CmdToss", detonationTime);
    }
    /*
    void Update()
    {
        if (Time.time - timer > detonationTime)
        {
            CmdDetonate();
        }
    }
    */
    


    // Applies an explosion force to all nearby rigidbodies
    [Command]
    public void CmdDetonate()
    {
        Vector3 explosionPos = GetComponent<Transform>().position;

        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Debug.Log("Bomb Explosion hit a rigidbody: " + rb.tag);
                //rb.AddExplosionForce(power, explosionPos, radius, upPower);
                Vector3 direction = rb.transform.position - transform.position;
                rb.velocity = (upPower * Vector3.up) + (direction.normalized * power);
            }
        }

        Destroy(gameObject,0.3f);

    }

}
