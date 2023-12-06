using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Runtime.CompilerServices;

public class spawnPlayers : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab, spawnPoint;
    [SerializeField] private GameObject[] spawnPoints; 
    private GameObject player;
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            spawnPlayer();
        }
    }

    public void spawnPlayer()
    {
        player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.transform.position, spawnPoint.transform.rotation);
    }
}
