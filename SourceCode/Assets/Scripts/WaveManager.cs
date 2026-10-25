using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public GameObject GhostSpawnGroup;
    public List<Transform> GhostSpawnLoc;
    Main main;
    Player player;
    TMP_Text endText;
    TMP_Text endSubText;
    public int GhostNumber;
    int GhostsSpawned;
    public GameObject ForestGhost;
    public GameObject target;
    public int GhostsInHouse;
    bool CanSpawn = false;
    public GameObject GameEndUI;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        main = FindObjectOfType<Main>();
        endText = GameEndUI.transform.GetChild(0).GetComponent<TMP_Text>();
        endSubText = GameEndUI.transform.GetChild(1).GetComponent<TMP_Text>();
        for (int i = 0; i < GhostSpawnGroup.transform.childCount; i++)
        {
            GhostSpawnLoc.Add(GhostSpawnGroup.transform.GetChild(i));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CanSpawn == true)
        {
            print("startwait");
            print(CanSpawn);
            CanSpawn = false;
            StartCoroutine(WaitSpawn());
        }
    }

    public void WaveStart()
    {
        CanSpawn = true;
        GhostsSpawned = 0;
        GhostsInHouse = 0;
        player.GhostsCollected = 0;
    }

    public void WaveEnd()
    {
        if (GhostsSpawned == player.GhostsCollected)
        {
            WaveWon();
        }
        else
        {
            WaveLost();
        }
        StartCoroutine(WaitPullUpUI());
    }

    public void WaveWon()
    {
        GameEndUI.SetActive(true);
        //GameEndUI.LeanMoveLocalY(5, 3);
        endText.text = "Victory";
        endSubText.text = "You caught all the ghosts!";
        main.AreasFinished[0] = true;
    }
    public void WaveLost()
    {
        GameEndUI.SetActive(true);
        //GameEndUI.LeanMoveLocalY(5, 3);
        endText.text = "Defeat";
        endSubText.text = "You didnt catch all the ghosts!";
    }
    IEnumerator WaitSpawn()
    {
        yield return new WaitForSeconds(5);
        if(GhostsSpawned <= GhostNumber)
        {
            GameObject ghost;
            ghost = Instantiate(ForestGhost);
            ghost.GetComponent<Transform>().position = GhostSpawnLoc[Random.Range(0, GhostSpawnLoc.Capacity)].position;
            ghost.GetComponent<AIPath>().maxSpeed = Random.Range(0.5f, 1);
            ghost.GetComponent<AIDestinationSetter>().target = target.transform;
            GhostsSpawned++;
            CanSpawn = true;
            print("spawned");
        }
    }
    IEnumerator WaitPullUpUI()
    {
        yield return new WaitForSeconds(3);
        //GameEndUI.LeanMoveY(125, 3);
        GameEndUI.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "ghost")
        {
            print("ghost");
            GhostsInHouse += 1;
            Destroy(col.gameObject);
        }
    }
}
