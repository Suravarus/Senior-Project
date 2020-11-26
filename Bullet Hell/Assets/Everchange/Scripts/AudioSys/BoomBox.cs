using System;
using UnityEngine;

namespace AudioSys
{
    [RequireComponent(typeof(AudioSource))]
    public class BoomBox : MonoBehaviour
    {
        /// <summary>
        /// Songs played by this boom-box will fall 
        /// under one of these categories.
        /// </summary>
        public enum Genres
        {
            /// <summary>
            /// Player is not in combat.
            /// </summary>
            Ambient,
            /// <summary>
            /// Player is engaged in combat with enemies
            /// that are not Bosses.
            /// </summary>
            Combat,
            /// <summary>
            /// Player is engaged in combat with boss-type 
            /// enemies.
            /// </summary>
            Boss
        }



        // ACCESSORS
        /// <summary>
        /// Is the song playing right now? (read-only)
        /// </summary>
        public bool IsPlaying => this.Speaker.isPlaying;
        /// <summary>
        /// The genre of the song that is currently playing.
        /// </summary>
        private Genres CurrentGenre { get; set; }
        private AudioSource Speaker { get; set; }

        // METHODS
        /// <summary>
        /// Plays the requested song.
        /// </summary>
        /// <param name="songGenre"></param>
        /// <param name="song"></param>
        public void PlaySong(Genres songGenre, AudioClip song)
        {

            if(song != null)
            {
                this.Speaker.Stop();
                this.Speaker.clip = song;
                this.Speaker.Play();
                this.CurrentGenre = songGenre;
            } else
            {
                throw new ArgumentNullException(nameof(song));
            }
        }
        /// <summary>
        /// Pauses playing the song.
        /// </summary>
        public void Pause()
        {
            this.Speaker.Pause();
        }
        /// <summary>
        /// Returns TRUE if currently playing the specified genre.
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public bool IsPlayingGenre(Genres g)
        {
            return this.IsPlaying && this.CurrentGenre == g;
        }

        // MONOBEHAVIOUR
        void Awake()
        {
            DontDestroyOnLoad(this);
            this.Speaker = this.GetComponent<AudioSource>();
            if (this.Speaker == null)
                throw new MissingComponentException(typeof(AudioSource).ToString());
        }
    }
}
