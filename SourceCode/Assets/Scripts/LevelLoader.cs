using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator transition;
    public bool HasEnded = false;
    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("LevelLoader");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadNextLevel(int Id)
    {
        StartCoroutine(LoadLevel(Id));
        transition.enabled = true;
        transition.SetTrigger("Start");
    }
    IEnumerator LoadLevel(int SceneIndex)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneIndex);
        StartCoroutine(Endtrans());
    }
    IEnumerator Endtrans()
    {
        yield return new WaitForSeconds(1.1f);
        transition.ResetTrigger("Start");
    }
}
