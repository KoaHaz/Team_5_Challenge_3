using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnTriggerEnter : MonoBehaviour
{

    //[SerializeField] string[] soundVariants;
    //public string[] soundVariants; // back when I tried to use multiple sounds
    public string soundName;
    //[SerializeField] string targetTag;
    public string targetTag;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        //string soundName = selSound();

        if (other.CompareTag(targetTag)) {
            FindObjectOfType<AudioManager>().Play(soundName);
        }
        
    }

    // Don't worry about it. It may work someday 
    //private string selSound() {
    //    return soundVariants[UnityEngine.Random.Range(0, soundVariants.Length)];
    //}
}
