using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public LevelLoader levelLoader;
    private void Awake()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        //SceneManager.LoadScene(1);
        levelLoader.LoadNextLevel(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
