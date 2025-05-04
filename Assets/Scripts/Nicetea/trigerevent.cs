using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class trigerevent : MonoBehaviour
{
    bool done = false;
    public Speak speak;
    public int character;
    public List<string> Dialog = new();
    public List<int> Indexes = new();
    public List<float> endwaittimes;
    public List<GameObject> Buttons = new();

    public int extraevent = 0;
    public List<GameObject> extraitems = new();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Contains("Player") && !done) 
        {
            Debug.Log("collider2d");
            for (int i = 0; i < Dialog.Count; i++)
            {
                if (Dialog[i].Contains('/'))
                    speak.speak(Indexes[i], Dialog[i], endwaittimes[i], Buttons);
                else
                    speak.speak(Indexes[i], Dialog[i], endwaittimes[i], null);
            }
            done = true;
            switch (extraevent)
            {
                case (1):
                    for(int i = 0; i < extraitems.Count; i++)
                    {
                        extraitems[i].SetActive(false);
                    }
                    break;
            }
        }
    }
}
