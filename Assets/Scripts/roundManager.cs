using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; 
using UnityEngine.SceneManagement;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class roundManager : MonoBehaviourPunCallbacks, IOnEventCallback //use this to be able to use photonevents. 
{
    
    public static roundManager instance; 
    int playerCount; 
   public void Awake(){
        instance = this; 
   }

    public enum EventCodes : byte
    {
        NewPlayer,
        ListPlayers,
        UpdateStat, 
        NextMatch,
        TimerSync,

        SafePlayerAdd


    }

    public List<PlayerInfo> allPlayers = new List<PlayerInfo>();
    public List<GameObject> allPlayerObjects = new List<GameObject>();
    public List<PlayerInfo> blueTeam = new List<PlayerInfo>();
    public List<PlayerInfo> redTeam = new List<PlayerInfo>();

    int safeCount; 
    
    private int index; 
    
    public enum GameState{
        Waiting, 
        Playing, 
        Ending
    }

    public GameState state = GameState.Waiting;

    void Start()
    {
        if(!PhotonNetwork.IsConnected){
            SceneManager.LoadScene(0);
        }
        else{
            //playerSend(PhotonNetwork.NickName);
            state = GameState.Playing;
        }
    }
    public override void OnEnable(){//add as a listener for photon events?
        PhotonNetwork.AddCallbackTarget(this);

    }
    public override void OnDisable(){ //remove as a listener for events
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    void Update()
    {
        
       
    }
  
    
    public void OnEvent(EventData photonEvent){ // do something whenever an event is called through the photon network. 
        if(photonEvent.Code < 200){
            EventCodes theEvent = (EventCodes)photonEvent.Code;
            object[] data = (object[])photonEvent.CustomData;
            switch(theEvent){
                case EventCodes.NewPlayer:
                   playerReceive(data);
                    break;
                case EventCodes.ListPlayers:
                    ListPlayersReceive(data);
                    break;
              case EventCodes.SafePlayerAdd:
                    receiveSafePlayer(data);
                    break;
                    
              
            }
        }
    }

    
    public void playerSend(string name, int playerId){
        object[] package = new object[3];
        package[0] = name; 
        package[1] = PhotonNetwork.LocalPlayer.ActorNumber;
        package[2] = playerId;
        
        
        //send this to the masterclient and raise event 
        PhotonNetwork.RaiseEvent((byte)EventCodes.NewPlayer, package, new RaiseEventOptions{Receivers = ReceiverGroup.MasterClient}, new SendOptions {Reliability = true}); //ohhh this sends to the server. 
        //Just gotta use server events. I think this sends to all the same script. 

    }
    public void playerReceive(object[] dataReceived){
        PlayerInfo player = new PlayerInfo((string)dataReceived[0], (int)dataReceived[1], (int)dataReceived[2]);
        allPlayers.Add(player);
        if(player.team == 1){
            blueTeam.Add(player);
        }
        else{
            redTeam.Add(player);
        }

        foreach(PlayerInfo playerInf in blueTeam) {
            Debug.Log("Blue team " + playerInf.name);
        }
        foreach(PlayerInfo playerInf in redTeam) {
            Debug.Log("Red team " + playerInf.name);
        }
        ListPlayersSend();
    }   
    public void ListPlayersSend(){
        object[] package = new object[allPlayers.Count + 1];
        package[0] = state;
        for(int i = 0; i < allPlayers.Count; i++){
            object[] piece = new object[4];
            piece[0] = allPlayers[i].name;
            piece[1] = allPlayers[i].actor;
            piece[2] = allPlayers[i].team;
            package[i+1] = piece; 
        }

        PhotonNetwork.RaiseEvent(
            (byte)EventCodes.ListPlayers, 
            package, 
            new RaiseEventOptions{Receivers = ReceiverGroup.All}, 
            new SendOptions{Reliability = true}
        );

    }
    public void ListPlayersReceive(object[] dataReceived){
        allPlayers.Clear();
        state = (GameState)dataReceived[0];
        for(int i = 1; i < dataReceived.Length; i++){
            object[] piece = (object[])dataReceived[i];

            PlayerInfo player = new PlayerInfo((string)piece[0], (int)piece[1], (int)piece[2]);
            allPlayers.Add(player);

            if(PhotonNetwork.LocalPlayer.ActorNumber == player.actor){
                index = i - 1; // this saves the index of the player always locally. 
                
            }
        }
    }

    public void addSafePlayer(){
         PhotonNetwork.RaiseEvent((byte)EventCodes.SafePlayerAdd, null, new RaiseEventOptions{Receivers = ReceiverGroup.MasterClient}, new SendOptions {Reliability = true}); 
         // I might just be able to use <PhotonView>().RPC("teleportSafeZone", RpcTarget.All); here but idk \\
       //  this.gameObject.GetComponent<PhotonView>().RPC("receiveSafePlayer", RpcTarget.All);

    }

    public void receiveSafePlayer(object thing){
        safeCount++;
        Debug.Log("player added to safe count");
        if(allPlayers.Count > 1 && safeCount > (int)blueTeam.Count/2)
            PhotonNetwork.LoadLevel("level2");
        else if(safeCount == redTeam.Count && safeCount != 0)
            PhotonNetwork.LoadLevel("level2");
    
        
    }
    

}

[System.Serializable]
public class PlayerInfo
{
    public string name;
    public int actor, team;
 

    public PlayerInfo(string name, int actor, int team)
    {
        this.name = name;
        this.actor = actor;
        this.team = team;
      
    }
}
