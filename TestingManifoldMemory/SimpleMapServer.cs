using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manifold.Interop;


using System;
using System.Collections.Generic;
using System.Text;
using M = Manifold.Interop;
using System.IO;

namespace LocationSolve.LayoutServer
{
    /// <summary>
    /// Custom wrapper for Manifold MapServer with standard settings.
    /// </summary>
    public class MapServer
    {
        private bool _isRunning;

        /// <summary>
        /// Gets or sets the map location.
        /// </summary>
        /// <value>The map location.</value>
        public string MapLocation { get; set; }
        /// <summary>
        /// Gets or sets the name of the map component.
        /// </summary>
        /// <value>The name of the map component.</value>
        public string MapComponentName { get; set; }
        private M.Application _application;
        private M.Document _document;
        private M.Map _map;


        /// <summary>
        /// Initializes a new instance of the <see cref="MapServer"/> class.
        /// </summary>
        /// <param name="MapLocation">The map location.</param>
        /// <param name="MapComponentName">Name of the map component.</param>
        public MapServer(String MapLocation, String MapComponentName)
        {
            if (String.IsNullOrEmpty(MapLocation))
            {
                throw new ArgumentNullException("MapLocation", "Map Location not set");
            }
            else
                if (String.IsNullOrEmpty(MapComponentName))
                {
                    throw new ArgumentNullException("MapComponentName", "Map Component Name not set");
                }

            MapServerConfig mc = new MapServerConfig();
            mc.MapPath = MapLocation;
            mc.ComponentName = MapComponentName;
            if (File.Exists(MapLocation) != true)
            {
                throw new FileNotFoundException(MapLocation + " does not exist");
            }
            //fire up the Manifold mapserver object!!!!  This can be error prone if not setup correctly
            M.MapServer ms = new M.MapServer();
            ms.CreateWithOpts(mc.CreateConfigFile(), string.Empty, null, false);
            _application = ms.Application;
            _document = ms.Document;
            _map = (M.Map)ms.Component;
            _isRunning = true;
        }

        public bool IsRunning()
        {
            return _isRunning;
        }

        /// <summary>
        /// Releases all resources and lock on Map file and resets all map file changes.  
        /// </summary>
        public void Dispose()
        {
            //create a new mapserver instance, then reload it.
            M.MapServer ms = new M.MapServer();
            ms.Reload();
            ms = null;
            GC.Collect();

        }

        /// <summary>
        /// Gets the Manifold application.
        /// </summary>
        /// <value>The Manifold application.</value>
        public M.Application Application { get { return _application; } }
        /// <summary>
        /// Gets the Manifold document.
        /// </summary>
        /// <value>The Manifold document.</value>
        public M.Document Document { get { return _document; } }
        /// <summary>
        /// Gets the Manifold map.
        /// </summary>
        /// <value>The Manifold map.</value>
        public M.Map Map { get { return _map; } }


    }
}

