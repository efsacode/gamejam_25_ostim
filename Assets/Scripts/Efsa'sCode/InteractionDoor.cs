using UnityEngine;
using System.Collections;


public class InteractionDoor : MonoBehaviour
{
    public GameObject dimObject; // Etkileşimdeki objenin kendisi
    public SpriteRenderer interactSprite; // Sprite objesini temsil eden SpriteRenderer

    private bool isInTrigger = false;
    public int partindex = 0;
    private Coroutine fadeCoroutine;
    private GameObject player;

    void Start()
    {
        if (interactSprite != null)
        {
            // Başta sprite tamamen görünmez olacak şekilde ayarla
            Color color = interactSprite.color;
            color.a = 0f;
            interactSprite.color = color;
            //interactSprite.gameObject.SetActive(false); // Sprite başta görünmesin
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isInTrigger)
            {
                if(partindex == 0) { 
                    if (dimObject != null)
                    {
                        if (!dimObject.activeSelf)
                        {
                            dimObject.SetActive(true); // Objeyi etkinleştir
                            //StartFadeOut();
                        } // E'ye basılınca sprite'ı gizle

                        else if (dimObject.activeSelf)
                        {
                            dimObject.SetActive(false);
                            //StartFadeOut();
                        }
                    }
                }
                else if(partindex == 1)
                {
                    player.SetActive(false);
                    StartCoroutine(pt2_1Woodman());
                }
                this.GetComponent<AudioSource>().Play();
            }
            
        }
    }

    IEnumerator pt2_1Woodman()
    {
        yield return null;
        GameObject woodman = GameObject.Find("Baltacipt2_1");
        woodman.GetComponent<SpriteRenderer>().flipX = false;
        float walktimer = 0;
        Vector2 startpos = woodman.transform.position;
        //Vector2 target = new Vector2(-84.02f, 1.5f);
        while(walktimer < 0.5f)
        {
            woodman.transform.position = Vector2.Lerp(startpos, this.transform.position, (walktimer * 2f));
            yield return null;
            walktimer += Time.deltaTime;
        }
        yield return new WaitForSeconds(0.5f);
        woodman.SetActive(false);
        Speak speak = GameObject.Find("TalkingObject").GetComponent<Speak>();
        speak.speak(0, "Zeliha... Munise...", 1);
        speak.speak(0, "Salgın nasıl bu kadar çabuk", 1);
        speak.speak(0, "YERDEKİ KAN MI?", 1);
        speak.speak(0, "SEN NE YAPTIN!", 1);
        speak.speak(3, "İlaçları ver bana", 1);
        speak.speak(3, ";", 1);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            isInTrigger = true;
            StartFadeIn(); // Oyuncu alanın içine girdiğinde sprite'ı göster
            Debug.Log("player in triggerenter.");
        }
        else
            Debug.Log("not player in triggerenter.");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = false;
            StartFadeOut(); // Oyuncu alandan çıktığında sprite'ı gizle
            Debug.Log("player in triggerenter.");
        }
        else
            Debug.Log("not player in triggerenter.");
    }

    private void StartFadeIn()
    {
        if (interactSprite == null) return;

        interactSprite.gameObject.SetActive(true); // Sprite'ı görünür yap
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeSprite(interactSprite, 0f, 1f, 0.5f));

    }

    private void StartFadeOut()
    {
        if (interactSprite == null) return;

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeSprite(interactSprite, interactSprite.color.a, 0f, 0.5f)); // 0.5 saniye içinde gizle

    }

    private IEnumerator FadeSprite(SpriteRenderer sprite, float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        Color startColor = sprite.color;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            sprite.color = new Color(startColor.r, startColor.g, startColor.b, alpha); // Renkle birlikte alpha'yı değiştir
            yield return null;
        }
        sprite.color = new Color(startColor.r, startColor.g, startColor.b, endAlpha); // Son renk değeri
        if (endAlpha == 0f)
            sprite.gameObject.SetActive(false); // Gizlemek için Sprite'ı deaktive et
    }
}
