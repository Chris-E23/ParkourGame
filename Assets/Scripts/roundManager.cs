using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; 
using UnityEngine.SceneManagement;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class roundManager : MonoBehaviourPunCallbacks, IOnEventCallback
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
        TimerSync

    }

    public List<PlayerInfo> allPlayers = new List<PlayerInfo>();
    
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
            playerSend(PhotonNetwork.NickName);
            state = GameState.Playing;
        }
    }
    public override void OnEnable(){
        PhotonNetwork.AddCallbackTarget(this);

    }
    public override void OnDisable(){
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    void Update()
    {
       
    }
    public void OnEvent(EventData photonEvent){
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
              
            }
        }
    }

    
    public void playerSend(string name){
        object[] package = new object[3];
        package[0] = name; 
        package[1] = PhotonNetwork.LocalPlayer.ActorNumber;
        GameObject player;
        if(allPlayers.Count%2 == 0)
            package[2] = 0;
        else
            package[2] = 1;
        //send this to the masterclient and raise event 
        PhotonNetwork.RaiseEvent((byte)EventCodes.NewPlayer, package, new RaiseEventOptions{Receivers = ReceiverGroup.MasterClient}, new SendOptions {Reliability = true});

    }
    public void playerReceive(object[] dataReceived){
        PlayerInfo player = new PlayerInfo((string)dataReceived[0], (int)dataReceived[1], (int)dataReceived[2]);
        allPlayers.Add(player);
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
                index = i - 1;
            }
        }
    }
    
    public int getTeam(int actorNumber){
        foreach(PlayerInfo player in allPlayers){
            if(player.actor == actorNumber){
                return player.actor;
            }

        }
        return -1;
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
