using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Realtime;
using Unity.VisualScripting;

public class launcher : MonoBehaviourPunCallbacks //use this when using PUN
{
    public static launcher instance;
    [SerializeField] private GameObject loadingScreen, menuButtons, createRoomScreen, errorScreen, roomBrowserScreen, nameInputScreen, roomScreen;
    [SerializeField] private TMP_Text loadingText, playerNameLabel, errorText;
    [SerializeField] private TMP_InputField roomNameInput, nicknameInput;
    private List<TMP_Text> allPlayerNames = new List<TMP_Text>();
    private List<RoomButton> roomButtons = new List<RoomButton>();
    RoomButton defaultrb;
    bool nickname;

    void Start()
    {
        CloseMenus();
        loadingScreen.SetActive(false);
        loadingText.text = "Connecting to Network...";
        PhotonNetwork.ConnectUsingSettings();
        menuButtons.SetActive(true);
    }
    public void startGame()
    {
        PhotonNetwork.LoadLevel("Sample Scene");
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

    public void screen()
    {

        if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
            Screen.fullScreenMode = FullScreenMode.Windowed;
        else if (Screen.fullScreenMode == FullScreenMode.Windowed)
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;

    }
    public override void OnConnectedToMaster()
    {
        CloseMenus();
        menuButtons.SetActive(true);
        PhotonNetwork.AutomaticallySyncScene = true;

    }
    public void openCreateRoom()
    {
        CloseMenus();
        createRoomScreen.SetActive(true);
    }
    public void setNickName()
    {
        if (!string.IsNullOrEmpty(nicknameInput.text))
        {
            PhotonNetwork.NickName = nicknameInput.text;
            PlayerPrefs.SetString("playerName", nicknameInput.text);
            CloseMenus();
            nickname = true;
        }
    }
    public override void OnJoinedLobby()
    {
        CloseMenus();
        menuButtons.SetActive(true);
        PhotonNetwork.NickName = Random.Range(0, 1000f).ToString();
        listPlayers();

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
    private void listPlayers()
    {
        foreach(TMP_Text player in allPlayerNames)
        {
            Destroy(player.gameObject);
        }
        allPlayerNames.Clear();
        Player[] players = PhotonNetwork.PlayerList;

        for(int i = 0; i < players.Length; i++)
        {
            TMP_Text newPlayerLabel = Instantiate(playerNameLabel, playerNameLabel.transform.parent);
            newPlayerLabel.text = players[i].NickName;
            newPlayerLabel.gameObject.SetActive(true);
            allPlayerNames.Add(newPlayerLabel);
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        TMP_Text newPLayerLabel = Instantiate(playerNameLabel, playerNameLabel.transform.parent);
        newPLayerLabel.text = newPlayer.NickName;
        newPLayerLabel.gameObject.SetActive(true);
        allPlayerNames.Add(playerNameLabel);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(RoomButton rb in roomButtons)
        {
            Destroy(rb.gameObject);
        }
        roomButtons.Clear();
        for(int i = 0; i < roomList.Count; i++)
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
    public void joinRoom(RoomInfo inputInfo)
    {
        PhotonNetwork.JoinRoom(inputInfo.Name);
        CloseMenus();
        loadingText.text = "Joining...";
        loadingScreen.SetActive(true); 
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
    public override void OnCreateRoomFailed(short returnCode, string message)
    {

        errorText.text = "Failed to Create Room: " + message;
        CloseMenus();
        errorScreen.SetActive(true);


    }
}
