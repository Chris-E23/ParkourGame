using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.Experimental.Rendering;

public class playerInfo : MonoBehaviourPunCallbacks
{
    //store team, kills 
    [SerializeField] private int team; 
    [SerializeField] private float kills; 
    [SerializeField] private int actorNumber; 
    [SerializeField] private GameObject playerObj; 
    [SerializeField] private Material red, blue; 
    void Start()
    {
        if(photonView.IsMine){
            actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            team = roundManager.instance.getTeam(actorNumber);
            if(playerObj != null){
                MeshRenderer renderer = playerObj.GetComponent<MeshRenderer>();
                switch(team){
                    case 0:
                        renderer.material = red; 
                        break;
                    case 1:
                        renderer.material = blue;
                        break;
                }
            }
     }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
