using System.Linq;
using UnityEngine;

public class AnimFade : MonoBehaviour
{
    [Range(1, 10)] public float duration;

    [Tooltip("Fade the GameObject itself")]
    public bool fadeSelf;

    [Tooltip("Fade the GameObject's children")]
    public bool fadeChildren;

    public enum Mode
    {
        FadeIn,
        FadeOut
    };

    public Mode mode = Mode.FadeOut;

    private Renderer[] renderers;
    private Color[] colorsStart;
    private Color[] colorsEnd;

    private float timer;

    private void Start()
    {
        colorsStart = new Color[] { };
        colorsEnd = new Color[] { };
        renderers = new Renderer[] { };

        if (fadeSelf)
        {
            var ownRenderer = GetComponent<Renderer>();
            if (ownRenderer != null)
                renderers.Append(ownRenderer);
        }

        if (fadeChildren)
        {
            var subRenderers = GetComponentsInChildren<Renderer>();
            renderers = renderers.Concat(subRenderers).ToArray();
        }

        if (renderers.Length > 0)
        {
            // Grab initial colors of each game object and store them
            colorsStart = renderers.Select(rendererItem => rendererItem.material.color).ToArray();
            // create final colors of each game object
            colorsEnd = colorsStart.Select(colorItem =>
                    new Color(colorItem.r, colorItem.g, colorItem.b, mode == Mode.FadeIn ? 1.0f : 0.0f))
                .ToArray();
        }
        else
            Disable();
    }

    private void Update()
    {
        if (renderers.Length == 0 || timer > duration)
        {
            Disable();
            return;
        }

        timer += Time.deltaTime;
        for (var i = 0; i < colorsStart.Length; i++)
            renderers[i].material.color = Color.Lerp(colorsStart[i], colorsEnd[i], timer / duration);
    }

    private void Disable()
    {
        enabled = false;
        timer = 0f;
    }
}