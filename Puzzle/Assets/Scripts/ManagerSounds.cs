using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerSounds : MonoBehaviour
{
    AudioSource AudioSource;
    public static ManagerSounds Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    public void click()
    {
        AudioSource.Play();
    }
}
