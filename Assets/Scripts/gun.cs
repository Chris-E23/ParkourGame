using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using UnityEngine.UIElements;

public class gun : MonoBehaviourPunCallbacks
{
    bool yes;
    private GameObject player;
    void Start()
    {
        yes = false;
        
    }
    void Update()
    {
        if (yes)
        {
            this.transform.position = player.gameObject.GetComponent<PlayerController>().getHand().position;
            this.transform.SetParent(player.gameObject.GetComponent<PlayerController>().getHand());
            
        }
        else
        {
            this.transform.SetParent(null);
        }    
    }

    [PunRPC]
    public void bePickedUp(int id)
    {
        if (photonView.IsMine)
        {
          
            pickedUp(id);
        }
    }
    private void pickedUp(int id)
    {
         player = PhotonView.Find(id).gameObject;
         yes = true;
        transform.rotation = player.transform.rotation;
        this.transform.Rotate(0,180,0);
        Destroy(this.gameObject.GetComponent<Rigidbody>());
       
    }
    [PunRPC]
    public void beDropped()
    {
        if (photonView.IsMine)
        {
            dropped();
        }
    }
    private void dropped()
    {
        this.gameObject.AddComponent<Rigidbody>();
        player = null;
        yes = false;
    }

    
}
