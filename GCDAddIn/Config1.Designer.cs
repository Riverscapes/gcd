//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GCDAddIn {
    using ESRI.ArcGIS.Framework;
    using ESRI.ArcGIS.ArcMapUI;
    using ESRI.ArcGIS.Editor;
    using ESRI.ArcGIS.esriSystem;
    using System;
    using System.Collections.Generic;
    using ESRI.ArcGIS.Desktop.AddIns;
    
    
    /// <summary>
    /// A class for looking up declarative information in the associated configuration xml file (.esriaddinx).
    /// </summary>
    internal static class ThisAddIn {
        
        internal static string Name {
            get {
                return "Geomorphic Change Detection (GCD) 7";
            }
        }
        
        internal static string AddInID {
            get {
                return "{f8ca7dea-22fb-40a3-8412-b84a4b528602}";
            }
        }
        
        internal static string Company {
            get {
                return "North Arrow Research";
            }
        }
        
        internal static string Version {
            get {
                return "7.0.11";
            }
        }
        
        internal static string Description {
            get {
                return "Geomorphic Change Detection (GCD) Software";
            }
        }
        
        internal static string Author {
            get {
                return "Philip Bailey";
            }
        }
        
        internal static string Date {
            get {
                return "04/04/2018";
            }
        }
        
        internal static ESRI.ArcGIS.esriSystem.UID ToUID(this System.String id) {
            ESRI.ArcGIS.esriSystem.UID uid = new ESRI.ArcGIS.esriSystem.UIDClass();
            uid.Value = id;
            return uid;
        }
        
        /// <summary>
        /// A class for looking up Add-in id strings declared in the associated configuration xml file (.esriaddinx).
        /// </summary>
        internal class IDs {
            
            /// <summary>
            /// Returns 'GCD7_AddIn_ToolbarExplorerButton', the id declared for Add-in Button class 'GCDAddIn.Project.btnProjectExplorer'
            /// </summary>
            internal static string GCDAddIn_Project_btnProjectExplorer {
                get {
                    return "GCD7_AddIn_ToolbarExplorerButton";
                }
            }
            
            /// <summary>
            /// Returns 'GCD7_AddIn_NewProjectButton', the id declared for Add-in Button class 'GCDAddIn.Project.btnNewProject'
            /// </summary>
            internal static string GCDAddIn_Project_btnNewProject {
                get {
                    return "GCD7_AddIn_NewProjectButton";
                }
            }
            
            /// <summary>
            /// Returns 'GCD7_AddIn_OpenProjectButton', the id declared for Add-in Button class 'GCDAddIn.Project.btnOpenProject'
            /// </summary>
            internal static string GCDAddIn_Project_btnOpenProject {
                get {
                    return "GCD7_AddIn_OpenProjectButton";
                }
            }
            
            /// <summary>
            /// Returns 'GCD7_AddIn_CloseProjectButton', the id declared for Add-in Button class 'GCDAddIn.Project.btnCloseProject'
            /// </summary>
            internal static string GCDAddIn_Project_btnCloseProject {
                get {
                    return "GCD7_AddIn_CloseProjectButton";
                }
            }
            
            /// <summary>
            /// Returns 'GCD7_AddIn_DockableProjectExplorerButton', the id declared for Add-in Button class 'GCDAddIn.Project.btnProjectExplorer2'
            /// </summary>
            internal static string GCDAddIn_Project_btnProjectExplorer2 {
                get {
                    return "GCD7_AddIn_DockableProjectExplorerButton";
                }
            }
            
            /// <summary>
            /// Returns 'GCD7_AddIn_ProjectPropertiesButton', the id declared for Add-in Button class 'GCDAddIn.Project.btnProjectProperties'
            /// </summary>
            internal static string GCDAddIn_Project_btnProjectProperties {
                get {
                    return "GCD7_AddIn_ProjectPropertiesButton";
                }
            }
            
            /// <summary>
            /// Returns 'GCD7_AddIn_OptionsButton', the id declared for Add-in Button class 'GCDAddIn.Customize.btnOptions'
            /// </summary>
            internal static string GCDAddIn_Customize_btnOptions {
                get {
                    return "GCD7_AddIn_OptionsButton";
                }
            }
            
            /// <summary>
            /// Returns 'GCD7_AddIn_FISLibraryButton', the id declared for Add-in Button class 'GCDAddIn.Customize.btnFISLibrary'
            /// </summary>
            internal static string GCDAddIn_Customize_btnFISLibrary {
                get {
                    return "GCD7_AddIn_FISLibraryButton";
                }
            }
            
            /// <summary>
            /// Returns 'GCD7_AddIn_CleanRasterButton', the id declared for Add-in Button class 'GCDAddIn.DataPreparation.btnCleanRaster'
            /// </summary>
            internal static string GCDAddIn_DataPreparation_btnCleanRaster {
                get {
                    return "GCD7_AddIn_CleanRasterButton";
                }
            }
            
            /// <summary>
            /// Returns 'GCD7_AddIn_ConcaveHull', the id declared for Add-in Button class 'ConcaveHullButton'
            /// </summary>
            internal static string ConcaveHullButton {
                get {
                    return "GCD7_AddIn_ConcaveHull";
                }
            }
            
            /// <summary>
            /// Returns 'GCD7_AddIn_AboutButton', the id declared for Add-in Button class 'GCDAddIn.Help.btnAbout'
            /// </summary>
            internal static string GCDAddIn_Help_btnAbout {
                get {
                    return "GCD7_AddIn_AboutButton";
                }
            }
            
            /// <summary>
            /// Returns 'GCD7_AddIn_OnlineHelp', the id declared for Add-in Button class 'GCDAddIn.Help.btnOnlineHelp'
            /// </summary>
            internal static string GCDAddIn_Help_btnOnlineHelp {
                get {
                    return "GCD7_AddIn_OnlineHelp";
                }
            }
            
            /// <summary>
            /// Returns 'GCD7_AddIn_WebSite', the id declared for Add-in Button class 'GCDAddIn.Help.btnWebSite'
            /// </summary>
            internal static string GCDAddIn_Help_btnWebSite {
                get {
                    return "GCD7_AddIn_WebSite";
                }
            }
            
            /// <summary>
            /// Returns 'GCD7_AddIn_TATWebSite', the id declared for Add-in Button class 'GCDAddIn.Help.btnTATWebSite'
            /// </summary>
            internal static string GCDAddIn_Help_btnTATWebSite {
                get {
                    return "GCD7_AddIn_TATWebSite";
                }
            }
            
            /// <summary>
            /// Returns 'GCD7_ucProjectManager', the id declared for Add-in DockableWindow class 'GCDAddIn.ucProjectManager+AddinImpl'
            /// </summary>
            internal static string GCDAddIn_ucProjectManager {
                get {
                    return "GCD7_ucProjectManager";
                }
            }
            
            /// <summary>
            /// Returns 'North_Arrow_Research_GCDAddIn_GCDExtension', the id declared for Add-in Extension class 'GCDExtension'
            /// </summary>
            internal static string GCDExtension {
                get {
                    return "North_Arrow_Research_GCDAddIn_GCDExtension";
                }
            }
        }
    }
    
