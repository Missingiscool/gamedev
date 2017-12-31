using UnityEngine;
using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] int maxHealth = 100;
    Player player;
    int health;

    //cant know if local or server
    void Awake()
    {
        player = GetComponent<Player>();
    }

    [ServerCallback]
    void OnEnable()
    {
        health = maxHealth;

        InvokeRepeating("CheckDeath", 10f, 10f);
    }


    void CheckDeath()
    {
        Debug.Log("Checking Y.");
        if (transform.position.y < -10)
        {
            Debug.Log("Fell to your Death.");
            RpcTakeDamage(true);
        }
    }


    [Server]
    public bool TakeDamage(int damage)
    {

        bool died = false;

        if (health <= 0)
        {
            return died;
        }
        health -= damage;
        died = health <= 0;

        RpcTakeDamage(died);

        return died;
    }

    [ClientRpc]
    void RpcTakeDamage(bool died)
    {
        if (died)
        {
            player.Die();
        }
    }

}
