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
         obj.gameObject.GetComponent<Rigidbody>().AddForce(this.transform.forward, ForceMode.Impulse);

        //if(obj.gameObject.GetComponent<PhotonView>() != null)
           //obj.GetPhotonView().RPC("shooting", RpcTarget.All, this.transform.forward);
       
    
   }
}
