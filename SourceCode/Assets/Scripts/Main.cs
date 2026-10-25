using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TarodevController;
using UnityEngine.UI;
public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerController playerController;
    public GameObject Player;
    public GameObject StartButton;
    public LevelLoader levelLoader;
    public WaveManager waveManager;
    public bool IsDay = true;
    public Image nightPanel;
    bool CanTran = true;
    public Animator nightTransition;
    public bool HasEnded = false;
    public bool[] AreasFinished;
    public int currentAreaID;
    private void Awake()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        waveManager = FindObjectOfType<WaveManager>();

    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsDay && CanTran)
        {
            StartCoroutine(WaitNight());
            CanTran = false;
        }
        else if(IsDay == false && CanTran)
        {
            StartCoroutine(WaitDay());
            CanTran = false;
        }
        if (CanTran)
        {
           
        }
    }

    IEnumerator WaitNight()
    {
        yield return new WaitForSeconds(10);
        IsDay = false;
        CanTran = true;
        FadeScene();
        if(AreasFinished[currentAreaID] == false)
        {
            waveManager.WaveStart();
        }
        
    }
    IEnumerator WaitDay()
    {
        yield return new WaitForSeconds(50);
        IsDay = true;
        CanTran = true;
        UnFadeScene();
        waveManager.WaveEnd();

    }
    IEnumerator Waitdisable()
    {
        yield return new WaitForSeconds(1);
        nightTransition.enabled = false;

    }

    public void RestartLevel()
    {
        //SceneManager.LoadScene(1);
        levelLoader.LoadNextLevel(1);
    }
    public void LoadMainMenu()
    {
        //SceneManager.LoadScene(0);
        levelLoader.LoadNextLevel(0);
    }
    public void FadeScene()
    {
        nightTransition.SetTrigger("Start");
        StartCoroutine(Waitdisable());
    }
    public void UnFadeScene()
    {
        nightTransition.enabled = true;
        nightTransition.ResetTrigger("Start");
    }


}
