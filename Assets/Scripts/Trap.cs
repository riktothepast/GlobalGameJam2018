using UnityEngine;

public class Trap : MonoBehaviour
{

    public void DoDamage(GameObject bot)
    {
        Destroy(bot);
    }
}
