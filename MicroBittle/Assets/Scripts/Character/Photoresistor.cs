using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Photoresistor : MonoBehaviour
{
    public enum LightStatus{ON, OFF}
    public static Photoresistor Instance = null;
    public Light FlashLight;
    public float currentLightVal = 100f;
    [SerializeField] Volume boxVolume;
    [SerializeField] VolumeProfile boxVolumeProfile;
    [SerializeField] float shrinkedVal = 0.78f;
    [SerializeField] float normalIntensityVal = 4.14f;
    [SerializeField] Color normalLightColor;
    [SerializeField] Color shrinkedColor;
    [SerializeField] Color offColor;
    [SerializeField] Color equippedNormalLightColor;
    [SerializeField] Color equippedShrinkedColor;
    [SerializeField] Color equippedOffColor;
    public LightStatus lightStatus = LightStatus.ON;
    // Start is called before the first frame update
    void Start()
    {
        // boxVolumeProfile = boxVolume.profile;
        normalIntensityVal = FlashLight.GetComponent<Light>().intensity;
        lightStatus = LightStatus.OFF;
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
        if(lightStatus == LightStatus.ON)
        {
            FlashLight.GetComponent<Light>().intensity = 0.0f;
            boxVolumeProfile.TryGet<ColorAdjustments>(out var cameraColorOff);
            if (OutfitMgr.Instance.currentObstacleType != ObstacleType.Light)
            {
                cameraColorOff.colorFilter.value = offColor;
            }
            else
            {
                cameraColorOff.colorFilter.value = equippedOffColor;
            }
            boxVolumeProfile.TryGet<Vignette>(out var cameraVignetteOff);
            cameraVignetteOff.intensity.value = 1;
            if(SoundMgr.Instance)
            {
                SoundMgr.Instance.PlayAudio("HEADLAMP_ON_OFF_v1");
            }
            
            lightStatus = LightStatus.OFF;
        }
        
    }

    public void LightOn()
    {
        if(lightStatus == LightStatus.OFF)
        {
            FlashLight.GetComponent<Light>().intensity = normalIntensityVal;
            boxVolumeProfile.TryGet<ColorAdjustments>(out var cameraColorOn);
            if (OutfitMgr.Instance.currentObstacleType != ObstacleType.Light)
            {
                cameraColorOn.colorFilter.value = normalLightColor;
            }
            else
            {
                cameraColorOn.colorFilter.value = equippedNormalLightColor;
            }
            boxVolumeProfile.TryGet<Vignette>(out var cameraVignetteOn);
            cameraVignetteOn.intensity.value = 0.75f;
            if(SoundMgr.Instance)
            {
                SoundMgr.Instance.PlayAudio("HEADLAMP_ON_OFF_v1");
            }
            
            lightStatus = LightStatus.ON;
        }
        
    }

    public void LightShrink()
    {
        FlashLight.GetComponent<Light>().intensity = shrinkedVal;
        boxVolumeProfile.TryGet<ColorAdjustments>(out var cameraColorOff);
        if (OutfitMgr.Instance.currentObstacleType != ObstacleType.Light)
        {
            cameraColorOff.colorFilter.value = shrinkedColor;
        }
        else
        {
            cameraColorOff.colorFilter.value = equippedShrinkedColor;
        }
        boxVolumeProfile.TryGet<Vignette>(out var cameraVignetteOff);
        cameraVignetteOff.intensity.value = 1;
    }
}
