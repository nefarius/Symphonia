using System;
using System.ComponentModel;
using PropertyChanged;
using Symphonia.DLNA.SOAP.Synology.ContentDirectory.Browse;

namespace Symphonia
{
    [AddINotifyPropertyChangedInterface]
    public class MainViewModel : INotifyPropertyChanged
    {
        public Item CurrentlyPlaying { get; set; }

        public string SearchQuery { get; set; }

        [Obsolete]
        public string SearchResult { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}