using System;
using UnityEngine;

namespace Combat.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class WeaponSounds : MonoBehaviour
    {
        public enum Sounds
        {
            Shot, Pickup, Drop, NoAmmo
        }

        // Unity Editor
        public AudioClip __shotSound;
        public AudioClip __pickupSound;
        public AudioClip __dropSound;
        public AudioClip __noAmmo;

        // ACCESSORS
        private AudioClip ShotSound { get; set; }
        private AudioClip PickupSound { get; set; }
        private AudioClip DropSound { get; set; }
        private AudioClip NoAmmoSound { get; set; }
        private AudioSource Speaker { get; set; }

        // METHODS
        public void PlaySound(Sounds s)
        {
            switch (s)
            {
                case Sounds.Shot:
                    this.Speaker.Stop();
                    this.Speaker.clip = this.ShotSound;
                    this.Speaker.Play();
                    break;
                case Sounds.Pickup:
                    this.Speaker.Stop();
                    this.Speaker.clip = this.PickupSound;
                    this.Speaker.Play();
                    break;
                case Sounds.Drop:
                    this.Speaker.Stop();
                    this.Speaker.clip = this.DropSound;
                    this.Speaker.Play();
                    break;
                case Sounds.NoAmmo:
                    this.Speaker.Stop();
                    this.Speaker.clip = this.NoAmmoSound;
                    this.Speaker.Play();
                    break;
            }
        }


        // MONOBEHAVIOUR
        void Awake()
        {
            this.ShotSound = this.__shotSound;
            if (this.ShotSound == null)
                throw new MissingFieldException(nameof(this.__shotSound));
            this.PickupSound = this.__pickupSound;
            if (this.PickupSound == null)
                throw new MissingFieldException(nameof(this.__pickupSound));
            this.DropSound = this.__dropSound;
            if (this.DropSound == null)
                throw new MissingFieldException(nameof(this.__dropSound));
            this.NoAmmoSound = this.__noAmmo;
            if (this.NoAmmoSound == null)
                throw new MissingFieldException(nameof(this.__noAmmo));
            this.Speaker = this.GetComponent<AudioSource>();
            if (this.Speaker == null)
                throw new MissingComponentException(typeof(AudioSource).ToString());
        }

        
    }
}
