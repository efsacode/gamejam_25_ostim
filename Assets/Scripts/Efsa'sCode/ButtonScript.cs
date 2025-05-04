using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{

    public GameObject blackdim;
    public GameObject startButton;
    public GameObject creditButton;
    public GameObject Credits;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = 1f;
        
    }

    public void PlayButtonSound()
    {
        Debug.Log("burda");
        this.GetComponent<AudioSource>().Play();

        StartCoroutine(OpeningScene());

        
    }

    public IEnumerator OpeningScene()
    {
       startButton.SetActive(false);
       creditButton.SetActive(false);

        float timer = 0;
        while (timer < 2f)
        {
            blackdim.GetComponent<UnityEngine.UI.Image>().color = new Color32(0, 0, 0, (byte)(timer * 128f));
            yield return null;
            timer += Time.deltaTime;
            Debug.Log("burda");
        }
        
        SceneManager.LoadScene("GameScene");
    }

    public void GoToCredit()
    {
        this.GetComponent<AudioSource>().Play();
        StartCoroutine(CreditsFunc());
    }

    public IEnumerator CreditsFunc()
    {
        startButton.SetActive(false);
        creditButton.SetActive(false);

        float timer = 0;
        while (timer < 1f)
        {
            blackdim.GetComponent<UnityEngine.UI.Image>().color = new Color32(0, 0, 0, (byte)(timer * 255f));
            yield return null;
            timer += Time.deltaTime;
        }
        SceneManager.LoadScene("CreditsScene");
    }

    public void SkipCredit()
    {
        StartCoroutine(ButtonStart());
        this.GetComponent<AudioSource>().Play();
    }

    public IEnumerator ButtonStart()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("StartMenu");
        
    }
}
