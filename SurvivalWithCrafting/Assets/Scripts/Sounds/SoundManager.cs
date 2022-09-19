using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip razorHitSound, metalHitSound, shootSound, noBulletsSound, metalFenceSound, cremalleraSound, botellaRota,
        herido1Sound, herido2Sound, carfteoSound;
    public static AudioSource audioSrc;
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();

        razorHitSound = Resources.Load<AudioClip>("AcuchillarSound");
        metalHitSound = Resources.Load<AudioClip>("GolpeMetalSound");
        shootSound = Resources.Load<AudioClip>("GunShotSound");
        noBulletsSound = Resources.Load<AudioClip>("SinBalasSound");
        metalFenceSound = Resources.Load<AudioClip>("BerjaMetalicaSound");
        cremalleraSound = Resources.Load<AudioClip>("CremalleraSound");
        botellaRota = Resources.Load<AudioClip>("BotellaRotaSound");
        herido1Sound = Resources.Load<AudioClip>("Herido1Sound");
        herido2Sound = Resources.Load<AudioClip>("Herido2Sound");
        carfteoSound = Resources.Load<AudioClip>("SierraCrafteoSound");
    }

    void Update()
    {
        
    }

    public static void PlaySound(string _clip)
    {
        switch(_clip)
        {
            case "AcuchillarSound":
                audioSrc.PlayOneShot(razorHitSound);
                break;
            case "GolpeMetalSound":
                audioSrc.PlayOneShot(metalHitSound);
                break;
            case "GunShotSound":
                audioSrc.PlayOneShot(shootSound);
                break;
            case "SinBalasSound":
                audioSrc.PlayOneShot(noBulletsSound);
                break;
            case "BerjaMetalicaSound":
                audioSrc.PlayOneShot(metalFenceSound);
                break;
            case "CremalleraSound":
                audioSrc.PlayOneShot(cremalleraSound);
                break;
            case "BotellaRotaSound":
                audioSrc.PlayOneShot(botellaRota);
                break;
            case "Herido1Sound":
                audioSrc.PlayOneShot(herido1Sound);
                break;
            case "Herido2Sound":
                audioSrc.PlayOneShot(herido2Sound);
                break;
            case "SierraCrafteoSound":
                audioSrc.PlayOneShot(carfteoSound);
                break;
        }
    }
}
