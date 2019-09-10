using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class GazeButton : MonoBehaviour, IGvrPointerHoverHandler, IPointerExitHandler
{
    [Range(0.5f, 5f)]
    public float gazeTime = 2f;

    [SerializeField] private Image fillLayer;

    private Button button;
    private float timer;
    private bool pressed;

    private void Start()
    {
        button = GetComponentInChildren<Button>();
    }

    public void OnGvrPointerHover(PointerEventData eventData)
    {
        if (timer < gazeTime)
        {
            timer += Time.deltaTime;
            if (fillLayer != null)
                fillLayer.fillAmount = Mathf.Clamp01(timer / gazeTime);
        }
        else if (!pressed)
        {
            pressed = true;
            button.onClick.Invoke();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        timer = 0f;
        pressed = false;
        if (fillLayer != null)
            fillLayer.fillAmount = 0f;
    }
}
