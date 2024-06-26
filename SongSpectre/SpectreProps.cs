﻿using System.Collections;
using System.Reflection;

namespace SongSpectre {
    internal class SpectreProps : IEnumerable<KeyValuePair<string, object?>> {

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
        private MPT _ptype = MPT.Unknown;
        public MPT? PlaybackType {
            get => _ptype;
            set => _ptype = value ?? MPT.Unknown;
        }
        public string? Subtitle { get; set; } = null;
        public SpectreProps() { }
        public SpectreProps(TCSProperties sesh) {
            WriteLine("SpectreProps.InitAsync should be used instead of directly constructing from a TCS object.");
            Sync(InitAsync(sesh));
        }

        public IEnumerator<KeyValuePair<string, object?>> GetEnumerator() {
            IEnumerable<PropertyInfo> properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                       .Where(p => p.CanRead && p.GetIndexParameters().Length == 0);

            foreach (PropertyInfo property in properties) {
                yield return new KeyValuePair<string, object?>(property.Name, property.GetValue(this));
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
        public async Task InitAsync(TCSProperties sesh) {
            Title = sesh.Title;
            Artist = sesh.Artist;
            Album = sesh.AlbumTitle;
            AlbumArtist = sesh.AlbumArtist;
            #pragma warning disable IDE0305 // Simplify collection initialization
            Genres = sesh.Genres.ToList();
            #pragma warning restore IDE0305 // Simplify collection initialization
            Thumbnail = await SpectreImg.RefToThumb(sesh.Thumbnail);
            TrackNumber = sesh.TrackNumber;
            TrackCount = sesh.AlbumTrackCount;
            PlaybackType = sesh.PlaybackType;
            Subtitle = sesh.Subtitle;
        }
    }
}
