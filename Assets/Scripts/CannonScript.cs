using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CannonScript : MonoBehaviourPunCallbacks
{

    [SerializeField] private GameObject button; 
    private bool pressed; 
    [SerializeField] private float coolDown;
    [SerializeField] private List<GameObject> canList = new List<GameObject>();

    public static CannonScript instance; 
    //I need to make this universal!

    void Start()
    { 
      pressed = false;
      coolDown = 0; 
    }
    public void awake(){
        instance = this; 
    }
    public enum EventCodes{

        shooting,
        notShooting
    }
    
    void Update()
    {
      
        if(coolDown >=0)
         coolDown -= Time.deltaTime;


    }
    [PunRPC]
    public void shoot(){
        foreach(GameObject can in canList){
            can.gameObject.GetPhotonView().RPC("shoot", RpcTarget.All);
        }

    }


    public void press(){
        if(coolDown >= 0){
         
            coolDown = 50; 
            shoot();
        }

    }


}
