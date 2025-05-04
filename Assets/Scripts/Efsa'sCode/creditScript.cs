using UnityEngine;

public class credits : MonoBehaviour
{
    public float speed;
    public float offset = 0;
    public float startx = 0;
    float startz = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startx = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = 1;
        offset += (Time.deltaTime * speed);
        Vector3 newpos = new Vector3(startx, offset, 0);
        Debug.Log(newpos);
        transform.position = newpos;
    }

}