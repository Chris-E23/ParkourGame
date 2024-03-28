using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class cannonObject : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject cannonBall; 
    [SerializeField] private Transform shootingPos; 
    

    [PunRPC]
   public void shoot(){

        GameObject obj = PhotonNetwork.Instantiate("shootingObject", shootingPos.forward, Quaternion.identity, 0);
        obj.GetPhotonView().RPC("shooting", RpcTarget.All, this.transform.forward);
        
    
   }
}
