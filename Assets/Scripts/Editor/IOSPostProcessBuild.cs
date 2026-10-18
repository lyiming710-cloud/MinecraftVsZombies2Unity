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

            plist.WriteToFile(plistPath);
        }
    }
}
#endif
