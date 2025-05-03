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
            for(int i = 0; i < Dialog.Count; i++)
            {
                speak.speak(Indexes[i], Dialog[i]);
            }
        }
    }

}
