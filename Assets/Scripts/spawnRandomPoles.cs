using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class spawnRandomPoles : MonoBehaviourPunCallbacks, IOnEventCallback
{
    
    public static spawnRandomPoles instance; 
    [SerializeField] GameObject[] poles;
    public enum EventCodes : byte
    {
       SpawnPoles
    }
     public EventCodes state = EventCodes.SpawnPoles;
    void Start()
    {
        instance = this; 
       
        PhotonNetwork.RaiseEvent((byte)EventCodes.SpawnPoles, null, new RaiseEventOptions{Receivers = ReceiverGroup.MasterClient}, new SendOptions {Reliability = true}); //ohhh this sends to the server. 

    }
    public override void OnEnable(){//add as a listener for photon events?
        PhotonNetwork.AddCallbackTarget(this);

    }
    public override void OnDisable(){ //remove as a listener for events
        PhotonNetwork.RemoveCallbackTarget(this);
    }
        
    public void OnEvent(EventData photonEvent){ // do something whenever an event is called through the photon network. 
        if(photonEvent.Code < 200){
            EventCodes theEvent = (EventCodes)photonEvent.Code;
            object[] data = (object[])photonEvent.CustomData;
            switch(theEvent){
              case EventCodes.SpawnPoles:
                    spawnPoles(data);
                    break;
              
            }
        }
    }
    public void spawnPoles(object thing){
        foreach(GameObject pole in poles){
            PhotonNetwork.Instantiate("Pole", new Vector3(Random.Range(-10.0f, 10.0f), 0, Random.Range(25f, 80f)), Quaternion.identity, 0);

        }

    }

}
