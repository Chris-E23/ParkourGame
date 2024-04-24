using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
public class CannonScript : MonoBehaviourPunCallbacks, IOnEventCallback
{

  

    [SerializeField] private float maxCoolDown;
    [SerializeField] private float curCoolDown; 
    [SerializeField] private List<GameObject> canList = new List<GameObject>();
    public override void OnEnable(){//add as a listener for photon events?
        PhotonNetwork.AddCallbackTarget(this);

    }
    public override void OnDisable(){ //remove as a listener for events
        PhotonNetwork.RemoveCallbackTarget(this);
    }    
    public static CannonScript instance; 
    //I need to make this universal!
      public void OnEvent(EventData photonEvent){ // do something whenever an event is called through the photon network. 
        if(photonEvent.Code < 200){
            EventCodes theEvent = (EventCodes)photonEvent.Code;
            object[] data = (object[])photonEvent.CustomData;
            switch(theEvent){
              case EventCodes.isShooting:
                    shooting(data);
                    break;
                case EventCodes.notShooting:
                    break;
              
            }
        }
    }    void Start()
    { 
     
        curCoolDown = maxCoolDown;
    }
    
    public void awake(){
        instance = this; 
    }
    public EventCodes state = EventCodes.notShooting;
    public enum EventCodes : byte{

        isShooting,
        notShooting
       
        
    }
    
    void Update()
    {
      
        if(curCoolDown >=0){
            Debug.Log(curCoolDown);
            curCoolDown -= Time.deltaTime;
        }
        else{
            curCoolDown = 0; 
        }

         


    }
    
    public void shoot(){
         PhotonNetwork.RaiseEvent((byte)EventCodes.isShooting, null, new RaiseEventOptions{Receivers = ReceiverGroup.MasterClient}, new SendOptions {Reliability = true}); 

    }
    public void shooting(object nothing){
    if(curCoolDown <= 0){
        foreach(GameObject cannon in canList){
            //cannon.GetPhotonView().RPC("shoot", RpcTarget.All, this.transform.forward);
        cannon.gameObject.GetComponent<cannonObject>().shoot(this.transform.forward);

        }
        curCoolDown = maxCoolDown;  
    }
}

    


}
