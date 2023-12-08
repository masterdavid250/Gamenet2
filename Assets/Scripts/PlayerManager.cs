using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable; 


public class PlayerManager : MonoBehaviourPunCallbacks
{
    int kills; 

    public void GetKill()
    {
        photonView.RPC("RPC_GetKill", photonView.Owner); 
    }

    [PunRPC]
    void RPC_GetKill()
    {
        kills++; 

        Hashtable hash = new Hashtable();
        hash.Add("kills", kills); 
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public static PlayerManager Find(Player player)
    {
        return FindObjectsOfType<PlayerManager>().SingleOrDefault(x => x.photonView.Owner == player); 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
