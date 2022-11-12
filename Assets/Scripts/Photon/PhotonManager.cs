
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager instance;
    public GameObject loadingPanel;
    public Text loadingText;
    public GameObject buttons;
    public GameObject createRoomPanel;
    public Text enterRoomName;
    public GameObject roomPanel;
    public Text roomName;
    public GameObject ErrorPanel;
    public Text ErrorText;
    public GameObject roomlistPanel;
    public Room originalRoomButton;
    public GameObject roomButtonContent;
    Dictionary<string,RoomInfo>roomsList=new Dictionary<string, RoomInfo>();
    private List<Room>allRoomButtons=new List<Room>();
    public Text playerNameText;

    private List<Text>allPlayerNames=new List<Text>();

    public GameObject PlayerNameContent;
    public GameObject nameInputPanel;
    public Text PlacehooderText;
    public InputField nameInput;
    private bool setName;

    private void  Awake()
    {
        instance=this;
    }
    void Start()
    {
        CloseMenuUI();
        loadingPanel.SetActive(true);
        loadingText.text="ネットワークに接続中";
        if(!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    public void CloseMenuUI()
    {
        loadingPanel.SetActive(false);
        buttons.SetActive(false);
        createRoomPanel.SetActive(false);
        roomPanel.SetActive(false);
        ErrorPanel.SetActive(false);
        roomlistPanel.SetActive(false);
        nameInputPanel.SetActive(false);
    }
    public void LobbyMenuDisplay()
    {
        CloseMenuUI();
        buttons.SetActive(true);
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        loadingText.text="ロビーに参加中";
    }
    public override void OnJoinedLobby()
    {
        LobbyMenuDisplay();
        roomsList.Clear();
        PhotonNetwork.NickName=Random.Range(0,100).ToString();
        ConfirmationName();
    }
    public void OpenCreatRoomPanel()
    {
        CloseMenuUI();
        createRoomPanel.SetActive(true);
    }
    public void CreatRoomButton()
    {
        if(!string.IsNullOrEmpty(enterRoomName.text))
        {
            RoomOptions options=new RoomOptions();
            options.MaxPlayers=8;
            PhotonNetwork.CreateRoom(enterRoomName.text,options);
            CloseMenuUI();
            loadingText.text="ルーム作成中";
            loadingPanel.SetActive(true);
        }
    }
    public override void OnJoinedRoom()
    {
        CloseMenuUI();
        roomPanel.SetActive(true);
        roomName.text=PhotonNetwork.CurrentRoom.Name;
        GetAllPlayer();
    }
    public void LeavRoom()
    {
        PhotonNetwork.LeaveRoom();
        CloseMenuUI();
        loadingText.text="退出中";
        loadingPanel.SetActive(true);
    }
    public override void OnLeftRoom()
    {
        LobbyMenuDisplay();
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        CloseMenuUI();
        ErrorText.text="ルームの作成に失敗しました"+message;
        ErrorPanel.SetActive(true);
    }
    public void FindRoom()
    {
        CloseMenuUI();
        roomlistPanel.SetActive(true);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {//ルームリストに更新があったときに呼ばれる関数
        RoomUiinitialization();
        UpdateRoomList(roomList);
    }
    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        for(int i=0;i<roomList.Count;i++)
        {
            RoomInfo info=roomList[i];
            if(info.RemovedFromList)
            {
                roomList.Remove(null);
            }
            else
            {
                roomsList[info.Name]=info;
            }
        }
        RoomListDisplay(roomsList);
    }
    
    public void RoomListDisplay(Dictionary<string,RoomInfo> cachedRoomList)
    {
        foreach(var roomInfo in cachedRoomList)
        {
            Room newButton=Instantiate(originalRoomButton);
            newButton.RegisterRommDetails(roomInfo.Value);
            newButton.transform.SetParent(roomButtonContent.transform);
            allRoomButtons.Add(newButton);
        }
    }
    public void RoomUiinitialization()
    {
        foreach(Room rm in allRoomButtons)
        {
            Destroy(rm.gameObject);
        }
        allRoomButtons.Clear();
    }
    public void JoinRoom(RoomInfo roomInfo)
    {
        PhotonNetwork.JoinRoom(roomInfo.Name);
        CloseMenuUI();
        loadingText.text="ルーム参加中";
        loadingPanel.SetActive(true);
    }
    public void GetAllPlayer()
    {
        InitilizePlayerList();
        PlayerDisplay();
    }
    public void InitilizePlayerList()
    {
        foreach(var rm in allPlayerNames)
        {
            Destroy(rm.gameObject);
        }
        allPlayerNames.Clear();
    }
    public void PlayerDisplay()
    {
        foreach(var players in PhotonNetwork.PlayerList)
        {
            PlayerTextGetneration(players);
        }
    }
    public void PlayerTextGetneration(Player player)
    {
        Text newPlayerText=Instantiate(playerNameText);
        newPlayerText.text=player.NickName;
        newPlayerText.transform.SetParent(PlayerNameContent.transform);
        allPlayerNames.Add(newPlayerText);
    }
    public void ConfirmationName()
    {
        if(!setName)
        {
            CloseMenuUI();
            nameInputPanel.SetActive(true);
            if(PlayerPrefs.HasKey("playerName"))
            {
                PlacehooderText.text=PlayerPrefs.GetString("playerName");
                nameInput.text=PlayerPrefs.GetString("playerName");
            }
        }
        else
        {
            PhotonNetwork.NickName=PlayerPrefs.GetString("playerName");
        }
    }
    public void SetName()
    {
        if(!string.IsNullOrEmpty(nameInput.text))
        {
            PhotonNetwork.NickName=nameInput.text;
            PlayerPrefs.SetString("playerName",nameInput.text);
            LobbyMenuDisplay();
            setName=true;
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerTextGetneration(newPlayer);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        GetAllPlayer();
    }
}
