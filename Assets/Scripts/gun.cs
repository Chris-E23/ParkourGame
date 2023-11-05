using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class gun : MonoBehaviourPunCallbacks
{
   
    void Update()
    {


    }

    [PunRPC]
    public void bePickedUp()
    {
        if (photonView.IsMine)
        {
            pickedUp();
            Debug.Log("Attempt!");
        }


    }
    private void pickedUp()
    {
     
        
    }
}
