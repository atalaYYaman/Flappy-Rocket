using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField]float LevelLoadDelay = 1.0f;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip finishSound;
    
    [SerializeField] ParticleSystem crashParticle;
    [SerializeField] ParticleSystem finishParticle;

    AudioSource audioSource;
    


    bool isTransitioning = false;
    bool collisionDisabled = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
           
    }

    void Update()
    {
        admin();
    }

    void admin()
    {
        if (Input.GetKeyDown(KeyCode.L)) 
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled;
        }
    }

    void OnCollisionEnter(Collision other)
    {


        if (isTransitioning || collisionDisabled)
        {
            return;
        }

        switch (other.gameObject.tag) 
        {
            case "Friendly":
                UnityEngine.Debug.Log("This thing is friendly");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();  
                break;
        }
    }
        
    void StartCrashSequence()
    {
        isTransitioning = true;
        crashParticle.Play();
        audioSource.PlayOneShot(crashSound);
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", LevelLoadDelay);
    }

    void StartSuccessSequence()
    {
        isTransitioning = true;
        finishParticle.Play();
        audioSource.PlayOneShot(finishSound);
        GetComponent <Movement>().enabled = false;
        Invoke("LoadNextLevel", LevelLoadDelay);
    }

    void ReloadLevel() 
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);    
    }
    
    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
