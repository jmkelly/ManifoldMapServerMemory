using System;
using System.Collections.Generic;
using System.Text;
using M = Manifold.Interop;

//simple object to build the config elements for the Manifold.Interop.MapServer
//set up with mostly default values

namespace LocationSolve.LayoutServer
{
    /// <summary>
    /// Generates the Manifold map server config file automatically.
    /// 
    /// </summary>
    public class MapServerConfig
    {
        private string _mapPath;
        private string _componentName;
        private Int32 _cx = 256;
        private Int32 _cy = 256;
        private bool _logo;
        private Int32 _refreshLinks = 60;
        private bool _refreshLinksOnOpen;

        /// <summary>
        /// Gets or sets the map path.
        /// </summary>
        /// <value>The map path.</value>
        public string MapPath { get { return _mapPath; } set { _mapPath = value; } }
        /// <summary>
        /// Gets or sets the name of the map component.
        /// </summary>
        /// <value>The name of the map component.</value>
        public string ComponentName { get { return _componentName; } set { _componentName = value; } }
        /// <summary>
        /// Gets or sets the CX.
        /// </summary>
        /// <value>The CX.</value>
        public Int32 CX { get { return _cx; } set { _cx = value; } }
        /// <summary>
        /// Gets or sets the CY.
        /// </summary>
        /// <value>The CY.</value>
        public Int32 CY { get { return _cy; } set { _cy = value; } }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MapServerConfig"/> is logo.
        /// </summary>
        /// <value><c>true</c> if logo; otherwise, <c>false</c>.</value>
        public bool Logo { get { return _logo; } set { _logo = value; } }
        /// <summary>
        /// Gets or sets the refresh links time in minutes.
        /// </summary>
        /// <value>The refresh links.</value>
        public Int32 RefreshLinks { get { return _refreshLinks; } set { _refreshLinks = value; } }
        /// <summary>
        /// Gets or sets a value indicating whether links are to be refreshed on open].
        /// </summary>
        /// <value><c>true</c> if [refresh link on open]; otherwise, <c>false</c>.</value>
        public bool RefreshLinkOnOpen { get { return _refreshLinksOnOpen; } set { _refreshLinksOnOpen = value; } }

        /// <summary>
        /// Creates the config file.
        /// </summary>
        /// <returns></returns>
        public string CreateConfigFile()
        {
            if (String.IsNullOrEmpty(_mapPath))
            {
                throw new ArgumentNullException(_mapPath, "Map Location not set");
            }
            else
                if (String.IsNullOrEmpty(_componentName))
                {
                    throw new ArgumentNullException(_componentName, "Map Component Name not set");
                }
            List<string> aList = new List<string>();
            aList.Add("file = " + _mapPath);
            aList.Add("component = " + _componentName);
            aList.Add("cx = " + _cx);
            aList.Add("cy = " + _cy);
            aList.Add("logo = " + _logo);
            aList.Add("refreshLinks = " + _refreshLinks);
            aList.Add("refreshLinksOnOpen = " + _refreshLinksOnOpen);

            string config = "";

            foreach (string item in aList)
            {
                config = config + item + System.Environment.NewLine;
            }

            return config;

        }
    }
}

