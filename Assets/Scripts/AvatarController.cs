using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class AvatarController : MonoBehaviourPunCallbacks,IPunObservable
{
    private const float Maxstamina=6f;
    [SerializeField] private Image staminaBar=default;
    private float currentStamina=Maxstamina;
    // Start is called before the first frame update
   void Start()
   {
       if(PhotonNetwork.IsMasterClient)
       {
        Debug.Log("Masterだよ");
        
       }
       Player owner=photonView.Owner;
       Debug.Log(owner.NickName+photonView.OwnerActorNr);
   }

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine)
        {
            var input=new Vector3(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"),0);
            if(input.sqrMagnitude>0f)
            {
                currentStamina=Mathf.Max(0f,currentStamina-Time.deltaTime);
                transform.Translate(6f*Time.deltaTime*input.normalized);    
            }
            else
            {
                currentStamina=Mathf.Min(currentStamina+Time.deltaTime*2,Maxstamina);
            }
            
        }
        if (Input.GetMouseButtonDown(0)) {
            photonView.RPC(nameof(RpcSendMessage), RpcTarget.All, "こんにちは");
        }
        staminaBar.fillAmount=currentStamina/Maxstamina;
    }
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    if (stream.IsWriting) 
    {
            // 自身のアバターのスタミナを送信する
            stream.SendNext(currentStamina);
    } else 
    {
            // 他プレイヤーのアバターのスタミナを受信する
            currentStamina = (float)stream.ReceiveNext();
    }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName+"が参加しました");
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName+"が退出したよ");
    }
    [PunRPC]
    private void RpcSendMessage(string message, PhotonMessageInfo info) {
        // メッセージを送信したプレイヤー名も表示する
        Debug.Log($"{info.Sender.NickName}: {message}");
    }
}
