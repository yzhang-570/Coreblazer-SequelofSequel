using UnityEngine;

public class FadeOut : MonoBehaviour
{
    [SerializeField] float fadeDuration = 2f; // Time to fully fade
    private Material material;
    private Color originalColor;
    private float fadeTimer = 0f;
    [SerializeField] bool isFading = false;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        Material mat = new Material(renderer.material); // Clone the material
        renderer.material = mat; // Assign new instance

        // Force transparency settings
        mat.SetOverrideTag("RenderType", "Transparent");
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;

        // Then set color alpha
        Color c = mat.GetColor("_BaseColor");
        c.a = 0.5f;
        mat.SetColor("_BaseColor", c);
    }

    //void Update()
    //{
    //    if (isFading)
    //    {
    //        fadeTimer += Time.deltaTime;
    //        float alpha = Mathf.Lerp(originalColor.a, 0f, fadeTimer / fadeDuration);
    //        material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

    //        if (fadeTimer >= fadeDuration)
    //        {
    //            isFading = false;
    //            gameObject.SetActive(false); // Optional: disable after fade
    //        }
    //    }
    //}

    //public void StartFadeOut()
    //{
    //    isFading = true;
    //    fadeTimer = 0f;
    //}
}
