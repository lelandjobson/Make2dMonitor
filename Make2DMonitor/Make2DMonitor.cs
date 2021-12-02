using Eto.Forms;
using Rhino;
using System;
using System.Diagnostics;

namespace Make2dMonitor
{
    ///<summary>
    /// <para>Every RhinoCommon .rhp assembly must have one and only one PlugIn-derived
    /// class. DO NOT create instances of this class yourself. It is the
    /// responsibility of Rhino to create an instance of this class.</para>
    /// <para>To complete plug-in information, please also see all PlugInDescription
    /// attributes in AssemblyInfo.cs (you might need to click "Project" ->
    /// "Show All Files" to see it in the "Solution Explorer" window).</para>
    ///</summary>
    public class Make2dMonitorPlugin : Rhino.PlugIns.PlugIn
    {
        public Make2dMonitorPlugin()
        {
            Instance = this;
            RhinoApp.KeyboardEvent += RhinoApp_KeyboardEvent;
        }

        bool _checkMake2d = false;

        private void RhinoApp_KeyboardEvent(int key)
        {
            if (_checkMake2d)
            {
                // Check command history
                CheckMake2d();
                _checkMake2d = false;
            }
            if (key == (char)13)
            {
                _checkMake2d = true;
                CheckMake2d();
                // Check if the command history has Make2D
            }
        }

        static int _remaining = 1;

        void CheckMake2d()
        {
            if (RhinoApp.CommandHistoryWindowText.Contains("Make2d", StringComparison.OrdinalIgnoreCase))
            {
                _checkMake2d = false;
                if(_remaining == 0)
                {
                    foreach (var process in Process.GetProcessesByName("Rhino"))
                    {
                        process.Kill();
                    }
                }
                else
                {
                    MessageBox.Show("Hey!", MessageBoxType.Warning);
                    RhinoApp.CommandPrompt = "No Make 2d allowed!";
                    _remaining--;
                }
            }
        }

        ///<summary>Gets the only instance of the Make2dMonitorPlugin plug-in.</summary>
        public static Make2dMonitorPlugin Instance { get; private set; }


        // You can override methods here to change the plug-in behavior on
        // loading and shut down, add options pages to the Rhino _Option command
        // and maintain plug-in wide options in a document.
    }
}