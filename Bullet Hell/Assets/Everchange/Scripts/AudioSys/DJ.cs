using UnityEngine;

namespace AudioSys
{
    /// <summary>
    /// Decides what music to play and when.
    /// </summary>
    public class DJ : MonoBehaviour
    {

        // UNITY EDITOR
        public BoomBox __boomBox;
        [Header("Audio Tracks")]
        public AudioClip __ambientMusic;

        // ACCESSORS
        private BoomBox PlayerBoomBox { get; set; }
        private AudioClip AmbientMusic { get; set; }

        // MONOBEHAVIOUR
        void Awake()
        {
            this.PlayerBoomBox = this.__boomBox;
            this.AmbientMusic = this.__ambientMusic;
        }

        void Update()
        {
            if (!this.PlayerBoomBox.IsPlayingGenre(BoomBox.Genres.Ambient))
                this.PlayerBoomBox.PlaySong(BoomBox.Genres.Ambient, this.AmbientMusic);
        }
    }
}
