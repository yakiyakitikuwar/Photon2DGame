
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
}
