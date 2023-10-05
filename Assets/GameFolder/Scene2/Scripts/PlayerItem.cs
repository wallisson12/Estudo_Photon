using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _namePlayer;
    [SerializeField] private Image bgImage;
    [SerializeField] private Color hightLightColor;
    [SerializeField] private GameObject leftBTN,rightBTN;

    public void SetAttributesPlayer(Player _player)
    {
        _namePlayer.text = _player.NickName;
    }

    public void ApplyLocalChanges()
    {
        bgImage.color = hightLightColor;
        leftBTN.SetActive(true);
        rightBTN.SetActive(true);
    }
}
