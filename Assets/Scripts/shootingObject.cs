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
        
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 0.5f);
      
        foreach(Collider d in hitColliders){
              Debug.Log(d.gameObject.tag);
            if(d.gameObject.tag == "map"){
                Collider[] objects = Physics.OverlapSphere(this.transform.position, 15);
                foreach(Collider h in objects){
                    Rigidbody r = h.gameObject.GetComponent<Rigidbody>();
                    if(r!=null){
                        r.AddExplosionForce(1000f, this.transform.position, 15);
                }
               
            }
             Destroy(this.gameObject);
              Destroy(this);
            }
        }
   
           
        
    }
    [PunRPC]
   public void shooting(int photonID)
    {
        Vector3 dir = PhotonView.Find(photonID).gameObject.GetComponent<PlayerController>().getDir()*1000;
       this.gameObject.GetComponent<Rigidbody>().AddForce(dir, ForceMode.Force);
    }   
}
