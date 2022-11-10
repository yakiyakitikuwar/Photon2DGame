using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Id{get;private set;}
    public int OwnerId{get;private set;}
    public bool Equals(int id, int ownerId) => id == Id && ownerId == OwnerId;
    
   
    private Vector3 velocity;

    public void Init(int id,int ownerid,Vector3 origin, float angle) {
        Id=id;
        OwnerId=ownerid;
        transform.position = origin;
        velocity = 9f * new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    private void Update() {
        transform.Translate(velocity * Time.deltaTime);
    }

    // 画面外に移動したら削除する
    // （Unityのエディター上ではシーンビューの画面も影響するので注意）
    private void OnBecameInvisible() {
        Destroy(gameObject);
    }
}