internal static class ArcMap
{
  private static IApplication s_app = null;
  private static IDocumentEvents_Event s_docEvent;

  public static IApplication Application
  {
    get
    {
      if (s_app == null)
      {
        s_app = Internal.AddInStartupObject.GetHook<IMxApplication>() as IApplication;
        if (s_app == null)
        {
          IEditor editorHost = Internal.AddInStartupObject.GetHook<IEditor>();
          if (editorHost != null)
            s_app = editorHost.Parent;
        }
      }
      return s_app;
    }
  }

  public static IMxDocument Document
  {
    get
    {
      if (Application != null)
        return Application.Document as IMxDocument;

      return null;
    }
  }
  public static IMxApplication ThisApplication
  {
    get { return Application as IMxApplication; }
  }
  public static IDockableWindowManager DockableWindowManager
  {
    get { return Application as IDockableWindowManager; }
  }
  public static IDocumentEvents_Event Events
  {
    get
    {
      s_docEvent = Document as IDocumentEvents_Event;
      return s_docEvent;
    }
  }
  public static IEditor Editor
  {
    get
    {
      UID editorUID = new UID();
      editorUID.Value = "esriEditor.Editor";
      return Application.FindExtensionByCLSID(editorUID) as IEditor;
    }
  }
}

namespace Internal
{
  [StartupObjectAttribute()]
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
  public sealed partial class AddInStartupObject : AddInEntryPoint
  {
    private static AddInStartupObject _sAddInHostManager;
    private List<object> m_addinHooks = null;

    [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
    public AddInStartupObject()
    {
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
    protected override bool Initialize(object hook)
    {
      bool createSingleton = _sAddInHostManager == null;
      if (createSingleton)
      {
        _sAddInHostManager = this;
        m_addinHooks = new List<object>();
        m_addinHooks.Add(hook);
      }
      else if (!_sAddInHostManager.m_addinHooks.Contains(hook))
        _sAddInHostManager.m_addinHooks.Add(hook);

      return createSingleton;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
    protected override void Shutdown()
    {
      _sAddInHostManager = null;
      m_addinHooks = null;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
    internal static T GetHook<T>() where T : class
    {
      if (_sAddInHostManager != null)
      {
        foreach (object o in _sAddInHostManager.m_addinHooks)
        {
          if (o is T)
            return o as T;
        }
      }

      return null;
    }

    // Expose this instance of Add-in class externally
    public static AddInStartupObject GetThis()
    {
      return _sAddInHostManager;
    }
  }
}
}
