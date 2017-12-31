using UnityEngine;
using UnityEngine.Networking;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] float shotCoolDown = .3f;
    [SerializeField] Transform firePosition;
    [SerializeField] Transform firePositionFar;
    //[SerializeField] shoteffect
    [SerializeField] int damage = 10;
    [SerializeField] float bombForce = 20f;

    [SerializeField] Camera cam;


    [SerializeField] GameObject bombPrefab;

    [SerializeField] GameObject gunBullet; 
    [SerializeField] float bulletSpeed;

    float ellapsedTime;
    bool canShoot;

    void Start()
    {
        //shoteffect.Initialize();
        if (isLocalPlayer)
        {
            canShoot = true;
        }
    }

    void Update()
    {
        if (!canShoot)
        {
            return;
        }
        ellapsedTime += Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && ellapsedTime > shotCoolDown)
        {
            ellapsedTime = 0f;
            //Cmd
            //CmdFireShot(firePosition.position, firePosition.forward);
            //CmdFireGrenade(firePosition.position, firePosition.rotation);

            float tiltedDown = firePosition.eulerAngles.x;

            Debug.Log(tiltedDown);
            Debug.Log(firePosition.eulerAngles.x);

            if (tiltedDown>20)
            {
                CmdFireEquippedGun(firePositionFar.position, firePositionFar.rotation);
            }
            else
            {
                CmdFireEquippedGun(firePosition.position, firePosition.rotation);
            }
        }
        if (Input.GetButtonDown("Fire2") && ellapsedTime > shotCoolDown)
        {
            ellapsedTime = 0f;
            //Cmd
            //CmdFireShot(firePosition.position, firePosition.forward);
            float tiltedDown = firePosition.rotation.x;
            if (tiltedDown > 20)
            {
                CmdFireGrenade(firePositionFar.position, firePositionFar.rotation);
            }
            else
            {
                CmdFireGrenade(firePosition.position, firePosition.rotation);
            }
            //CmdFireEquippedGun(firePosition.position, firePosition.rotation);
        }

    }


    [Command]
    void CmdFireShot(Vector3 origin, Vector3 direction)
    {
        Debug.Log("Shooting Bullet");

        RaycastHit hit;

        Ray ray = new Ray(origin, direction);
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 1f);

        bool result = Physics.Raycast(ray, out hit, 50f);

        if (result)
        {
            Debug.Log("Hit Something");

            PlayerHealth enemy = hit.transform.GetComponent<PlayerHealth>();

            Debug.Log(enemy);

            if (enemy!= null){
                Debug.Log("Hit Enemy" + " Giving Damage: " + damage);
                enemy.TakeDamage(damage);
            }

        }
        RpcProcessShotEffects(result, hit.point);
    }


    [Command]
    void CmdFireEquippedGun(Vector3 position, Quaternion rotation)
    {
        Debug.Log("Shooting Gun");
        //spawn a bullet
        Debug.Log("Server Spawning a bullet");
        // Create the bomb from the bomb Prefab
        var bullet = (GameObject)Instantiate(gunBullet, position, rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;
        // Spawn the bomb on the Clients
        //NetworkServer.SpawnWithClientAuthority(bullet, (RigidBodyFPSController)gameObject);
        NetworkServer.Spawn(bullet);
    }

    [Command]
    void CmdFireGrenade(Vector3 position, Quaternion rotation)
    {
        Debug.Log("Shooting Grenade");
        //spawn a grenade
        Debug.Log("Server Spawning a bomb");
        // Create the bomb from the bomb Prefab
        var bomb = (GameObject)Instantiate(bombPrefab, position, rotation);

        bomb.GetComponent<Rigidbody>().velocity = bomb.transform.forward * bombForce;

        // Spawn the bomb on the Clients
        NetworkServer.Spawn(bomb);
    }


    [ClientRpc]
    void RpcProcessShotEffects(bool hit, Vector3 point)
    {
        //shoteffects.PlayShotEffects();

        if (hit)
        {

            //shoteffect.PlayImpactEffect(point);
        }
    }

}
