namespace ProjectNahual.Utils
{
    using UnityEngine;

    public static class CursorHandler
    {
        public static void LockCursor()
        {
            // //Setup cursor when using camera
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public static void FreeCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}