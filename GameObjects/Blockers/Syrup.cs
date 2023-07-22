using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class Syrup : Blocker
{
    public override Task DoCellGridMemberAction()
    {
        SoundManager.instance.PlaySound("Syrup");
        ParticlesPlay();
        return Task.CompletedTask;
    }
    protected override void Disable()
    {
        Particles.particlesAction -= Disable;
        Particles.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}
