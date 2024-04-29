using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class cannonObject : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject cannonBall; 
    [SerializeField] private Transform shootingPos; 
    

    //[PunRPC]
   public void shoot(Vector3 direction){

        GameObject obj = PhotonNetwork.Instantiate("shootingObject", shootingPos.right, Quaternion.identity, 0);
       obj.GetPhotonView().RPC("shooting", RpcTarget.All, direction);

        //if(obj.gameObject.GetComponent<PhotonView>() != null)
           //obj.GetPhotonView().RPC("shooting", RpcTarget.All, this.transform.forward);
       
    
   }
}
