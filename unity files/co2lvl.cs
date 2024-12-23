using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class co2lvl : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private ParticleSystem.EmissionModule _emissionModule;

    // CO2 level thresholds
    public float lowThreshold = 400;   // Example: Low CO2 level threshold
    public float highThreshold = 1000; // Example: High CO2 level threshold

    // Particle emission rates corresponding to CO2 levels
    public float lowEmissionRate = 0.01f;   // Example: Low emission rate
    public float highEmissionRate = .5f;  // Example: High emission rate

    void Start()
    {
        // Get the ParticleSystem component and its emission module
        _particleSystem = GetComponent<ParticleSystem>();
        _emissionModule = _particleSystem.emission;
    }

    // Method to adjust particle emission based on CO2 level
    public void Update_Component(float co2Level)
    {
        // Map CO2 level to emission rate
        float emissionRate = Mathf.Lerp(lowEmissionRate, highEmissionRate, Mathf.InverseLerp(lowThreshold, highThreshold, co2Level));

        // Apply the emission rate to the particle system
        _emissionModule.rateOverTime = emissionRate;
    }
}
