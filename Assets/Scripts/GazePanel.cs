using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GazePanel : MonoBehaviour, IGvrPointerHoverHandler
{
    [Range(0.5f, 10f)]
    public float gazeTime = 2f;

    public UnityEvent onTimeOut;

    private float timer;
    
    public void OnGvrPointerHover(PointerEventData eventData)
    {
        if (timer < gazeTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            onTimeOut.Invoke();
            enabled = false;
        }
    }
}
