using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public List<DialogueAsset> Dialogues = new List<DialogueAsset>();
    public List<int> DialoguePositions = new List<int>();
    public NPC_DIALOGUE currentNPC;
    public GameObject player;
    public GameObject bubblePrefab;
    public bool canDialogue;
    public bool canStartDialogue;
    public int currentDialogueNumber;
    public bool isDialogue;
    public bool IsDialogueFirst = true;
    public int currentPosition = 0;
    string[] splitText;
    List<GameObject> InstantiatedBubbles = new List<GameObject>();
    InventoryManager inventoryManager;
    private void Awake()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        inventoryManager.AddItem(5, 20);
        inventoryManager.AddItem(1, 50);
        inventoryManager.AddItem(1, 60);
        inventoryManager.RemoveItem(1, 120);
    }

    // Update is called once per frame
    void Update()
    {
        if(canDialogue && canStartDialogue)
        {
            isDialogue = true;
            canStartDialogue = false;
            currentDialogueNumber = currentNPC.DialogueNumber;
            ShowBubble();
        }
    }

    public void ShowBubble()
    {
        //print("show bubble" + canDialogue);
        if (isDialogue)
        {
            if(IsDialogueFirst == true)
            {
                currentPosition = DialoguePositions[currentDialogueNumber];
                IsDialogueFirst = false;
            }
            splitText = Dialogues[currentDialogueNumber].DialogueStrings[currentPosition].Split('>');
            string prefix = splitText[0];
            splitText[1] = splitText[1].Substring(0);

            switch (prefix[0])
            {
                case '1':
                    InstantiateBubble(player);
                    currentPosition += 1;
                    break;
                case '0':
                    InstantiateBubble(currentNPC.gameObject);
                    currentPosition += 1;
                    break;
                case 'E':
                    isDialogue = false;
                    IsDialogueFirst = true;
                    currentPosition = 0;
                    foreach (GameObject bubble in InstantiatedBubbles)
                    {
                        Destroy(bubble);
                    }
                    currentNPC = null;
                    break;

            }

            if (prefix.Length > 1)
            {
                string[] WholeArguments;
                string[] arguments;
                WholeArguments = prefix.Split('_');
                arguments = WholeArguments[1].Split(' ');
                if (prefix[1] == '_')
                {
                    print("yes");
                    foreach (string item in arguments)
                    {
                        print(item.Substring(0, 5));
                        switch (item.Substring(0, 5))
                        {
                            case "nextp":
                                SetNextPos(item.Substring(5, 2), false);
                                break;
                            case "savep":
                                SetNextPos(item.Substring(5, 2), true);
                                break;
                            case "givei":
                                //give item
                                break;
                            case "takei":
                                //take item
                                break;
                            case "check":
                                //check for item
                                break;

                        }
                    }
                }
            }
        }
    }

    public void InstantiateBubble(GameObject dialogueSpeaker)
    {
        foreach (GameObject bubble in InstantiatedBubbles)
        {
            Destroy(bubble);
        }
        GameObject speechBubble = Instantiate(bubblePrefab, GameObject.Find("GeneralCanvas").transform);
        speechBubble.transform.position = Camera.main.WorldToScreenPoint(dialogueSpeaker.gameObject.transform.position + new Vector3(0f, 1f));
        speechBubble.GetComponent<TextManager>().ChangeText(splitText[1]);
        InstantiatedBubbles.Add(speechBubble);
    }

    public void SetNextPos(string posNum, bool doSave)
    {
        int nextPosition = int.Parse(posNum.Substring(0, 2));
        print(posNum);
        if (doSave)
        {
            DialoguePositions[currentDialogueNumber] = nextPosition;
        }
        else
        {
            currentPosition = nextPosition;
        }
    }

    public void ExtendedSpeak(string prefixPars)
    {
        if (prefixPars.Substring(2, 2) != "XX")
        {
            currentPosition = int.Parse(prefixPars.Substring(2, 2)) +1;
        }
        else
        {
            currentPosition += 1;
        }

        if (prefixPars[4] == 'S')
        {
            if (prefixPars.Length > 5)
            { 
                if(prefixPars[5] != 'A')
                {
                    DialoguePositions[currentDialogueNumber] = int.Parse(prefixPars.Substring(5, 2));
                }
            }
            else
            {
                DialoguePositions[currentDialogueNumber] = currentPosition - 1;
            }
        }
    }
    public void CallDialogueNPC(int DialogueID, NPC_DIALOGUE NPC)
    {
        currentDialogueNumber = DialogueID;
        currentNPC = NPC;
        isDialogue = true;
        canStartDialogue = false;
        ShowBubble();
    }

}

