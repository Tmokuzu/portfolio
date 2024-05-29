#if UNITY_5 || UNITY_5_3_OR_NEWER
using UnityEngine;
using UnityEditor;

using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;

namespace DlibFaceLandmarkDetector
{
    class DlibFaceLandmarkDetectorMenuItem : MonoBehaviour
    {

        /// <summary>
        /// Dlib FaceLandmark Detector API Reference.
        /// </summary>
        [MenuItem ("Tools/Dlib FaceLandmark Detector/Open Dlib FaceLandmark Detector API Reference", false, 12)]
        public static void OpenDlibFaceLandmarkDetectorAPIReference ()
        {
            Application.OpenURL ("http://enoxsoftware.github.io/DlibFaceLandmarkDetector/doc/html/index.html");
        }

        /// <summary>
        /// Sets the plugin import settings.
        /// </summary>
        [MenuItem ("Tools/Dlib FaceLandmark Detector/Set Plugin Import Settings", false, 1)]
        public static void SetPluginImportSettings ()
        {

            //Disable Extra folder
            SetPlugins (new string[] { "Assets/DlibFaceLandmarkDetector/Extra/enable_avx/dlibfacelandmarkdetector.bundle" }, null, null);
            SetPlugins (GetPluginFilePaths ("Assets/DlibFaceLandmarkDetector/Extra/enable_avx/WSA/UWP/x64"), null, null);
            SetPlugins (GetPluginFilePaths ("Assets/DlibFaceLandmarkDetector/Extra/enable_avx/WSA/UWP/x86"), null, null);
            SetPlugins (GetPluginFilePaths ("Assets/DlibFaceLandmarkDetector/Extra/enable_avx/x86_64"), null, null);
            SetPlugins (GetPluginFilePaths ("Assets/DlibFaceLandmarkDetector/Extra/enable_avx/x86"), null, null);
            SetPlugins (new string[] { "Assets/DlibFaceLandmarkDetector/Extra/enable_sse4/dlibfacelandmarkdetector.bundle" }, null, null);
            SetPlugins (GetPluginFilePaths ("Assets/DlibFaceLandmarkDetector/Extra/enable_sse4/WSA/UWP/x64"), null, null);
            SetPlugins (GetPluginFilePaths ("Assets/DlibFaceLandmarkDetector/Extra/enable_sse4/WSA/UWP/x86"), null, null);
            SetPlugins (GetPluginFilePaths ("Assets/DlibFaceLandmarkDetector/Extra/enable_sse4/x86_64"), null, null);
            SetPlugins (GetPluginFilePaths ("Assets/DlibFaceLandmarkDetector/Extra/enable_sse4/x86"), null, null);

            //Android
            SetPlugins (GetPluginFilePaths ("Assets/DlibFaceLandmarkDetector/Plugins/Android/libs/armeabi-v7a"), null,
                new Dictionary<BuildTarget, Dictionary<string, string>> () { {BuildTarget.Android,new Dictionary<string, string> () { {
                                "CPU",
                                "ARMv7"
                            }
                        }
                    }
                });
            SetPlugins (GetPluginFilePaths ("Assets/DlibFaceLandmarkDetector/Plugins/Android/libs/x86"), null,
                new Dictionary<BuildTarget, Dictionary<string, string>> () { {BuildTarget.Android,new Dictionary<string, string> () { {
                                "CPU",
                                "x86"
                            }
                        }
                    }
                });
#if UNITY_2018_1_OR_NEWER && ENABLE_IL2CPP
            SetPlugins (GetPluginFilePaths ("Assets/DlibFaceLandmarkDetector/Plugins/Android/libs/arm64-v8a"), null,
                new Dictionary<BuildTarget, Dictionary<string, string>> () { {BuildTarget.Android,new Dictionary<string, string> () { {
                                "CPU",
                                "ARM64"
                            }
                        }
                    }
                });
#else
            SetPlugins (GetPluginFilePaths ("Assets/DlibFaceLandmarkDetector/Plugins/Android/libs/arm64-v8a"), null, null);
#endif

            //iOS
            SetPlugins (GetPluginFilePaths ("Assets/DlibFaceLandmarkDetector/Plugins/iOS"), null,
                new Dictionary<BuildTarget, Dictionary<string, string>> () { {
                        BuildTarget.iOS,
                        null
                    }
                });

            //OSX
            SetPlugins (new string[] { "Assets/DlibFaceLandmarkDetector/Plugins/dlibfacelandmarkdetector.bundle" }, new Dictionary<string, string> () { {
                    "CPU",
                    "AnyCPU"
                }, {
                    "OS",
                    "OSX"
                }
            },
                new Dictionary<BuildTarget, Dictionary<string, string>> () {
#if UNITY_2017_3_OR_NEWER
                    {
                        BuildTarget.StandaloneOSX,new Dictionary<string, string> () { {
                                "CPU",
                                "AnyCPU"
                            }
                        }
                    }
#else
                    {
                        BuildTarget.StandaloneOSXIntel,new Dictionary<string, string> () { {
                                "CPU",
                                "x86"
                            }
                        }
                    }, {
                        BuildTarget.StandaloneOSXIntel64,new Dictionary<string, string> () { {
                                "CPU",
                                "x86_64"
                            }
                        }
                    }, {
                        BuildTarget.StandaloneOSXUniversal,new Dictionary<string, string> () { {
                                "CPU",
                                "AnyCPU"
                            }
                        }
                    }
#endif
                });

            //Windows
            SetPlugins (new string[] { "Assets/DlibFaceLandmarkDetector/Plugins/x86/dlibfacelandmarkdetector.dll" }, new Dictionary<string, string> () { {
                    "CPU",
                    "x86"
                }, {
                    "OS",
                    "Windows"
                }
            },
                new Dictionary<BuildTarget, Dictionary<string, string>> () { {BuildTarget.StandaloneWindows,new Dictionary<string, string> () { {
                                "CPU",
                                "x86"
                            }
                        }
                    }
                });
            SetPlugins (new string[] { "Assets/DlibFaceLandmarkDetector/Plugins/x86_64/dlibfacelandmarkdetector.dll" }, new Dictionary<string, string> () { {
                    "CPU",
                    "x86_64"
                }, {
                    "OS",
                    "Windows"
                }
            },
                new Dictionary<BuildTarget, Dictionary<string, string>> () { {BuildTarget.StandaloneWindows64,new Dictionary<string, string> () { {
                                "CPU",
                                "x86_64"
                            }
                        }
                    }
                });

            //Linux
            SetPlugins (new string[] { "Assets/DlibFaceLandmarkDetector/Plugins/x86/libdlibfacelandmarkdetector.so" }, new Dictionary<string, string> () { {
                    "CPU",
                    "x86"
                }, {
                    "OS",
                    "Linux"
                }
            },
                new Dictionary<BuildTarget, Dictionary<string, string>> () { {BuildTarget.StandaloneLinux,new Dictionary<string, string> () { {
                                "CPU",
                                "x86"
                            }
                        }
                    },
                });
            SetPlugins (new string[] { "Assets/DlibFaceLandmarkDetector/Plugins/x86_64/libdlibfacelandmarkdetector.so" }, new Dictionary<string, string> () { {
                    "CPU",
                    "x86_64"
                }, {
                    "OS",
                    "Linux"
                }
            },
                new Dictionary<BuildTarget, Dictionary<string, string>> () { {BuildTarget.StandaloneLinux64,new Dictionary<string, string> () { {
                                "CPU",
                                "x86_64"
                            }
                        }
                    },
                });
                            

            //UWP
            #if UNITY_5_0 || UNITY_5_1
            SetPlugins (GetPluginFilePaths ("Assets/DlibFaceLandmarkDetector/Plugins/WSA/UWP/ARM"), null, null);
            SetPlugins (GetPluginFilePaths ("Assets/DlibFaceLandmarkDetector/Plugins/WSA/UWP/x64"), null, null);
            SetPlugins (GetPluginFilePaths ("Assets/DlibFaceLandmarkDetector/Plugins/WSA/UWP/x86"), null, null);
            #else
            SetPlugins (GetPluginFilePaths ("Assets/DlibFaceLandmarkDetector/Plugins/WSA/UWP/ARM"), null,
                new Dictionary<BuildTarget, Dictionary<string, string>> () { {BuildTarget.WSAPlayer,new Dictionary<string, string> () { {
                                "SDK",
                                "UWP"
                            }, {
                                "CPU",
                                "ARM"
                            }
                        }
                    }
                });
            SetPlugins (GetPluginFilePaths ("Assets/DlibFaceLandmarkDetector/Plugins/WSA/UWP/x64"), null,
                new Dictionary<BuildTarget, Dictionary<string, string>> () { {BuildTarget.WSAPlayer,new Dictionary<string, string> () { {
                                "SDK",
                                "UWP"
                            }, {
                                "CPU",
                                "x64"
                            }
                        }
                    }
                });
            SetPlugins (GetPluginFilePaths ("Assets/DlibFaceLandmarkDetector/Plugins/WSA/UWP/x86"), null,
                new Dictionary<BuildTarget, Dictionary<string, string>> () { {BuildTarget.WSAPlayer,new Dictionary<string, string> () { {
                                "SDK",
                                "UWP"
                            }, {
                                "CPU",
                                "x86"
                            }
                        }
                    }
                });
            #endif

            //WebGL
            #if UNITY_5_3 || UNITY_5_4
            SetPlugins (new string[] { "Assets/DlibFaceLandmarkDetector/Plugins/WebGL/5.3-5.4/dlibfacelandmarktracker.bc" }, null,
                new Dictionary<BuildTarget, Dictionary<string, string>> () { {
                        BuildTarget.WebGL,
                        null
                    }
                });
            SetPlugins (new string[] { "Assets/DlibFaceLandmarkDetector/Plugins/WebGL/5.5/dlibfacelandmarktracker.bc" }, null, null);
            SetPlugins (new string[] { "Assets/DlibFaceLandmarkDetector/Plugins/WebGL/5.6/dlibfacelandmarktracker.bc" }, null, null);
            #elif UNITY_5_5
            SetPlugins (new string[] { "Assets/DlibFaceLandmarkDetector/Plugins/WebGL/5.3-5.4/dlibfacelandmarktracker.bc" }, null,
            null);
            SetPlugins (new string[] { "Assets/DlibFaceLandmarkDetector/Plugins/WebGL/5.5/dlibfacelandmarktracker.bc" }, null, new Dictionary<BuildTarget, Dictionary<string, string>> () { { BuildTarget.WebGL, null } });
            SetPlugins (new string[] { "Assets/DlibFaceLandmarkDetector/Plugins/WebGL/5.6/dlibfacelandmarktracker.bc" }, null, null);
            #elif UNITY_5_6_OR_NEWER
            SetPlugins (new string[] { "Assets/DlibFaceLandmarkDetector/Plugins/WebGL/5.3-5.4/dlibfacelandmarktracker.bc" }, null,
            null);
            SetPlugins (new string[] { "Assets/DlibFaceLandmarkDetector/Plugins/WebGL/5.5/dlibfacelandmarktracker.bc" }, null, null);
            SetPlugins (new string[] { "Assets/DlibFaceLandmarkDetector/Plugins/WebGL/5.6/dlibfacelandmarktracker.bc" }, null, new Dictionary<BuildTarget, Dictionary<string, string>> () { { BuildTarget.WebGL, null } });
            #else
            SetPlugins (new string[] { "Assets/DlibFaceLandmarkDetector/Plugins/WebGL/5.3-5.4/dlibfacelandmarktracker.bc" }, null,
            null);
            SetPlugins (new string[] { "Assets/DlibFaceLandmarkDetector/Plugins/WebGL/5.5/dlibfacelandmarktracker.bc" }, null, null);
            SetPlugins (new string[] { "Assets/DlibFaceLandmarkDetector/Plugins/WebGL/5.6/dlibfacelandmarktracker.bc" }, null, null);
            #endif
        }

