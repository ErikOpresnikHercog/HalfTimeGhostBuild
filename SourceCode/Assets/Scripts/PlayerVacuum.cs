using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVacuum : MonoBehaviour
{
    public Player player;
    public float x, y;
    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }
    private void Update()
    {
        if (x == 0 && y == 0)
        {
            gameObject.LeanRotateZ(0, 0.1f);
        }
        if (x == 1)
        {
            gameObject.LeanRotateZ(90, 0.1f);
        }
        if (x == -1)
        {
            gameObject.LeanRotateZ(-90, 0.1f);
        }
        if (y == 1)
        {
            gameObject.LeanRotateZ(180, 0.1f);
        }
        if (y == -1)
        {
            gameObject.LeanRotateZ(0, 0.1f);
        }
        if (x > 0.5 && y > 0.5 )
        {
            gameObject.LeanRotateZ(120, 0.1f);
        }
        if (x < -0.5 && y > 0.5)
        {
            gameObject.LeanRotateZ(-120, 0.1f);
        }
        if (x == -1)
        {
            gameObject.LeanRotateZ(-90, 0.1f);
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "ghost")
        {
            player.GhostsCollected += 1;
            Destroy(col.gameObject);
        }
    }
}
