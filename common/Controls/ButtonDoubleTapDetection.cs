namespace GBJAM.Commons.Controls
{
    public class ButtonDoubleTapDetection
    {
        public float delay;

        public bool isDoubleTap;

        private float notPressedTime;
        private bool wasPressed;
            
        public void Track(bool pressed, float dt)
        {
            isDoubleTap = false;
                
            if (pressed)
            {
                if (!wasPressed)
                {
                    isDoubleTap = notPressedTime < delay;
                }
            }
            else
            {
                if (wasPressed)
                {
                    notPressedTime = 0;
                }

                notPressedTime += dt;
            }

            wasPressed = pressed;
        }
    }
}