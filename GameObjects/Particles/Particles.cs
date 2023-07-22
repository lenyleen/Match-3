using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PooledObject))]
public class Particles : MonoBehaviour
{
    private ParticleSystem particles;
    public Action particlesAction;
    private PooledObject pooledObject;
    private void OnEnable()
    {
        pooledObject = this.GetComponent<PooledObject>();
        particles = this.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = particles.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    public void Play()
    {
        this.gameObject.SetActive(true);
    }

    private void OnParticleSystemStopped()
    {
        particlesAction?.Invoke();
        pooledObject.ReturnToPool();
        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        particlesAction = null;
        this.transform.localPosition = Vector3.zero;
    }
}
