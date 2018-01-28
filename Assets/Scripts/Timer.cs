using UnityEngine;

public class Timer : MonoBehaviour {

    public delegate void OnTimeUp();
    public OnTimeUp OnTimeUpAction;

    public void StartTimer(float time) {
        Invoke("CallDeletage", time);
    }

    public void CancelTimer()
    {
        CancelInvoke("CallDeletage");
    }

    void CallDeletage() {
        if (OnTimeUpAction != null) {
            OnTimeUpAction();
            OnTimeUpAction = null;
        }
    }
}
