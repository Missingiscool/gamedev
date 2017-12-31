using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;


[System.Serializable]
public class ToggleEvent : UnityEvent<bool> { }

public class Player : NetworkBehaviour {

    [SerializeField] ToggleEvent onToggleShared;
    [SerializeField] ToggleEvent onToggleLocal;
    [SerializeField] ToggleEvent onToggleRemote;

    [SerializeField] float respawnTime = 5f;

    GameObject mainCamera;

    // Use this for initialization
    void Start () {
        //onToggleShared.AddListener(FPSPlayer.FirstPersonController.enabled);

        mainCamera = Camera.main.gameObject;

        EnablePlayer();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void DisablePlayer()
    {
        if (isLocalPlayer)
        {
            mainCamera.SetActive(true);
        }

        onToggleShared.Invoke(false);
        if (isLocalPlayer)
        {
            onToggleLocal.Invoke(false);
        }
        else
        {
            onToggleRemote.Invoke(false);
        }
    }

    void EnablePlayer()
    {
        if (isLocalPlayer)
        {
            mainCamera.SetActive(false);
        }

        onToggleShared.Invoke(true);
        if (isLocalPlayer)
        {
            onToggleLocal.Invoke(true);
        }
        else
        {
            onToggleRemote.Invoke(true);
        }
    }

    public void Die()
    {
        DisablePlayer();
        Invoke("Respawn", respawnTime);
    }

    void Respawn()
    {
        Debug.Log("Respawning");
        if (isLocalPlayer)
        {
            //set position to a spawn point
            Transform spawn = NetworkManager.singleton.GetStartPosition();
            transform.position = spawn.position;
            transform.rotation = spawn.rotation;
        }

        EnablePlayer();
    }

}
