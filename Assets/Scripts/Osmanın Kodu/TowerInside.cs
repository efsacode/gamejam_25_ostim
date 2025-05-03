using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerInside : MonoBehaviour
{
    public GameObject dimObject; // Etkile�imdeki objenin kendisi
    public SpriteRenderer interactSprite; // Sprite objesini temsil eden SpriteRenderer
    public GameObject insideTower;
    public GameObject massblock1;
    public GameObject massblock2;
    public GameObject massblock3;

    public List<GameObject> aktif = new();
    public List<GameObject> deaktif = new();
    public bool altkapi = false;

    public bool isInTrigger = false;
    private Coroutine fadeCoroutine;

    void Start()
    {
        if (interactSprite != null)
        {
            // Ba�ta sprite tamamen g�r�nmez olacak �ekilde ayarla
            Color color = interactSprite.color;
            color.a = 0f;
            interactSprite.color = color;
            interactSprite.gameObject.SetActive(false); // Sprite ba�ta g�r�nmesin
        }
    }

    void Update()
    {
        if (isInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            if (dimObject != null)
            {
                //if (!dimObject.activeSelf)
                /*{
                    dimObject.SetActive(true); // Objeyi etkinle�tir
                    insideTower.SetActive(false);
                    massblock1.SetActive(true);
                    massblock2.SetActive(true);
                    massblock3.SetActive(true);
                    StartFadeOut();
                } // E'ye bas�l�nca sprite'� gizle
                */
                if (dimObject.activeSelf && !altkapi)
                {
                    dimObject.SetActive(false);
                    insideTower.SetActive(true);
                    massblock1.SetActive(false);
                    massblock2.SetActive(false);
                    massblock3.SetActive(false);

                    StartFadeOut();
                }
                else if (altkapi)
                {
                    for (int i = 0; i < aktif.Count; i++)
                    {
                        aktif[i].SetActive(true);
                    }
                    for (int i = 0; i < deaktif.Count; i++)
                    {
                        deaktif[i].SetActive(false);
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
            StartFadeIn(); // Oyuncu alan�n i�ine girdi�inde sprite'� g�ster
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = false;
            StartFadeOut(); // Oyuncu alandan ��kt���nda sprite'� gizle
        }
    }

    private void StartFadeIn()
    {
        if (interactSprite == null) return;

        interactSprite.gameObject.SetActive(true); // Sprite'� g�r�n�r yap
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeSprite(interactSprite, 0f, 1f, 0.5f));

    }

    private void StartFadeOut()
    {
        if (interactSprite == null) return;

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeSprite(interactSprite, interactSprite.color.a, 0f, 0.5f)); // 0.5 saniye i�inde gizle

    }

    private IEnumerator FadeSprite(SpriteRenderer sprite, float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        Color startColor = sprite.color;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            sprite.color = new Color(startColor.r, startColor.g, startColor.b, alpha); // Renkle birlikte alpha'y� de�i�tir
            yield return null;
        }
        sprite.color = new Color(startColor.r, startColor.g, startColor.b, endAlpha); // Son renk de�eri
        if (endAlpha == 0f)
            sprite.gameObject.SetActive(false); // Gizlemek i�in Sprite'� deaktive et
    }
}