        /// <summary>
        /// Gets the plugin file paths.
        /// </summary>
        /// <returns>The plugin file paths.</returns>
        /// <param name="folderPath">Folder path.</param>
        static string[] GetPluginFilePaths (string folderPath)
        {
            Regex reg = new Regex (".meta$|.DS_Store$|.zip");
            try {
                return Directory.GetFiles (folderPath).Where (f => !reg.IsMatch (f)).ToArray ();
            } catch (Exception ex) {
                Debug.LogWarning ("SetPluginImportSettings Faild :" + ex);
                return null;
            }
        }

        /// <summary>
        /// Sets the plugins.
        /// </summary>
        /// <param name="files">Files.</param>
        /// <param name="editorSettings">Editor settings.</param>
        /// <param name="settings">Settings.</param>
        public static void SetPlugins (string[] files, Dictionary<string, string> editorSettings, Dictionary<BuildTarget, Dictionary<string, string>> settings)
        {
            if (files == null)
                return;
            
            foreach (string item in files) {
                
                PluginImporter pluginImporter = PluginImporter.GetAtPath (item) as PluginImporter;
                
                if (pluginImporter != null) {
                    
                    pluginImporter.SetCompatibleWithAnyPlatform (false);
                    pluginImporter.SetCompatibleWithEditor (false);
                    pluginImporter.SetCompatibleWithPlatform (BuildTarget.Android, false);
                    pluginImporter.SetCompatibleWithPlatform (BuildTarget.iOS, false);
                    pluginImporter.SetCompatibleWithPlatform (BuildTarget.StandaloneWindows, false);
                    pluginImporter.SetCompatibleWithPlatform (BuildTarget.StandaloneWindows64, false);
#if UNITY_2017_3_OR_NEWER
                    pluginImporter.SetCompatibleWithPlatform (BuildTarget.StandaloneOSX, false);
#else
                    pluginImporter.SetCompatibleWithPlatform (BuildTarget.StandaloneOSXIntel, false);
                    pluginImporter.SetCompatibleWithPlatform (BuildTarget.StandaloneOSXIntel64, false);
                    pluginImporter.SetCompatibleWithPlatform (BuildTarget.StandaloneOSXUniversal, false);
#endif
                    pluginImporter.SetCompatibleWithPlatform (BuildTarget.StandaloneLinux, false);
                    pluginImporter.SetCompatibleWithPlatform (BuildTarget.StandaloneLinux64, false);
                    pluginImporter.SetCompatibleWithPlatform (BuildTarget.StandaloneLinuxUniversal, false);
                    pluginImporter.SetCompatibleWithPlatform (BuildTarget.WSAPlayer, false);
                    pluginImporter.SetCompatibleWithPlatform (BuildTarget.WebGL, false);
                    
                    
                    if (editorSettings != null) {
                        pluginImporter.SetCompatibleWithEditor (true);
                        
                        foreach (KeyValuePair<string, string> pair in editorSettings) {
                            if (pluginImporter.GetEditorData (pair.Key) != pair.Value) {
                                pluginImporter.SetEditorData (pair.Key, pair.Value);
                            }
                        }
                    }
                    
                    if (settings != null) {
                        foreach (KeyValuePair<BuildTarget, Dictionary<string, string>> settingPair in settings) {
                            
                            pluginImporter.SetCompatibleWithPlatform (settingPair.Key, true);
                            if (settingPair.Value != null) {
                                foreach (KeyValuePair<string, string> pair in settingPair.Value) {
                                    if (pluginImporter.GetPlatformData (settingPair.Key, pair.Key) != pair.Value) {
                                        pluginImporter.SetPlatformData (settingPair.Key, pair.Key, pair.Value);
                                    }
                                }
                            }
                            
                        }
                    } else {
                        pluginImporter.SetCompatibleWithPlatform (BuildTarget.Android, false);
                        pluginImporter.SetCompatibleWithPlatform (BuildTarget.iOS, false);
                        pluginImporter.SetCompatibleWithPlatform (BuildTarget.StandaloneWindows, false);
                        pluginImporter.SetCompatibleWithPlatform (BuildTarget.StandaloneWindows64, false);
#if UNITY_2017_3_OR_NEWER
                        pluginImporter.SetCompatibleWithPlatform (BuildTarget.StandaloneOSX, false);
#else
                        pluginImporter.SetCompatibleWithPlatform (BuildTarget.StandaloneOSXIntel, false);
                        pluginImporter.SetCompatibleWithPlatform (BuildTarget.StandaloneOSXIntel64, false);
                        pluginImporter.SetCompatibleWithPlatform (BuildTarget.StandaloneOSXUniversal, false);
#endif
                        pluginImporter.SetCompatibleWithPlatform (BuildTarget.StandaloneLinux, false);
                        pluginImporter.SetCompatibleWithPlatform (BuildTarget.StandaloneLinux64, false);
                        pluginImporter.SetCompatibleWithPlatform (BuildTarget.StandaloneLinuxUniversal, false);
                        pluginImporter.SetCompatibleWithPlatform (BuildTarget.WSAPlayer, false);
                        pluginImporter.SetCompatibleWithPlatform (BuildTarget.WebGL, false);
                    }
                    
                    
                    pluginImporter.SaveAndReimport ();
                    
                    Debug.Log ("SetPluginImportSettings Success :" + item);
                } else {
                    Debug.LogWarning ("SetPluginImportSettings Faild :" + item);
                }
            }
        }
    }
}
#endif