using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPoint : MonoBehaviour
{
    [SerializeField] ParticleSystem finishParticles;
    [SerializeField] float relodeDelay = 0.5f;
    CrashDetector cd;

    void Start()
    {
        cd = FindObjectOfType<CrashDetector>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            finishParticles.Play();
            cd.Invoke("ReloadLevel", relodeDelay);
        }
    }
}
