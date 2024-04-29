using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class cannonObject : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject cannonBall; 
    [SerializeField] private Transform shootingPos; 
    
/*
    //[PunRPC]
   public void shoot(Vector3 direction){

        GameObject obj = PhotonNetwork.Instantiate("shootingObject", shootingPos.transform.position, Quaternion.identity, 0);
       obj.GetPhotonView().RPC("shooting", RpcTarget.All, shootingPos.transform.right); //It keeps shooting to the right 

        //if(obj.gameObject.GetComponent<PhotonView>() != null)
           //obj.GetPhotonView().RPC("shooting", RpcTarget.All, this.transform.forward);
       
    
   }
   */
   public Transform getShootingPos(){
      return shootingPos;
   }
}
