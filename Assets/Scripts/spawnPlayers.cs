using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Runtime.CompilerServices;

public class spawnPlayers : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab, spawnPoint1, spawnPoint2;
   
    private GameObject player;
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            if((PhotonNetwork.LocalPlayer.ActorNumber+1)% 2 == 0){
                spawnPlayer(spawnPoint1);
            }
            else{
                spawnPlayer(spawnPoint2);
            }
        }
    }

    public void spawnPlayer(GameObject location)
    {
        player = PhotonNetwork.Instantiate(playerPrefab.name, location.transform.position, location.transform.rotation);
    }
}
