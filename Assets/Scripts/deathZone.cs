using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class deathZone : MonoBehaviourPunCallbacks
{

    void OnCollisionEnter(Collision collision)
    {   
        if(collision.gameObject.tag == "player" )
        {
            //PhotonNetwork.Destroy(this.gameObject);
            if(collision.gameObject.GetComponent<PlayerController>().deadValue()){
                 collision.gameObject.GetPhotonView().RPC("setDead", RpcTarget.All, true);
                 roundManager.instance.addDeadPlayer();

            }
            
        }
    }
    

}
