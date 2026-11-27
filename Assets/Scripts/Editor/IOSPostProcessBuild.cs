#if UNITY_IOS
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace MVZ2.Editor
{
    public static class IOSPostProcessBuild
    {
        [PostProcessBuild(999)]
        public static void OnPostProcessBuild(BuildTarget target, string buildPath)
        {
            if (target != BuildTarget.iOS)
                return;

            var plistPath = Path.Combine(buildPath, "Info.plist");
            var plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            // Opt out of iPadOS compatibility-mode pointer translation so
            // trackpads/mice remain distinguishable from direct finger touches.
            plist.root.SetBoolean("UIApplicationSupportsIndirectInputEvents", true);

            // Preserve the resolved marketing/build versions without hardcoding them.
            var shortVersion = Environment.GetEnvironmentVariable("MVZ2_APP_VERSION");
            if (string.IsNullOrWhiteSpace(shortVersion))
                shortVersion = PlayerSettings.bundleVersion;

            var buildVersion = Environment.GetEnvironmentVariable("MVZ2_BUILD_NUMBER");
            if (string.IsNullOrWhiteSpace(buildVersion))
                buildVersion = PlayerSettings.iOS.buildNumber;

            if (!string.IsNullOrWhiteSpace(shortVersion))
                plist.root.SetString("CFBundleShortVersionString", shortVersion.Trim());
            if (!string.IsNullOrWhiteSpace(buildVersion))
                plist.root.SetString("CFBundleVersion", buildVersion.Trim());

            plist.WriteToFile(plistPath);
        }
    }
}
#endif
