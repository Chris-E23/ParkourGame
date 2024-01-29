using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class randomPoles : MonoBehaviourPunCallbacks
{

    void Start()
    {
        for(int i = 0; i < 15; i++){

            Vector3 randomSpawnPosition = new Vector3(Random.Range(0,70), 13, Random.Range(0, 70));
            PhotonNetwork.Instantiate("Pole", randomSpawnPosition, Quaternion.identity, 0);

        }
        
    }

    
    void Update()
    {
        
    }
}
