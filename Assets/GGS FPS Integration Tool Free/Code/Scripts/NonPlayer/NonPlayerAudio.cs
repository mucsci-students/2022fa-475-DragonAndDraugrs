using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GGSFPSIntegrationTool.Scripts.NonPlayer
{
    [RequireComponent(typeof(AudioSource))]
    public class NonPlayerAudio : MonoBehaviour
    {
        [SerializeField] AudioClip _PeriodicAudioClip;
        [SerializeField] AudioClip _DamageAudioClip;
        [SerializeField] AudioClip _DeathAudioClip;

        [Space]

        [SerializeField] public float MaximumPitch = 1.2f;
        [SerializeField] public float MinimumPitch = 0.9f;

        public Utilities.General.Range PeriodicAudioIntervalRange { get; set; } = 
            new Utilities.General.Range(3f, 6f);
        public Utilities.General.Range DamageAudioIntervalRange { get; set; } = 
            new Utilities.General.Range(0.4f, 0.6f);
        public Utilities.General.Range PitchRange { get; set; }

        Utilities.General.Time.DownCounter PeriodicAudioCountDown { get; set; } = 
            new Utilities.General.Time.DownCounter();
        Utilities.General.Time.DownCounter DamageAudioCountDown { get; set; } = 
            new Utilities.General.Time.DownCounter();

        bool HasDeathAudioPlayed { get; set; } = false;

        AudioSource AudioSourceProperty { get; set; }

        void Awake()
        {
            AudioSourceProperty = GetComponent<AudioSource>();

            PitchRange = new Utilities.General.Range(MaximumPitch, MinimumPitch);
        }

        void OnEnable()
        {
            HasDeathAudioPlayed = false;

            PeriodicAudioCountDown.Start(PeriodicAudioIntervalRange.RandomValue);
            DamageAudioCountDown.Start(DamageAudioIntervalRange.RandomValue);
        }

        void Update()
        {
            if (!HasDeathAudioPlayed)
            {
                PlayPeriodicAudio();

                PeriodicAudioCountDown.Update();
                DamageAudioCountDown.Update();
            }
        }

        
        public void PlayDamageAudio()
        {
            if (DamageAudioCountDown.HasCountDownEnded)
            {
                PlayAudio(_DamageAudioClip);

                PeriodicAudioCountDown.Start(PeriodicAudioIntervalRange.RandomValue);
                DamageAudioCountDown.Start(DamageAudioIntervalRange.RandomValue);
            }
        }

        public void PlayDeathAudio()
        {
            if (!HasDeathAudioPlayed)
            {
                PlayAudio(_DeathAudioClip);
                HasDeathAudioPlayed = true;
            }
        }

        void PlayPeriodicAudio()
        {
            if (PeriodicAudioCountDown.HasEnded)
            {
                PlayAudio(_PeriodicAudioClip);
                PeriodicAudioCountDown.Start(PeriodicAudioIntervalRange.RandomValue);
            }
        }

        void PlayAudio(AudioClip audioClip)
        {
            AudioSourceProperty.pitch = PitchRange.RandomValue;
            AudioSourceProperty.clip = audioClip;
            AudioSourceProperty.Play();
        }
    }
}