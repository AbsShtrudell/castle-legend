using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] AudioSource m_Source;
    [SerializeField] ParticleSystem m_Particles;

    public void PlayEffect()
    {
        m_Source.Play();
        m_Particles.Play();
    }
}
