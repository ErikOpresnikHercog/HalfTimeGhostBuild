using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public LevelLoader levelLoader;
    private void Awake()
    {
        levelLoader = FindObjectOfType<LevelLoader>();

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        levelLoader.LoadNextLevel(0);
    }

}
