using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnTriggerEnter : MonoBehaviour
{

    [SerializeField] string[] soundVariants;
    [SerializeField] string targetTag;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        string soundName = selSound();

        if (other.CompareTag(targetTag)) {
            FindObjectOfType<AudioManager>().Play(soundName);
        }
        
    }

    private string selSound() {
        return soundVariants[UnityEngine.Random.Range(0, soundVariants.Length)];
    }
}
