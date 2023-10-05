using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] playerPrefabs;

    void Start()
    {
        GameObject playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
        PhotonNetwork.Instantiate(playerToSpawn.name, new Vector2(Random.Range(-8f, 8f), Random.Range(-3f, 3f)),Quaternion.identity);
    }
}
