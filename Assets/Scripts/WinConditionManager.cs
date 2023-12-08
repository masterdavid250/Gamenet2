using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinConditionManager : MonoBehaviour
{
    public static WinConditionManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void PlayerWins()
    {
        ReturnToLobby();
    }

    private void ReturnToLobby()
    {
        SceneManager.LoadScene("LobbyScene");
        PhotonNetwork.LeaveRoom();
    }
}
