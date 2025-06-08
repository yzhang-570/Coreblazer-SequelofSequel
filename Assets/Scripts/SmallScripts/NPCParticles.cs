using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Yarn.Unity;

public class NPCParticles : MonoBehaviour
{
    public ParticleSystem particleEffect;
    float krillTimer = 0.5f;
    public void StartFade(string npc)
    {
        if (gameObject.name == npc ){
        // Instantiate the particle effect at the object's position
        Instantiate(particleEffect, transform.position, Quaternion.identity);
        AudioManager.Instance.PlaySFX("die");
        StartCoroutine(FadeOut());}
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(krillTimer);

        // Disable or destroy the object
        gameObject.SetActive(false); // or Destroy(gameObject);
    }
}
