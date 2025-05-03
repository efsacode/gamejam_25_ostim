using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Speak : MonoBehaviour
{
    public List<GameObject> Sounds = new();
    public List<float> Pitches = new();
    public List<GameObject> CharactersUi = new();

    public TextMeshProUGUI speechtext;
    public GameObject canvas;

    private List<string> Dialog = new();
    private List<int> Indexes = new();
    private bool isSpeaking = false;

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

            if (!isSpeaking)
            {
                StartCoroutine(speaktask());
                isSpeaking = true;
            }
            else
            {

            }
        }

    }

    public void speak(int index, string text)
    {
        Dialog.Add(text);
        Indexes.Add(index);
    }

    IEnumerator speaktask()
    {
        speechtext.text = "";
        string text = Dialog[0];
        int index = Indexes[0];

        CharactersUi[index].SetActive(true);
        canvas.SetActive(true);
        float pluspitch = Pitches[index];
        GameObject audioprefab = Sounds[index];

        for (int i = 0; i < text.Length; i++)
        {
            yield return null;
            speechtext.text += text[i];
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

        }
        //Time.timeScale = 1;
        yield return new WaitForSeconds(2);

        CharactersUi[index].SetActive(false);
        Dialog.RemoveAt(0);
        Indexes.RemoveAt(0);
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

}
