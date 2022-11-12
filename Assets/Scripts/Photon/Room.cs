

using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class Room : MonoBehaviour
{
    public Text ButtonText;
    private RoomInfo info;
    public void RegisterRommDetails(RoomInfo info)
    {
        this.info=info;

        ButtonText.text=this.info.Name;
    }
    public void OpenRoom()
    {
        PhotonManager.instance.JoinRoom(info);
    }
    
}
