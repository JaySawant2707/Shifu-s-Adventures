using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrashDetector : MonoBehaviour
{
    [SerializeField] AudioClip crashSound;
    [SerializeField] ParticleSystem fallParticles;
    [SerializeField] float relodeDelay = 0.5f;

    Animator animator;
    SurfaceEffector2D surfaceEffector2D;
    bool hasCrashed = false;
    Playercontroller playercontroller;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        surfaceEffector2D = FindObjectOfType<SurfaceEffector2D>();
        playercontroller = GetComponent<Playercontroller>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Obstacle") && !hasCrashed){
            hasCrashed = true;
            animator.SetBool("Crashed", true);
            fallParticles.Play();
            GetComponent<AudioSource>().PlayOneShot(crashSound);
            surfaceEffector2D.speed = 0;
            playercontroller.enabled = false;
            Invoke("ReloadLevel", relodeDelay);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ground" && !hasCrashed)
        {
            hasCrashed = true;
            fallParticles.Play();
            GetComponent<AudioSource>().PlayOneShot(crashSound);
            surfaceEffector2D.speed = 0;
            playercontroller.enabled = false;
            Invoke("ReloadLevel", relodeDelay);
        }
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
