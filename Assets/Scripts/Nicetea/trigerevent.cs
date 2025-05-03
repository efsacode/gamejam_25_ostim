using UnityEngine;

public class trigerevent : MonoBehaviour
{
    bool done = false;
    public Speak speak;
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
            Debug.Log("Entered trigger zone!");
            speak.speak(0, "buraya girdim");
            //done = true;

        }
    }

}
