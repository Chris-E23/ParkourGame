using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using UnityEngine.UIElements;

public class gun : MonoBehaviourPun
{
    bool yes; //this means pickedup, bad name for a variable 
    private GameObject player;
    private GameObject hand; 

    void Start()
    {
        
        yes = false;
        
    }
    void Update()
    {
        if (yes)
        {
           
        }
        else
        {
            this.transform.SetParent(null);
        }    
    }

   /*
    public void bePickedUp(int id, Vector3 position)
    {
       // pickedUp(id);
       
        //this.gameObject.GetPhotonView().RPC("pickedUp", RpcTarget.All, photonView.ViewID, position);
    }
    */
     [PunRPC]
    private void pickedUp(Vector3 rot, int id)
    {   //this.transform.Rotate(-this.transform.rotation.x, -this.transform.rotation.y, -this.transform.rotation.z);
         //this.transform.Rotate(0,0,180);
        player = PhotonView.Find(id).gameObject;
        Transform hand = player.transform.GetChild(0).transform.GetChild(0);
        yes = true;
        this.transform.SetParent(hand);
        transform.localPosition = Vector3.zero; 
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;
        this.transform.position = hand.transform.position;
        this.transform.Rotate(0, 0, 180);
        this.gameObject.GetComponent<PhotonView>().enabled = false; 
        Destroy(this.gameObject.GetComponent<Rigidbody>());
       
    }
   /*
    public void beDropped()
    {
         //   dropped();
       this.gameObject.GetPhotonView().RPC("dropped", RpcTarget.All);
    }
    */
    
     [PunRPC]
    private void dropped()
    {
        this.gameObject.AddComponent<Rigidbody>();
        player = null;
        yes = false;
        this.transform.SetParent(null);
        this.gameObject.GetComponent<PhotonView>().enabled = true;
    }

    
}
