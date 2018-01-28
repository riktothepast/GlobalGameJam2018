using UnityEngine;

public class Projectile : MonoBehaviour {

    private void Awake()
    {
        //Invoke("Kill", 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player")) {
            Bot otherBot = other.transform.parent.GetComponent<Bot>();
            otherBot.effectService.PlaySmokeExplosion(otherBot.transform.position);
            other.transform.parent.GetComponent<Bot>().Disable();
            Kill();
        }
    }

    void Kill()
    {
        Destroy(gameObject);
    }

}
