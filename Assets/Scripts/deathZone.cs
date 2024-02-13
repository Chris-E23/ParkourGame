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
             PhotonNetwork.Destroy(this.gameObject);
             collision.gameObject.GetComponent<PlayerController>().setDead(true);
             roundManager.instance.addDeadPlayer();
        }
    }
}
