using UnityEngine;

public class CollectPoint : MonoBehaviour
{
    private GameManager gm;

    void Start()
    {
        gm = Object.FindFirstObjectByType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gm.CollectPoint();
            Destroy(gameObject);
        }
    }
}