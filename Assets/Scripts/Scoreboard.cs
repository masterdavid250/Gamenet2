using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime; 
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;

public class Scoreboard : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI usernameText;
    public TextMeshProUGUI killsText; 
    public TextMeshProUGUI deathsText;

    Player player; 

    public void Initialize(Player player)
    {
        usernameText.text = player.NickName;
        this.player = player; 
        UpdateStats();
    }

    public void UpdateStats()
    {
        if (player.CustomProperties.TryGetValue("kills", out object kills))
        {
            killsText.text = kills.ToString();
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer == player)
        {
            if (changedProps.ContainsKey("kills"))
            {
                UpdateStats(); 
            }
        }
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
