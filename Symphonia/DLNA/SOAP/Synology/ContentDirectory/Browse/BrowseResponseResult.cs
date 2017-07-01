/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */

using System.Collections.Generic;
using System.Xml.Serialization;

namespace Symphonia.DLNA.SOAP.Synology.ContentDirectory.Browse
{
    [XmlRoot(ElementName = "albumArtURI", Namespace = "urn:schemas-upnp-org:metadata-1-0/upnp/")]
    public class AlbumArtURI
    {
        [XmlAttribute(AttributeName = "profileID", Namespace = "urn:schemas-dlna-org:metadata-1-0/")]
        public string ProfileID { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "res", Namespace = "urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/")]
    public class Res
    {
        [XmlAttribute(AttributeName = "protocolInfo")]
        public string ProtocolInfo { get; set; }

        [XmlAttribute(AttributeName = "size")]
        public string Size { get; set; }

        [XmlAttribute(AttributeName = "bitrate")]
        public string Bitrate { get; set; }

        [XmlAttribute(AttributeName = "duration")]
        public string Duration { get; set; }

        [XmlAttribute(AttributeName = "nrAudioChannels")]
        public string NrAudioChannels { get; set; }

        [XmlAttribute(AttributeName = "sampleFrequency")]
        public string SampleFrequency { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "item", Namespace = "urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/")]
    public class Item
    {
        [XmlElement(ElementName = "title", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string Title { get; set; }

        [XmlElement(ElementName = "class", Namespace = "urn:schemas-upnp-org:metadata-1-0/upnp/")]
        public string Class { get; set; }

        [XmlElement(ElementName = "artist", Namespace = "urn:schemas-upnp-org:metadata-1-0/upnp/")]
        public string Artist { get; set; }

        [XmlElement(ElementName = "creator", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string Creator { get; set; }

        [XmlElement(ElementName = "genre", Namespace = "urn:schemas-upnp-org:metadata-1-0/upnp/")]
        public string Genre { get; set; }

        [XmlElement(ElementName = "albumArtURI", Namespace = "urn:schemas-upnp-org:metadata-1-0/upnp/")]
        public AlbumArtURI AlbumArtURI { get; set; }

        [XmlElement(ElementName = "res", Namespace = "urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/")]
        public Res Res { get; set; }

        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "parentID")]
        public string ParentID { get; set; }

        [XmlAttribute(AttributeName = "restricted")]
        public string Restricted { get; set; }

        [XmlElement(ElementName = "originalTrackNumber", Namespace = "urn:schemas-upnp-org:metadata-1-0/upnp/")]
        public string OriginalTrackNumber { get; set; }

        [XmlElement(ElementName = "date", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string Date { get; set; }

        [XmlElement(ElementName = "album", Namespace = "urn:schemas-upnp-org:metadata-1-0/upnp/")]
        public string Album { get; set; }

        [XmlElement(ElementName = "author", Namespace = "urn:schemas-upnp-org:metadata-1-0/upnp/")]
        public string Author { get; set; }

        [XmlElement(ElementName = "albumArtist", Namespace = "urn:schemas-upnp-org:metadata-1-0/upnp/")]
        public string AlbumArtist { get; set; }
    }

    [XmlRoot(ElementName = "DIDL-Lite", Namespace = "urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/")]
    public class DIDLLite
    {
        [XmlElement(ElementName = "item", Namespace = "urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/")]
        public List<Item> Item { get; set; }

        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }

        [XmlAttribute(AttributeName = "dc", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Dc { get; set; }

        [XmlAttribute(AttributeName = "upnp", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Upnp { get; set; }

        [XmlAttribute(AttributeName = "sec", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Sec { get; set; }

        [XmlAttribute(AttributeName = "dlna", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Dlna { get; set; }
    }
}