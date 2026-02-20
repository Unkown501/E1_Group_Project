using UnityEngine;

public class DoorwayScript : MonoBehaviour
{
    [SerializeField] NewSceneLoader sceneLoader;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {}
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
             sceneLoader.LoadScene();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

