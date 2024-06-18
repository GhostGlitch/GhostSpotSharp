using Windows.Media.Control;
using Windows.Media;
using Windows.Storage.Streams;
using System.IO;
using System.Windows;
using Windows.UI.Core;
using Windows.ApplicationModel.Core;
using Windows.Win32.UI.WindowsAndMessaging;
using Windows.Win32.Foundation;



Test.Hope();

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
        WriteLine(ex.ToString());
    }
    SeshMap[sesh] = props;
}
foreach (GhostProps props in SeshMap.Values) {
//#pragma warning disable CS8604 // Possible null reference argument.
    //GhostImg.DebugViewImage(props.Thumbnail, props.Title);
//#pragma warning restore CS8604 // Possible null reference argument.
    foreach (KeyValuePair<string, object?> kvp in props) {
        if (kvp.Key == "Genres") {
            List<string>? genres = (List<string>?)kvp.Value;
            WriteLine($"Genres: {(genres.Count > 0 ? string.Join(", ", genres) : "")}");
        } else if (kvp.Key != "Thumbnail") {
            WriteLine($"{kvp.Key}: {kvp.Value}");
        }
    }
    WriteLine();
}
