using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParticlePoolHolder : MonoBehaviour
{
    [SerializeField] private GameObject particlePrefab;
    private List<GameObject> particles;
    private void OnEnable()
    {
        particles = new List<GameObject>();
        for (var i = 0; i < Constants.CandiesInPoolCount; i++)
        {
            particles.Add(Instantiate(particlePrefab,this.transform.position,Quaternion.identity,this.transform));
        }
    }
    public GameObject GetCandy()
    {
        return particles.FirstOrDefault(particle => !particle.activeInHierarchy);
    }
}
