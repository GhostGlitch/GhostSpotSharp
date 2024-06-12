using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media;
using TCSProperties = Windows.Media.Control.GlobalSystemMediaTransportControlsSessionMediaProperties;

namespace GhostSpotSharp {
    internal class GhostProps {

        private string _title = "Unknown Title";
        public string? Title {
            get => _title;
            set => _title = !string.IsNullOrEmpty(value) ? value : "Unknown Title";
        }

        private string _artist = "Unknown Artist";
        public string? Artist {
            get => _artist;
            set => _artist = !string.IsNullOrEmpty(value) ? value : "Unknown Artist";
        }

        private string _album = "Unknown Album";
        public string? Album {
            get => _album;
            set => _album = !string.IsNullOrEmpty(value) ? value : "Unknown Album";
        }

        public string? AlbumArtist { get; set; } = null;
        public List<string> Genres { get; set; } = [];
        private Bitmap _thumbnail = Resources.ErrorThumb;
        public Bitmap? Thumbnail {
            get => _thumbnail;
            set => _thumbnail = value ?? Resources.ErrorThumb;
        }
        public int? TrackNumber { get; set; } = null;
        public int? TrackCount { get; set; } = null;
        private MediaPlaybackType _ptype = MediaPlaybackType.Unknown;
        public MediaPlaybackType? PlaybackType {
            get => _ptype;
            set => _ptype = value ?? MediaPlaybackType.Unknown;
        }
        public string? Subtitle { get; set; } = null;
        public GhostProps() {
        }
        public GhostProps(TCSProperties sesh) {
            Console.WriteLine("GhostProp.InitAsync should be used instead of directly constructing from a TCS object.");
            InitAsync(sesh).GetAwaiter().GetResult();
        }
        public async Task InitAsync(TCSProperties sesh) {
            Title = sesh.Title;
            Artist = sesh.Artist;
            Album = sesh.AlbumTitle;
            AlbumArtist = sesh.AlbumArtist;
            #pragma warning disable IDE0305 // Simplify collection initialization
            Genres = sesh.Genres.ToList();
            #pragma warning restore IDE0305 // Simplify collection initialization
            Thumbnail = await GhostImg.RefToImage(sesh.Thumbnail);
            TrackNumber = sesh.TrackNumber;
            TrackCount = sesh.AlbumTrackCount;
            PlaybackType = sesh.PlaybackType;
            Subtitle = sesh.Subtitle;
        }
    }
}
