using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditSceneScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(CreditsFunc());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator CreditsFunc()
    {
        yield return new WaitForSeconds(15f);
        SceneManager.LoadScene("StartMenu");
    }
}
