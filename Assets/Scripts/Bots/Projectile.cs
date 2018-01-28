using UnityEngine;

public class Projectile : MonoBehaviour {

    private void Awake()
    {
        Invoke("Kill", 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player")) {
            other.transform.parent.GetComponent<Bot>().Disable();
        }
    }

    void Kill()
    {
        Destroy(gameObject);
    }

}
