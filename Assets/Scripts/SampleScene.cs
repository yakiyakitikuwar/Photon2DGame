using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SampleScene : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.NickName="Player";
        var hashtable=new ExitGames.Client.Photon.Hashtable();
        hashtable["Scroe"]=0;
        hashtable["Message"]="こんにちわ";
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
    }
    public override void OnJoinedRoom()
    {
        var position=new Vector3(Random.Range(-5f,5f),Random.Range(-5f,5f));
        PhotonNetwork.Instantiate("Avatar",position,Quaternion.identity);
        Debug.Log(PhotonNetwork.NickName);
    }
}
