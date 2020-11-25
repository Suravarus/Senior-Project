
using Utilities;
namespace ProcGen
{
    /// <summary>
    /// Every room will have 4 hidden doors. All doors will 
    /// implement this interface.
    /// </summary>
    public interface IDoor
    {
        /// <summary>
        /// Returns North, South, East, or West.
        /// </summary>
        /// <returns></returns>
        CardinalDirection GetCardinalDir();
        /// <summary>
        /// This door will become visible within the room.
        /// </summary>
        void RevealDoor();
        /// <summary>
        /// Returns TRUE if the doors is visible.
        /// </summary>
        /// <returns></returns>
        bool IsVisible();
    }
}
