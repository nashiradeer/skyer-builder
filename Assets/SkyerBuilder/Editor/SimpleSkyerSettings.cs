using UnityEngine;

namespace NashiraDeer.SkyerBuilder
{
    public class SimpleSkyerSettings : ScriptableObject
    {
        public string BuildPath = "";
        public bool PlaceInProductFolder = false;
        public bool PlaceInVersionFolder = false;

        public bool Windows = false;
        public bool Windows32 = false;
        public bool Linux = false;
        public bool MacOSX = false;
        public bool Android = false;
        public bool iOS = false;
        public bool WebGL = false;
        public bool WSA = false;
        public bool PS4 = false;
        public bool PS5 = false;
        public bool XboxOne = false;
        public bool Switch = false;
        public bool Stadia = false;
        public bool tvOS = false;
        public bool CloudRendenring = false;

        public bool DevelopmentBuild = false;
    }
}
