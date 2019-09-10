using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TransportPoint : MonoBehaviour
{
    public Color normalColor = new Color(0.8f, 0.8f, 0.8f);
    public Color highlightColor = new Color(0.9f, 0.5f, 0.5f);
    public Color targetHighlightColor = new Color(0.2f, 0.9f, 0.2f);
    public Vector3 transportOffset = new Vector3(0, 1f, 0);
    public bool useGaze;
    [Range(1f, 5f)] public float gazeTime = 2f;

    private ParticleSystemRenderer particle;
    private Renderer mRenderer;
    private bool isGazedAt;
    private float currentGazeTimer;

    private void Start()
    {
        particle = GetComponent<ParticleSystemRenderer>();
        mRenderer = GetComponent<Renderer>();
        SetGazedAt(false);
        Player.Instance.OnPlayerTeleported += OnPlayerTeleported;

        var trigger = gameObject.AddComponent<EventTrigger>();

        var entry = new EventTrigger.Entry {eventID = EventTriggerType.PointerEnter};
        entry.callback.AddListener((data) => { SetGazedAt(true); });
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry {eventID = EventTriggerType.PointerExit};
        entry.callback.AddListener((data) => { SetGazedAt(false); });
        trigger.triggers.Add(entry);


        entry = new EventTrigger.Entry {eventID = EventTriggerType.PointerClick};
        entry.callback.AddListener((data) => { TransportToPoint(); });
        trigger.triggers.Add(entry);
    }


    private void SetGazedAt(bool gazedAt)
    {
        isGazedAt = gazedAt;
        if (particle != null)
        {
            if (gazedAt)
                particle.material.SetColor("_TintColor", highlightColor);
            else
                particle.material.SetColor("_TintColor", normalColor);
        }
        else
        {
            if (gazedAt)
                mRenderer.material.color = highlightColor;
            else
                mRenderer.material.color = normalColor;
        }
    }

    private void TransportToPoint()
    {
        Player.Instance.Transport(this);
    }

    void OnPlayerTeleported(TransportPoint point)
    {
        SetGazedAt(false);
        gameObject.SetActive(point != this);
    }

    private void Update()
    {
        if (useGaze && isGazedAt)
        {
            currentGazeTimer += Time.deltaTime;
            mRenderer.material.color = Color.Lerp(highlightColor, targetHighlightColor, (currentGazeTimer / gazeTime));

            if (currentGazeTimer >= gazeTime)
            {
                currentGazeTimer = 0f;
                TransportToPoint();
            }
        }
        else
        {
            currentGazeTimer = 0f;
        }
    }
}