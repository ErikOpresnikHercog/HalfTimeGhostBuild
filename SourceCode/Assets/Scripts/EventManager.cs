using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;
using Cinemachine;


public class EventManager : MonoBehaviour
{
    Main main;
    public GameObject FinishText;
    public GameObject Camera;
    void Start()
    {
        main = FindObjectOfType<Main>();
        FinishText.SetActive(false);
    }

    void Update()
    {
        
    }

    public GameObject CollideEvent(GameObject Collider)
    {
        if (Collider.tag == "DeathFloor")
        {
            //StartCoroutine(WaitDisCamera());
            main.RestartLevel();
            //StartCoroutine(WaitRespawn());
        }
        else if (Collider.tag == "Finish")
        {
            StartCoroutine(WaitUI(FinishText));
        }
        return null;
    }

    IEnumerator WaitUI(GameObject Enable)
    {
        yield return new WaitForSeconds(2f);
        //Enable.SetActive(true);
        main.LoadMainMenu();
    }
    IEnumerator WaitRespawn()
    {
        yield return new WaitForSeconds(1f);
        
    }
    IEnumerator WaitDisCamera()
    {
        yield return new WaitForSeconds(0.2f);
        //Camera.GetComponent<CinemachineBrain>().enabled = false;
    }
}
