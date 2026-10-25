using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_DIALOGUE : MonoBehaviour
{
    private DialogueManager Dmanager;
    public int DialogueNumber = 0;
    public bool callAtStart = false;
    // Start is called before the first frame update
    void Start()
    {
        Dmanager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        if(callAtStart == true)
        {
            Dmanager.CallDialogueNPC(DialogueNumber, this);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Dmanager.currentNPC = this;
            Dmanager.canStartDialogue = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Dmanager.currentNPC = null;
            Dmanager.canStartDialogue = false;

        }
    }




}
