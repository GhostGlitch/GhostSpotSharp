using Windows.Media.Control;
using TCS = Windows.Media.Control.GlobalSystemMediaTransportControlsSession;
using TCSProperties = Windows.Media.Control.GlobalSystemMediaTransportControlsSessionMediaProperties;
using TCSManager = Windows.Media.Control.GlobalSystemMediaTransportControlsSessionManager;
using Windows.Media;
using System.Drawing;
using Windows.Storage.Streams;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using Windows.UI.Core;
using Windows.ApplicationModel.Core;
using GhostSpotSharp;

TCSManager Manager = await TCSManager.RequestAsync();

IReadOnlyList<TCS> Sessions = Manager.GetSessions();
Dictionary<TCS, GhostProps> SeshMap = [];
foreach (TCS sesh in Sessions) {
    GhostProps props = new();
    try {
        await Task.Run(async () =>
        {
            TCSProperties tst = await sesh.TryGetMediaPropertiesAsync();
            await props.InitAsync(tst);
        }).ConfigureAwait(false);
    }
    catch (Exception ex) {
        Console.WriteLine(sesh.ToString());
        Console.WriteLine(ex.ToString());
    }
    SeshMap[sesh] = props;
}
foreach (GhostProps props in SeshMap.Values) {
    Console.WriteLine(props);
    Console.WriteLine(props.Title);
    Console.WriteLine(props.Artist);
    Console.WriteLine(props.Album);
    Console.WriteLine(props.AlbumArtist);
    foreach (string Genre in props.Genres) {
        Console.WriteLine(Genre); }
    Console.WriteLine(props.Thumbnail);

#pragma warning disable CS8604 // Possible null reference argument.
    GhostImg.DebugViewImage(props.Thumbnail, props.Title);
#pragma warning restore CS8604 // Possible null reference argument.
    Console.WriteLine(props.TrackNumber);
    Console.WriteLine(props.TrackCount);
    Console.WriteLine(props.PlaybackType);
    Console.WriteLine(props.Subtitle);
}