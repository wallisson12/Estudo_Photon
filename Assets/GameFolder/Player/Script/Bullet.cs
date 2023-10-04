using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speedB;
    [SerializeField] private Rigidbody2D _rbB;

    void Start()
    {
        Destroy(gameObject,5f);    
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<PlayerController>()._photonView.RPC("Die", RpcTarget.All);
            //other.GetComponent<PlayerController>().Die();
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [PunRPC]
    public void Shoot(float lag)
    {
        _rbB = GetComponent<Rigidbody2D>();
        _rbB.velocity = new Vector2(PlayerController.direction * _speedB,0f);
        _rbB.AddTorque(Random.Range(-20f,20f));
        _rbB.position += _rbB.velocity * lag;
    }
}
