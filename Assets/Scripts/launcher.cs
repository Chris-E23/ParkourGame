using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Realtime;
using System.Runtime.CompilerServices;

public class launcher : MonoBehaviourPunCallbacks //use this when using PUN
{
    public static launcher instance;
    [SerializeField] private GameObject loadingScreen, menuButtons, createRoomScreen, errorScreen, roomBrowserScreen, nameInputScreen, roomScreen, startButton, settingsScreen;
    [SerializeField] private TMP_Text loadingText, playerNameLabel, errorText, roomName;
    [SerializeField] private TMP_InputField roomNameInput, nicknameInput;
    private List<TMP_Text> allPlayerNames = new List<TMP_Text>();
    private List<RoomButton> roomButtons = new List<RoomButton>();
    [SerializeField] private RoomButton defaultrb;
    bool nickname;

    void Start()
    {
        instance = this;
        CloseMenus();
        loadingScreen.SetActive(true);
        loadingText.text = "Connecting to Network...";
        if (!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }
    public void startGame()
    {
        //PhotonNetwork.CurrentRoom.isOpen = false; 
         PhotonNetwork.CurrentRoom.IsVisible = false;

        PhotonNetwork.LoadLevel("SampleScene");

    
    }
    void CloseMenus()
    {
        loadingScreen.SetActive(false);
        menuButtons.SetActive(false);
        createRoomScreen.SetActive(false);
        roomScreen.SetActive(false);
        errorScreen.SetActive(false);
        roomBrowserScreen.SetActive(false);
        nameInputScreen.SetActive(false);
        settingsScreen.SetActive(false);

    }
    public void openRB()
    {
        CloseMenus();
        roomBrowserScreen.SetActive(true);
    }
    public void closeRB()
    {
        
        CloseMenus();
        menuButtons.SetActive(true);
    }
    public void quit()
    {
        Application.Quit();
    }

    public void leaveRoom(){
        
        PhotonNetwork.LeaveRoom();
        CloseMenus();
    }
    public override void OnLeftRoom(){


    }
    public void screen()
    {

        if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
            Screen.fullScreenMode = FullScreenMode.Windowed;
        else if (Screen.fullScreenMode == FullScreenMode.Windowed)
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;

    }

    public void openCreateRoom()
    {
        CloseMenus();
        createRoomScreen.SetActive(true);
    }
    public void openSettings()
    {
        CloseMenus();
        settingsScreen.SetActive(true);
    }
    public void setNickName()
    {
        if (!string.IsNullOrEmpty(nicknameInput.text))
        {
            PhotonNetwork.NickName = nicknameInput.text;
            PlayerPrefs.SetString("playerName", nicknameInput.text);
            CloseMenus();
            menuButtons.SetActive(true);
            nickname = true;
        }
    }
    //ALL IMPORTANT FUNCTIONS!!
    public override void OnJoinedLobby() //When you load into the menu
    {
        CloseMenus();
        menuButtons.SetActive(true);
        PhotonNetwork.NickName = UnityEngine.Random.Range(0, 1000f).ToString();
        //listPlayers();

        if (!nickname)
        {
            CloseMenus();
            nameInputScreen.SetActive(true);
            if (PlayerPrefs.HasKey("playerName"))
            {
                nicknameInput.text = PlayerPrefs.GetString("playerName");
            }
        }
        else
        {
            PhotonNetwork.NickName = PlayerPrefs.GetString("playerName");
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer) //whenever the play joins a room
    {
        TMP_Text newPLayerLabel = Instantiate(playerNameLabel, playerNameLabel.transform.parent);
        newPLayerLabel.text = newPlayer.NickName;
        newPLayerLabel.gameObject.SetActive(true);
        allPlayerNames.Add(playerNameLabel);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList) // update the room buttons on the scroll view 
    {
        foreach(RoomButton rb in roomButtons)
        {
            Destroy(rb.gameObject);
        }
        roomButtons.Clear();
        defaultrb.gameObject.SetActive(false);

        for (int i = 0; i < roomList.Count; i++)
        {
            
            if (roomList[i].PlayerCount != roomList[i].MaxPlayers && !roomList[i].RemovedFromList)
            {
                RoomButton newRoom = Instantiate(defaultrb, defaultrb.transform.parent); //adds it to the list 
                newRoom.SetButtonDetails(roomList[i]); //set its info
                newRoom.gameObject.SetActive(true); //makes its active
                roomButtons.Add(newRoom);

            }
        }
    }
   
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);

        }
    }
    public override void OnJoinedRoom()
    {
        CloseMenus();
        roomScreen.SetActive(true);
        roomName.text = roomNameInput.text;
        listPlayers();
        if (PhotonNetwork.IsMasterClient)
            startButton.SetActive(true);
        else
            startButton.SetActive(false);

    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {

        errorText.text = "Failed to Create Room: " + message;
        CloseMenus();
        errorScreen.SetActive(true);


    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
        loadingText.text = "Joining Lobby...";

    }
    public void QuickJoin()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 8;
        PhotonNetwork.CreateRoom("Test Room", options);
        CloseMenus();
        loadingText.text = "Creating Room..";
        loadingScreen.SetActive(true);

    }
    public void CreateRoom()
    {
        if (!string.IsNullOrEmpty(roomNameInput.text)){
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 8; //maybe add something to limit how many players there are 
            PhotonNetwork.CreateRoom(roomNameInput.text, options);
            loadingText.text = "Creating Room";
            loadingScreen.SetActive(true);
        }

    }
    public void joinRoom(RoomInfo inputInfo)
    {
        PhotonNetwork.JoinRoom(inputInfo.Name);
        CloseMenus();
        loadingText.text = "Joining...";
        loadingScreen.SetActive(true);
    }
    private void listPlayers()
    {
        foreach (TMP_Text player in allPlayerNames)
        {
            Destroy(player.gameObject);
        }
        allPlayerNames.Clear();
        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Length; i++)
        {
            TMP_Text newPlayerLabel = Instantiate(playerNameLabel, playerNameLabel.transform.parent);
            newPlayerLabel.text = players[i].NickName;
            newPlayerLabel.gameObject.SetActive(true);
            allPlayerNames.Add(newPlayerLabel);
        }
    }
}
