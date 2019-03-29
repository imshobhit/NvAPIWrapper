﻿using System;
using System.Runtime.InteropServices;
using NvAPIWrapper.Native.Attributes;
using NvAPIWrapper.Native.General.Structures;
using NvAPIWrapper.Native.Helpers;
using NvAPIWrapper.Native.Interfaces;
using NvAPIWrapper.Native.Interfaces.DRS;

namespace NvAPIWrapper.Native.DRS.Structures
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    [StructureVersion(3)]
    public struct DRSApplicationV3 : IInitializable, IDRSApplication
    {
        public const char FileInFolderSeparator = DRSApplicationV2.FileInFolderSeparator;
        internal StructureVersion _Version;
        internal uint _IsPredefined;
        internal UnicodeString _ApplicationName;
        internal UnicodeString _FriendlyName;
        internal UnicodeString _LauncherName;
        internal UnicodeString _FileInFolder;
        internal uint _Flags;

        public DRSApplicationV3(
            string applicationName,
            string friendlyName = null,
            string launcherName = null,
            string[] fileInFolders = null,
            bool isMetro = false
        )
        {
            this = typeof(DRSApplicationV3).Instantiate<DRSApplicationV3>();
            IsPredefined = false;
            ApplicationName = applicationName;
            FriendlyName = friendlyName ?? string.Empty;
            LauncherName = launcherName ?? string.Empty;
            FilesInFolder = fileInFolders ?? new string[0];
            IsMetroApplication = isMetro;
        }

        public bool IsPredefined
        {
            get => _IsPredefined > 0;
            private set => _IsPredefined = value ? 1u : 0u;
        }

        public bool IsMetroApplication
        {
            get => _Flags.GetBit(0);
            private set => _Flags = _Flags.SetBit(0, value);
        }

        public bool HasCommandLine
        {
            get => _Flags.GetBit(1);
            private set => _Flags = _Flags.SetBit(1, value);
        }

        public string ApplicationName
        {
            get => _ApplicationName.Value;
            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Name can not be empty or null.");
                }

                _ApplicationName = new UnicodeString(value);
            }
        }

        public string FriendlyName
        {
            get => _FriendlyName.Value;
            private set => _FriendlyName = new UnicodeString(value);
        }

        public string LauncherName
        {
            get => _LauncherName.Value;
            private set => _LauncherName = new UnicodeString(value);
        }

        public string[] FilesInFolder
        {
            get => _FileInFolder.Value?.Split(new[] {FileInFolderSeparator}, StringSplitOptions.RemoveEmptyEntries) ??
                   new string[0];
            private set => _FileInFolder = new UnicodeString(string.Join(FileInFolderSeparator.ToString(), value));
        }
    }
}