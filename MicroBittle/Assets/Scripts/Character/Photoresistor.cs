using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Photoresistor : MonoBehaviour
{
    public static Photoresistor Instance = null;
    public Light FlashLight;
    public float currentLightVal = 100f;
    [SerializeField] Volume boxVolume;
    [SerializeField] VolumeProfile boxVolumeProfile;
    [SerializeField] float shrinkedVal = 0.78f;
    [SerializeField] float normalIntensityVal = 4.14f;
    [SerializeReference] Color normalLightColor;
    [SerializeReference] Color shrinkedColor;
    [SerializeReference] Color offColor;
    // Start is called before the first frame update
    void Start()
    {
        boxVolumeProfile = boxVolume.profile;
        normalIntensityVal = FlashLight.GetComponent<Light>().intensity;
        LightOn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public void InputLightVal(int val)
    {
        FlashLight.GetComponent<Light>().intensity = val;
    }

    public void LightOff()
    {
        FlashLight.GetComponent<Light>().intensity = 0.0f;
        boxVolumeProfile.TryGet<ColorAdjustments>(out var cameraColorOff);
        cameraColorOff.colorFilter.value = offColor;
        boxVolumeProfile.TryGet<Vignette>(out var cameraVignetteOff);
        cameraVignetteOff.intensity.value = 1;
    }

    public void LightOn()
    {
        FlashLight.GetComponent<Light>().intensity = normalIntensityVal;
        boxVolumeProfile.TryGet<ColorAdjustments>(out var cameraColorOn);
        cameraColorOn.colorFilter.value = normalLightColor;
        boxVolumeProfile.TryGet<Vignette>(out var cameraVignetteOn);
        cameraVignetteOn.intensity.value = 0.75f;
    }

    public void LightShrink()
    {
        FlashLight.GetComponent<Light>().intensity = shrinkedVal;
        boxVolumeProfile.TryGet<ColorAdjustments>(out var cameraColorOff);
        cameraColorOff.colorFilter.value = shrinkedColor;
        boxVolumeProfile.TryGet<Vignette>(out var cameraVignetteOff);
        cameraVignetteOff.intensity.value = 1;
    }
}
