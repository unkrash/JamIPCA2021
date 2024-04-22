using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    public ParticleSystem fire_ps;
    public ParticleSystem glow_ps;
    public ParticleSystem sparks_ps;
    public Light dirLight;
    public Volume postProcessingVolume;


    private ColorAdjustments _colorAdjustments;
    private ParticleSystem.LightsModule _sparksLightModule;
    private ParticleSystem.EmissionModule _fireEmissonMod;
    private ParticleSystem.EmissionModule _glowEmissonMod;
    private ParticleSystem.EmissionModule _sparksEmissonMod;
    private float defaultRange;

    private void Start()
    {
        _sparksLightModule = sparks_ps.lights;
        _sparksEmissonMod = sparks_ps.emission;
        _glowEmissonMod = glow_ps.emission;
        _fireEmissonMod = fire_ps.emission;
        postProcessingVolume.profile.TryGet<ColorAdjustments>(out _colorAdjustments);
        defaultRange = _sparksLightModule.rangeMultiplier;
    }

    private void Update()
    {
        _sparksLightModule.intensityMultiplier = GameManager.instance.oilAmount;
        _sparksLightModule.rangeMultiplier = defaultRange * GameManager.instance.oilAmount;
        _sparksEmissonMod.rateOverTime = 10 * GameManager.instance.oilAmount;
        _fireEmissonMod.rateOverTime = 10 * GameManager.instance.oilAmount;
        _glowEmissonMod.rateOverTime = 60 * GameManager.instance.oilAmount;
        dirLight.intensity = 1.2f * GameManager.instance.oilAmount;
        _colorAdjustments.saturation.value = - (10 * 1/GameManager.instance.oilAmount);
    }
}
