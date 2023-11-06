using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class shootingObject : MonoBehaviourPunCallbacks
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

   public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
    
       this.gameObject.GetComponent<Rigidbody>().AddForce(this.transform.forward * 100, ForceMode.Force);
    }   
}
