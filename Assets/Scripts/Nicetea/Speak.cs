using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Platformer.Mechanics; // Make sure to include the namespace


public class Speak : MonoBehaviour
{
    public List<GameObject> Sounds = new();
    public List<float> Pitches = new();
    public List<GameObject> CharactersUi = new();
    public List<GameObject> Buttons = new();
    public PlayerController player;

    public TextMeshProUGUI speechtext;
    public GameObject canvas;

    private List<string> Dialog = new();
    private List<int> Indexes = new();
    private List<float> endwaittimes = new();
    private bool isSpeaking = false;
    private bool waitforbutton = false;
    char[] seslistesi = { 'o', 'O', 'u', 'U', 'a', 'A', 'ı', 'I', 'ö', 'Ö', 'i', 'İ', 'e', 'E', 'ü', 'Ü' };
    char[] waits = { ' ', ',', '.' };

    void Start()
    {
        //speak(0, "merhabalar....");
        //speak(1, "Merhaba dünyaaaa. konuşuyorum bendee.");
    }

    // Update is called once per frame
    void Update()
    {
        if (Dialog.Count > 0 && Indexes.Count > 0)
        {
            player.controlEnabled = false;
            if (!isSpeaking)
            {
                //Debug.Log("startcoroutine");
                StartCoroutine(speaktask());
                isSpeaking = true;
            }
        }
        else
            player.controlEnabled = true;

    }

    public void speak(int index, string text, float endwaittime, List<GameObject> buttons = null)
    {
        Dialog.Add(text);
        Indexes.Add(index);
        endwaittimes.Add(endwaittime);
        Buttons = buttons;
    }

    IEnumerator speaktask()
    {
        speechtext.text = "";
        string text = Dialog[0];
        int index = Indexes[0];
        float waittime = endwaittimes[0];
        //Debug.Log("speaktask");
        CharactersUi[index].SetActive(true);
        canvas.SetActive(true);
        float pluspitch = Pitches[index];
        GameObject audioprefab = Sounds[index];
        //Debug.Log("foroncesi");
        for (int i = 0; i < text.Length; i++)
        {
            yield return null;
            //Debug.Log("yenikod");
            if (!(text[i] == '/'))
                speechtext.text += text[i];
            else
            {
                //Debug.Log("yenikod else");
                waitforbutton = true;
                for (int b = 0; b < Buttons.Count; b++)
                {
                    Buttons[b].SetActive(true);
                    
                }
            }
            //Debug.Log("ifustu");
            //Time.timeScale = 0;
            if (seslistesi.Contains(text[i])) // Check if the character is a vowel
            {
                GameObject audio = Instantiate(audioprefab);
                AudioSource tempaudio = audio.GetComponent<AudioSource>();
                tempaudio.pitch = pluspitch + Array.IndexOf(seslistesi, text[i]) / 150f;

                tempaudio.Play();
                yield return new WaitForSecondsRealtime(0.08f);
                StartCoroutine(deleteAudioSource(audio));
            }

            if (text[i] == ' ')
            {
                yield return new WaitForSecondsRealtime(0.16f);
            }
            else if (text[i] == ',')
            {
                yield return new WaitForSecondsRealtime(0.3f);
            }
            else if (text[i] == '.' || text[i] == '?' || text[i] == '!')
            {
                yield return new WaitForSecondsRealtime(0.4f);
            }
            //Debug.Log("whileoncesi");
            while (waitforbutton) { 
                yield return null;
                //Debug.Log("here");
            }
            //Debug.Log("lastif");
            if (Buttons != null)
            {
                for (int b = 0; b < Buttons.Count; b++)
                {
                    Buttons[b].SetActive(false);
                    yield return null;
                    //Debug.Log("yenikodforalt");
                }
            }

        }
        //Time.timeScale = 1;
        yield return new WaitForSeconds(waittime);
        //Debug.Log("biris");
        CharactersUi[index].SetActive(false);
        Dialog.RemoveAt(0);
        Indexes.RemoveAt(0);
        endwaittimes.RemoveAt(0);
        speechtext.text = "";
        isSpeaking = false;
        canvas.SetActive(false);
        yield return new WaitForSeconds(0.2f);
    }
    public IEnumerator deleteAudioSource(GameObject target)
    {
        yield return new WaitForSecondsRealtime(0.3f);
        Destroy(target);
    }
    public void buttontest()
    {
        waitforbutton = false;
    }

}
