using GMap.NET;
using MissionPlanner.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SKYROVER.GCS.Core.Command
{
   public interface IMapTool
    {
        myGMAP MapControl { get; set; }
       
        string Name { get; }

        string Description { get; }

        bool Enabled { get; set; }

        event EventHandler EnabledChanged;

        Cursor Cursor { get; }

        event EventHandler CursorChanged;
        /// <summary>
        /// Function to perform some action on mouse hover
        /// </summary>
        /// <param name="mapPosition">The position at which the mouse hovers</param>
        /// <returns><value>true</value> if the action was handled and <b>no</b> other action should be taken</returns>
        bool DoMouseHover(PointLatLng mapPosition);

        /// <summary>
        /// Function to perform some action when the mouse enters the map
        /// </summary>
        /// <returns><value>true</value> if the action was handled and <b>no</b> other action should be taken</returns>
        bool DoMouseEnter();

        /// <summary>
        /// Function to perform some action when the mouse leaves the map
        /// </summary>
        /// <returns><value>true</value> if the action was handled and <b>no</b> other action should be taken</returns>
        bool DoMouseLeave();

        /// <summary>
        /// Function to perform some action when the map was double clicked at a certain position
        /// </summary>
        /// <param name="mapPosition">The position at which the mouse hovers</param>
        /// <param name="mouseEventArgs">The mouse event arguments</param>
        /// <returns><value>true</value> if the action was handled and <b>no</b> other action should be taken</returns>
        bool DoMouseDoubleClick(PointLatLng mapPosition, MouseEventArgs mouseEventArgs);

        /// <summary>
        /// Function to perform some action when a mouse button was "downed" on the map
        /// </summary>
        /// <param name="mapPosition">The position at which the mouse button was downed</param>
        /// <param name="mouseEventArgs">The mouse event arguments</param>
        /// <returns><value>true</value> if the action was handled and <b>no</b> other action should be taken</returns>
        bool DoMouseDown(PointLatLng mapPosition, MouseEventArgs mouseEventArgs);

        /// <summary>
        /// Function to perform some action when a mouse button was moved on the map
        /// </summary>
        /// <param name="mapPosition">The position to which the mouse moved</param>
        /// <param name="mouseEventArgs">The mouse event arguments</param>
        /// <returns><value>true</value> if the action was handled and <b>no</b> other action should be taken</returns>
        bool DoMouseMove(PointLatLng mapPosition, MouseEventArgs mouseEventArgs);

        /// <summary>
        /// Function to perform some action when a mouse button was "uped" on the map
        /// </summary>
        /// <param name="mapPosition">The position at which the mouse hovers</param>
        /// <param name="mouseEventArgs">The mouse event arguments</param>
        /// <returns><value>true</value> if the action was handled and <b>no</b> other action should be taken</returns>
        bool DoMouseUp(PointLatLng mapPosition, MouseEventArgs mouseEventArgs);

        /// <summary>
        /// Function to perform some action when a mouse wheel was scrolled on the map
        /// </summary>
        /// <param name="mapPosition">The position at which the mouse hovers</param>
        /// <param name="mouseEventArgs">The mouse event arguments</param>
        /// <returns><value>true</value> if the action was handled and <b>no</b> other action should be taken</returns>
        bool DoMouseWheel(PointLatLng mapPosition, MouseEventArgs mouseEventArgs);

        /// <summary>
        /// Some drawing operation of the tool
        /// </summary>
        /// <param name="e">The event's arguments</param>
        void DoPaint(PaintEventArgs e);

        /// <summary>
        /// Function to perform some action when a key was "downed" on the map
        /// </summary>
        /// <param name="mapPosition">The position at which the mouse hovers</param>
        /// <param name="keyEventArgs">The key event arguments</param>
        /// <returns><value>true</value> if the action was handled and <b>no</b> other action should be taken</returns>
        bool DoKeyDown(PointLatLng mapPosition, KeyEventArgs keyEventArgs);

        /// <summary>
        /// Function to perform some action when a key was "uped" on the map
        /// </summary>
        /// <param name="mapPosition">The position at which the mouse hovers</param>
        /// <param name="keyEventArgs">The key event arguments</param>
        /// <returns><value>true</value> if the action was handled and <b>no</b> other action should be taken</returns>
        bool DoKeyUp(PointLatLng mapPosition, KeyEventArgs keyEventArgs);

    }
}
