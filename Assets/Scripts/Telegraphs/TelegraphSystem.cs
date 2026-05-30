using UnityEngine;

public class TelegraphList
{
    [SerializeField] private Telegraph telegraph;
    [SerializeField] private float delayBeforeNextTelegraph;

}

public class TelegraphSystem : MonoBehaviour
{
    [SerializeField] private TelegraphList[] telegraphs;
    //[SerializeField] private TelegraphSpawnRequest telegraphSpawnRequest;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
