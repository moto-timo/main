﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NETSTANDARD1_6

using System.Diagnostics;

namespace System
{
    public sealed class OperatingSystem
    {
        private readonly Version _version;
        private readonly PlatformID _platform;
        private readonly string _servicePack;
        private string _versionString;

        private OperatingSystem() { }

        public OperatingSystem(PlatformID platform, Version version) :
            this(platform, version, null)
        { }

        internal OperatingSystem(PlatformID platform, Version version, string servicePack)
        {
            if (platform < PlatformID.Win32S || platform > PlatformID.MacOSX)
            {
                throw new ArgumentOutOfRangeException(nameof(platform));
            }

            if (version == null)
            {
                throw new ArgumentNullException(nameof(version));
            }

            _platform = platform;
            _version = version;
            _servicePack = servicePack;
        }

        public PlatformID Platform => _platform;

        public string ServicePack => _servicePack ?? string.Empty;

        public Version Version => _version;

        public object Clone() => new OperatingSystem(_platform, _version, _servicePack);

        public override string ToString() => VersionString;

        public string VersionString
        {
            get
            {
                if (_versionString == null)
                {
                    string os;
                    switch (_platform)
                    {
                        case PlatformID.Win32S: os = "Microsoft Win32S "; break;
                        case PlatformID.Win32Windows: os = (_version.Major > 4 || (_version.Major == 4 && _version.Minor > 0)) ? "Microsoft Windows 98 " : "Microsoft Windows 95 "; break;
                        case PlatformID.Win32NT: os = "Microsoft Windows NT "; break;
                        case PlatformID.WinCE: os = "Microsoft Windows CE "; break;
                        case PlatformID.Unix: os = "Unix "; break;
                        case PlatformID.Xbox: os = "Xbox "; break;
                        case PlatformID.MacOSX: os = "Mac OS X "; break;
                        default:
                            Debug.Fail($"Unknown platform {_platform}");
                            os = "<unknown> "; break;
                    }

                    _versionString = string.IsNullOrEmpty(_servicePack) ?
                        os + _version.ToString() :
                        os + _version.ToString(3) + " " + _servicePack;
                }

                return _versionString;
            }
        }
    }
}

#endif
