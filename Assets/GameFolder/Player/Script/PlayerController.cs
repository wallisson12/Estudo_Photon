using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    public static int direction = 2;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _speedM;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Transform _spawnBullet;
    [SerializeField] private int life;

    [Header("Photon Settings")]
    [SerializeField] private PhotonView _photonView;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        if (_photonView.IsMine)
        {
            JumpPlayer();
            Flip();

            if (Input.GetKeyDown(KeyCode.X))
            {
                _photonView.RPC("OnFire", RpcTarget.All);

            }else if (Input.GetKeyDown(KeyCode.C))
            {
                _photonView.RPC("ChangeColor", RpcTarget.All);
            }

        }
    }

    void FixedUpdate()
    {
        if (_photonView.IsMine)
        {
            MovePlayer();
        }
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxisRaw("Horizontal");

        _rb.velocity = new Vector2(moveX*_speedM,_rb.velocity.y);
    }

    void JumpPlayer()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rb.velocity = Vector2.zero;
            _rb.AddForce(new Vector2(_rb.velocity.x,_jumpForce),ForceMode2D.Impulse);
        }
    }

    void Flip()
    {
        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            direction = 2;
            Vector3 scale = transform.localScale;
            scale.x = direction;
            transform.localScale = scale;
        }
        else if (Input.GetAxisRaw("Horizontal") == -1)
        {
            direction = -2;
            Vector3 scale = transform.localScale;
            scale.x = direction;
            transform.localScale = scale;
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    [PunRPC]
    public void OnFire(PhotonMessageInfo info)
    {
        //Uma forma de compensar o atraso
        float lag = (float) (PhotonNetwork.Time - info.SentServerTime);
        GameObject bulletOBJ = PhotonNetwork.Instantiate("Bullet", _spawnBullet.position,Quaternion.identity);
        bulletOBJ.GetComponent<Bullet>().Shoot(lag);
    }

    [PunRPC]
    public void ChangeColor(PhotonMessageInfo info)
    {
        GetComponent<SpriteRenderer>().color = new Color(Random.Range(0.0f,1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
    }
}
