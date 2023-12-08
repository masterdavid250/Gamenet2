using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime; 
using TMPro;
using Photon.Pun.UtilityScripts;

public class Shooting : MonoBehaviourPunCallbacks
{
    public Camera camera;
    public GameObject hitEffectPrefab;

    /*public Transform Waypoint1;
    public Transform Waypoint2;
    public Transform Waypoint3;
    public Transform Waypoint4;*/

    [Header("HP Related Stuff")]
    public float startHealth = 100;
    private float health;
    public Image healthBar;
    public TextMeshProUGUI playerName; 

    [Header("Score")]
    //public float userStartingScore = 0;
    //private float userScore;
    public TextMeshProUGUI userScoreText;
    public TextMeshProUGUI victoryText;
    public TextMeshProUGUI killFeedText;

    private Animator animator; 
    private bool isScoreRecorded = false;

    //
    //PlayerManager playerManager;

    // Start is called before the first frame update
    void Start()
    {
        health = startHealth;
        healthBar.fillAmount = health / startHealth;  
        animator = this.GetComponent<Animator>();

        playerName.text = photonView.Owner.NickName; 


        //Scoring
        //userScore = userStartingScore;
        
    }

    // Update is called once per frame
    void Update()
    {
        //userScoreText.text = "Score: " + photonView.Owner.GetScore();
        if (photonView.Owner.GetScore() >= 10)
        {
            victoryText.text = photonView.Owner.NickName + " wins the Game";

            Invoke("ActivatePlayerWin", 2f); 
            photonView.Owner.SetScore(0); 
        }
    }

    public void ActivatePlayerWin()
    {
        WinConditionManager.Instance.PlayerWins();
    }

    public void Fire()
    {
        RaycastHit hit;
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        //photonView.RPC("GetUserScore", RpcTarget.All); 
        
        

        if (Physics.Raycast(ray, out hit, 200))
        {
            Debug.Log(hit.collider.gameObject.name);

            //All Players can see particles
            photonView.RPC("CreateHitEffects", RpcTarget.All, hit.point); 

            if (hit.collider.gameObject.CompareTag("Player") && !hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
            {
                hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 25); 
                
                //hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", photonView.Owner, 25);
            }
        }
    }

    [PunRPC]
    public void TakeDamage(int damage, PhotonMessageInfo info)
    {
        this.health -= damage; 
        this.healthBar.fillAmount = health / startHealth;
        
        if (health <= 0)
        {
            Die();
            //PlayerManager.Find(info.Sender).GetKill(); 
            Debug.Log(info.Sender.NickName + " killed " + info.photonView.Owner.NickName);
            killFeedText.text = info.Sender.NickName + " killed " + info.photonView.Owner.NickName;
            if (!isScoreRecorded)
            {
                info.Sender.AddScore(1);
                isScoreRecorded = true;
            }
                
            Invoke("ClearFeed", 1f);

            //
            
            

        }
    }

    public void ClearFeed()
    {
        killFeedText.text = ""; 
    }

    /*[PunRPC]
    public void GetUserScore()
    {
        
    }*/

    [PunRPC]
    public void CreateHitEffects(Vector3 position)
    {
        GameObject hitEffectGameObject = Instantiate(hitEffectPrefab, position, Quaternion.identity);
        Destroy(hitEffectGameObject, 0.2f);
    }

    public void Die()
    {
        if (photonView.IsMine)
        {
            animator.SetBool("isDead", true);
            DeathDelay(); 
            //RespawnCharacter(); 
            //StartCoroutine(RespawnCountdown());
        }
    }

    public void DeathDelay()
    {
        transform.GetComponent<PlayerMovementController>().enabled = false;
        Invoke("RespawnCharacter", 2f); 
    }

    public void RespawnCharacter()
    {
        animator.SetBool("isDead", false);
        isScoreRecorded = false; 

        /*float waypointX = 0;
        float waypointZ = 0;
        int randomizerSpawn = Random.Range(0, 4);

        //Waypoint1 
        if (randomizerSpawn == 0)
        {
            waypointX = -25.2f;
            waypointZ = 20.23f;
        }
        else if (randomizerSpawn == 1)
        {
            waypointX = 17.73f;
            waypointZ = 29.74f;
        }
        else if (randomizerSpawn == 2)
        {
            waypointX = 17.73f;
            waypointZ = -34.9f;
        }
        else if (randomizerSpawn == 3)
        {
            waypointX = -19.701f;
            waypointZ = -34.9f;
        }*/

        //int randomPointX = Random.Range(-20, 20);
        //int randomPointZ = Random.Range(-20, 20);

        //this.transform.position = new Vector3(waypointX, 0, waypointZ);

        this.transform.position = SpawnManager.instance.GetRandomXZCoordinate();  //Vector3(waypointX, 0, waypointZ);
        transform.GetComponent<PlayerMovementController>().enabled = true;

        photonView.RPC("RegainHealth", RpcTarget.AllBuffered);
    }

    IEnumerator RespawnCountdown()
    {
        GameObject respawnText = GameObject.Find("RespawnText"); 
        float respawnTime = 5.0f; 

        while (respawnTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            respawnTime--;

            transform.GetComponent<PlayerMovementController>().enabled = false;
            respawnText.GetComponent<Text>().text = "You are killed. Respawning in " + respawnTime.ToString(".00"); 
        }

        animator.SetBool("isDead", false);
        respawnText.GetComponent<Text>().text = "";

        int randomPointX = Random.Range(-20, 20);
        int randomPointZ = Random.Range(-20, 20);

        this.transform.position = new Vector3(randomPointX, 0, randomPointZ);
        transform.GetComponent<PlayerMovementController>().enabled = true;

        photonView.RPC("RegainHealth", RpcTarget.AllBuffered); 
    }

    [PunRPC]
    public void RegainHealth()
    {
        health = 100;
        healthBar.fillAmount = health / startHealth;
    }
}
