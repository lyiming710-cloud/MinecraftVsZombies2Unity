#if UNITY_IOS
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
            // Preserve the exact marketing/build versions in the generated iOS app.
            plist.root.SetString("CFBundleShortVersionString", "0.7.2");
            plist.root.SetString("CFBundleVersion", "702");

            plist.WriteToFile(plistPath);
        }
    }
}
#endif
