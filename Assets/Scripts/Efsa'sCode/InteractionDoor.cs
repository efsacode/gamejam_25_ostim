using UnityEngine;
using System.Collections;

public class InteractionDoor : MonoBehaviour
{
    public GameObject dimObject; // Etkileşimdeki objenin kendisi
    public SpriteRenderer interactSprite; // Sprite objesini temsil eden SpriteRenderer

    private bool isInTrigger = false;
    private Coroutine fadeCoroutine;

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
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("e ye btim");
            if (isInTrigger)
            {
                if (dimObject != null)
                {
                    if (!dimObject.activeSelf)
                    {
                        dimObject.SetActive(true); // Objeyi etkinleştir
                        StartFadeOut();
                    } // E'ye basılınca sprite'ı gizle

                    else if (dimObject.activeSelf)
                    {
                        dimObject.SetActive(false);
                        StartFadeOut();
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
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
