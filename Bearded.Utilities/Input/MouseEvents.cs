using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Bearded.Utilities.Input
{
    sealed class MouseEvents
    {
        private float scrollSinceLastFrame;

        internal float DeltaScrollF { get; private set; }

        internal MouseEvents(NativeWindow nativeWindow)
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
