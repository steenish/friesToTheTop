﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System;

// Inspired by Brackey's audio manager.
public class AudioManager : MonoBehaviour {

#pragma warning disable
    [SerializeField]
    private Sound[] sounds;
#pragma warning restore

    public static AudioManager instance;

    private List<Sound> currentlyLooping;

    void Awake() {
        // Handle audio manager instancing between scene loads.
        // If there is no instance, let this be the new instance, otherwise, destroy this object.
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        // If this object was set as the instance, make sure it is not destroyed on scene loads.
        DontDestroyOnLoad(gameObject);

        currentlyLooping = new List<Sound>();

        foreach (Sound sound in sounds) {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.loop = sound.loop;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
        }
    }

    public void Play(string name) {
        Sound sound = Array.Find(sounds, e => e.name == name);

        if (sound != null) {
            // If sound is not a looping sound, or is a looping sound and currently not playing, play the sound.
            if (!sound.loop || (sound.loop && !currentlyLooping.Contains(sound))) {
                sound.source.Play();

                // Add the looping sound to currently playing looping sounds.
                if (sound.loop) {
                    currentlyLooping.Add(sound);
                }
            }
        } else {
            Debug.LogWarning("Audio clip " + name + " not found. No audio played.");
        }
    }

    public void Stop(string name) {
        Sound sound = Array.Find(sounds, e => e.name == name);

        if (sound != null) {
            sound.source.Stop();

            // If the sound was a loop, remove the sound from the currently playing looping sounds.
            if (sound.loop) {
                currentlyLooping.Remove(sound);
            }
        } else {
            Debug.LogWarning("Audio clip " + name + " not found. No audio stopped.");
        }
    }

    public void ChangeVolume(string name, float volume) {
        volume = Mathf.Clamp(volume, 0.0f, 1.0f);
        Sound sound = Array.Find(sounds, e => e.name == name);
        sound.source.volume = volume;
    }
}
