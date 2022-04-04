using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    public GameObject TitleScreen;
    private int idx = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        idx++;
        TitleScreen.gameObject.SetActive(false);
        transform.GetChild(idx).gameObject.SetActive(true);
    }


    public void NextPage()
    {
        transform.GetChild(idx).gameObject.SetActive(false);
        idx++;
        if (idx >= transform.childCount)
        {
            SceneManager.LoadScene("SampleScene");
            return;
        }
        transform.GetChild(idx).gameObject.SetActive(true);
    }
}
