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
              //Debug.Log(d.gameObject.tag);
            if(d.gameObject.tag == "map"){
                Collider[] objects = Physics.OverlapSphere(this.transform.position, 15);
                foreach(Collider h in objects){
                    Rigidbody r = h.gameObject.GetComponent<Rigidbody>();
                    if(r!=null && h.gameObject.tag != "player"){
                        r.AddExplosionForce(100f, this.transform.position, 15);
                          Destroy(this.gameObject);
                    }
                    else if(h.gameObject.tag == "player"){
                        h.gameObject.GetComponent<PhotonView>().RPC("addExplosionForce", RpcTarget.All, this.transform.position);
                        PhotonNetwork.Destroy(this.gameObject.GetComponent<PhotonView>());
                          Destroy(this.gameObject);
                    }
                }
                
               
            }
            else if(d.gameObject.tag == "player"){
                 d.gameObject.GetComponent<PhotonView>().RPC("addExplosionForce", RpcTarget.All,  this.transform.position);
                 Destroy(this.gameObject);

            }
           
              
            }

        }
   
           
        
    
    [PunRPC]
   public void shooting(Vector3 rot)
    {
        Vector3 dir = rot*50;
       this.gameObject.GetComponent<Rigidbody>().AddForce(dir, ForceMode.Impulse);
    }   
}
