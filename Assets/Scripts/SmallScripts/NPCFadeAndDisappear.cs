using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFadeAndDisappear : MonoBehaviour
{
    public float fadeDuration = 10f;
    private List<Material> material = new List<Material>();
    private List<Color> originalColor = new List<Color>();

    void Start()
    {
        // Use a unique material instance to avoid affecting others
        foreach (Transform _child in transform)
        {
            foreach (Transform childObj in _child)
            {
                Renderer renderer = childObj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Material mat = renderer.material;
                    material.Add(mat);
                    originalColor.Add(mat.color);
                }
            }
        }
    }

    public void StartFade()
    {
        if (material != null)
        {
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        for (int i = 0; i < material.Count; ++i)
        {
            material[i].shader = Shader.Find("Standard");
        }

        float timer = 0f;
        while (timer < fadeDuration)
        {
            for (int i = 0; i < material.Count; ++i)
            {
                float alpha = Mathf.Lerp(originalColor[i].a, 0f, timer / fadeDuration);
                material[i].color = new Color(originalColor[i].r, originalColor[i].g, originalColor[i].b, alpha);
                timer += Time.deltaTime;
            }
            yield return null;
        }

        // Ensure it's fully transparent
        for (int i = 0; i < material.Count; ++i)
        {
            material[i].color = new Color(originalColor[i].r, originalColor[i].g, originalColor[i].b, 0f);
        }

        // Disable or destroy the object
        gameObject.SetActive(false); // or Destroy(gameObject);
    }
}
