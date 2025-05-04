using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Platformer.Mechanics; // Make sure to include the namespace
using UnityEngine.SceneManagement;

public class Speak : MonoBehaviour
{
    public List<GameObject> Sounds = new();
    public List<float> Pitches = new();
    public List<GameObject> CharactersUi = new();
    public List<GameObject> Buttons = new();
    public List<GameObject> Buttonhold = new();
    public PlayerController player;
    public List<bool> buttonvalues = new();

    public TextMeshProUGUI speechtext;
    public GameObject canvas;
    public GameObject leveltostart;
    public GameObject leveltoclose;

    public List<string> Dialog = new();
    public List<int> Indexes = new();
    public List<float> endwaittimes = new();
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
        Debug.Log(text);
        //Debug.Log("foroncesi");
        if (text.Contains(";"))
        {
            if (buttonvalues[1]) {
                audioprefab = Sounds[Sounds.Count - 1];
                GameObject audio = Instantiate(audioprefab);
                AudioSource tempaudio = audio.GetComponent<AudioSource>();

                tempaudio.Play(); 
            }
            CharactersUi[index].SetActive(false);
            Dialog.RemoveAt(0);
            Indexes.RemoveAt(0);
            endwaittimes.RemoveAt(0);
            speechtext.text = "";
            isSpeaking = false;
            canvas.SetActive(false);

            yield return new WaitForSeconds(1);
            float dimtimer = 0;
            GameObject dim = GameObject.Find("DimObject");
            canvas.SetActive(false);
            yield return new WaitForSeconds(1);

            while (dimtimer < 1)
            {
                dimtimer += Time.deltaTime;
                yield return null;
                dim.GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, (byte)(dimtimer * 255));
            }
            yield return new WaitForSeconds(0.4f);
            SceneManager.LoadScene("CreditsScene");
        }
        else
        {

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
                if (Buttons != null)
                    Buttonhold = Buttons;
                while (waitforbutton)
                {
                    yield return null;
                }
                if (Buttonhold != null)
                {
                    for (int b = 0; b < Buttonhold.Count; b++)
                    {
                        Buttonhold[b].SetActive(false);
                        yield return null;
                    }
                    Buttonhold = null;
                }

            }
            yield return new WaitForSeconds(waittime);
            CharactersUi[index].SetActive(false);
            Dialog.RemoveAt(0);
            Indexes.RemoveAt(0);
            endwaittimes.RemoveAt(0);
            speechtext.text = "";
            isSpeaking = false;
            canvas.SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }
    }
    public IEnumerator deleteAudioSource(GameObject target)
    {
        yield return new WaitForSecondsRealtime(0.3f);
        Destroy(target);
    }
    public void buttontest(int index)
    {
        buttonvalues[index] = true;
        waitforbutton = false;

        switch (index)
        {
            case (0):
                speak(3, "Teşekkür ederim Muzaffer Bey!", 1);
                speak(3, "İlerideki terkedilmiş kulenin içinde olabilir.", 2);
                

                break;
            case (1):
                speak(3, "Terkedilmiş kulenin içinde var. Ama sonra... komşuyu unutursan, bazı şeyler bozulur. Bu zamanlarda kimseyi kızdırmamak gerek", 1);

                break;
            case (4):
                speak(3, "Güzel...", 1);
                speak(3, "İlacı bana ver Muzaffer!", 2);
                speak(0, "Hayır! Kızımın bu ilaca ihtiyacı var veremem.", 1);
                speak(3, "Kızın...", 1);
                speak(3, "Artık ilaca ihtiyacı yok Muzaffer!", 1);
                speak(0, "Ne.. Na-. Nasıl yani?", 1);
                speak(3, "İçeri girip kendin bak.", 2);
                break;
            case (5):
                speak(3, "İlaç olabileceğini düşünmüştüm.", 1);
                speak(3, "Ama üzülmene gerek yok.", 2);
                speak(3, "Artık ilaca ihiyacın kalmadı", 1);
                speak(0, "Ne.. Na-. Nasıl yani?", 1);
                speak(3, "İçeri girip kendin bak.", 2);
                speak(3, "Bu arada", 2);
                speak(3, "Yalan söylediğini biliyorum!", 1);
                break;
            case (6):
                //iki ilaç buldu ve komşuya bir tanesini paylaşıcak
                speak(3, "İlaç bulabildin mi kulenin içinde?", 1);
                speak(0, "Evet. 2 tane bulabildim Oduncu Efendi", 2);
                speak(3, "O zaman bir tanesini alabilirim.", 2);
                speak(0, "Evet. Geçmiş olsun Oduncu Efendi!", 1);
                break;

            case (7):
                speak(3, "Tüh be! Bulabileceğini düşünmüştüm.", 1);
                speak(3, "Bu salgın herkesi öteki dünyayla tanıştıracak.", 2);
                speak(3, "Baktığın için sağolasın Muzaffer Bey.", 2);
                leveltostart.SetActive(true);
                leveltoclose.SetActive(false);
                break;
            case (8):
                speak(0, "Özür dilerim kızım.", 1);
                speak(0, ";", 1);
                break;
            case (9):
                speak(0, "Özür dilerim Zeliha.", 1);
                speak(0, ";", 1);
                break;

        }
    }

}
