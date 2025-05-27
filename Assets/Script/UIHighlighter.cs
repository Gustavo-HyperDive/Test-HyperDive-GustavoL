using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class UIHighlighter : MonoBehaviour
{
    [System.Serializable]
    public class UIHintGroup
    {
        public KeyCode key;
        public GameObject groupRoot;
    }

    public List<UIHintGroup> hintGroups;
    public float highlightDuration = 1.0f;
    public float fadeSpeed = 5f;

    private Dictionary<KeyCode, List<Graphic>> keyToGraphics = new();
    private Dictionary<Graphic, Coroutine> activeCoroutines = new();
    private Dictionary<Graphic, float> originalAlphas = new();

    void Start()
    {
        foreach (var group in hintGroups)
        {
            if (group.groupRoot == null) continue;

            var graphics = new List<Graphic>();
            graphics.AddRange(group.groupRoot.GetComponentsInChildren<Image>(true));
            graphics.AddRange(group.groupRoot.GetComponentsInChildren<TextMeshProUGUI>(true));
            keyToGraphics[group.key] = graphics;

            // Salva alpha original uma vez
            foreach (var graphic in graphics)
            {
                if (!originalAlphas.ContainsKey(graphic))
                    originalAlphas[graphic] = graphic.color.a;
            }
        }
    }

    void Update()
    {
        foreach (var kvp in keyToGraphics)
        {
            if (Input.GetKeyDown(kvp.Key))
            {
                foreach (var graphic in kvp.Value)
                {
                    if (activeCoroutines.TryGetValue(graphic, out Coroutine c))
                        StopCoroutine(c);

                    activeCoroutines[graphic] = StartCoroutine(Highlight(graphic));
                }
            }
        }
    }

    IEnumerator Highlight(Graphic graphic)
    {
        float originalAlpha = originalAlphas[graphic];
        Color color = graphic.color;

        // Aparece imediatamente
        color.a = 1.0f;
        graphic.color = color;

        // Aguarda tempo visível
        yield return new WaitForSeconds(highlightDuration);

        // Fade para o alpha original
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            color.a = Mathf.Lerp(1.0f, originalAlpha, t);
            graphic.color = color;
            yield return null;
        }

        // Garante alpha exato
        color.a = originalAlpha;
        graphic.color = color;

        activeCoroutines.Remove(graphic);
    }
}
