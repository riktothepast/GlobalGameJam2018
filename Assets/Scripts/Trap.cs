using UnityEngine;

public class Trap : MonoBehaviour
{

    public void DoDamage(GameObject bot)
    {
		bot.GetComponent<Bot> ().Disable ();
    }
}
