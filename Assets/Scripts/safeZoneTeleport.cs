using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class safeZoneTeleport : MonoBehaviourPunCallbacks
{
    
    void OnCollisionEnter(Collision collision)
    {   
        if(collision.gameObject.tag == "player" )
        {
             collision.gameObject.GetComponent<PhotonView>().RPC("teleportSafeZone", RpcTarget.All);
             roundManager.instance.addSafePlayer();
        }
    }
}
