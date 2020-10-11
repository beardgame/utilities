using OpenToolkit.Windowing.Common;

namespace Bearded.Utilities.Input
{
    sealed class MouseEvents
    {
        private float scrollSinceLastFrame;

        internal float DeltaScrollF { get; private set; }

        internal MouseEvents(INativeWindow nativeWindow)
        {
            nativeWindow.MouseWheel += onMouseWheel;
        }

        private void onMouseWheel(MouseWheelEventArgs e)
        {
            scrollSinceLastFrame += e.OffsetY;
        }

        public void Update()
        {
            DeltaScrollF = scrollSinceLastFrame;
            scrollSinceLastFrame = 0;
        }
    }
}
