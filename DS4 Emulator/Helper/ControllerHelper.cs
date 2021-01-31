using System.Threading;
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.DualShock4;
using SharpDX.XInput;

namespace Helper
{
    public class ControllerHelper
    {
        private Controller controller;
        private ViGEmClient client;
        private IDualShock4Controller xinput;
        private Timer timer;

        public ControllerHelper()
        {
            controller = new Controller(UserIndex.One);
            client = new ViGEmClient();
            xinput = client.CreateDualShock4Controller();
            xinput.Connect();
            timer = new Timer(obj => Update());
        }

        public void Start()
        {
            timer.Change(0, 1000 / 25);
        }

        private void Update()
        {
            controller.GetState(out State state);
            Buttons(state);
            AnalogSticks(state);
        }

        private void Buttons(State state)
        {
            A(state);
            B(state);
            Y(state);
            X(state);
            LB(state);
            RB(state);
            DPad(state);
            LT(state);
            RT(state);
            RS(state);
            LS(state);
            Start(state);
            Back(state);
            Home(state);
        }
        #region Buttons

        #region Color
        private void A(State state)
        {
            var isDown = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.A);
            xinput.SetButtonState(DualShock4Button.Cross, isDown);
        }

        private void B(State state)
        {
            var isDown = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.B);
            xinput.SetButtonState(DualShock4Button.Circle, isDown);
        }

        private void Y(State state)
        {
            var isDown = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.Y);
            xinput.SetButtonState(DualShock4Button.Triangle, isDown);
        }

        private void X(State state)
        {
            var isDown = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.X);
            xinput.SetButtonState(DualShock4Button.Square, isDown);
        }
        #endregion

        #region Special
        private void Back(State state)
        {
            var isDown = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.Back);
            xinput.SetButtonState(DualShock4SpecialButton.Touchpad, isDown);
        }
        private void Start(State state)
        {
            var isDown = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.Start);
            xinput.SetButtonState(DualShock4Button.Options, isDown);
        }
        private void Home(State state) {
            //var isDown = state.Gamepad.Buttons.HasFlag((GamepadButtonFlags)0x0400); // Does not work. Maybe not mappable? (waiting implementation)
            var isDown = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.Start) && state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.Back);
            xinput.SetButtonState(DualShock4SpecialButton.Ps, isDown);
        }
        #endregion

        #region Directional Pad
        private void Left(State state)
        {
            var isDown = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadLeft);
            xinput.SetDPadDirection(DualShock4DPadDirection.East);
        }

        private void Right(State state)
        {
            var isDown = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadRight);
            xinput.SetDPadDirection(DualShock4DPadDirection.West);
        }

        private void Up(State state)
        {
            var isDown = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadUp);
            xinput.SetDPadDirection(DualShock4DPadDirection.North);
        }

        private void Down(State state)
        {
            var isDown = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadDown);
            xinput.SetDPadDirection(DualShock4DPadDirection.South);
        }

        private void DPad(State state)
        {
            bool up = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadUp),
                down = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadDown),
                left = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadLeft),
                right = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadRight);

            if (up && right)
                xinput.SetDPadDirection(DualShock4DPadDirection.Northeast);
            else if (up && left)
                xinput.SetDPadDirection(DualShock4DPadDirection.Northwest);
            else if (down && right)
                xinput.SetDPadDirection(DualShock4DPadDirection.Southeast);
            else if (down && left)
                xinput.SetDPadDirection(DualShock4DPadDirection.Southwest);
            else if (up)
                xinput.SetDPadDirection(DualShock4DPadDirection.North);
            else if (down)
                xinput.SetDPadDirection(DualShock4DPadDirection.South);
            else if (left)
                xinput.SetDPadDirection(DualShock4DPadDirection.West);
            else if (right)
                xinput.SetDPadDirection(DualShock4DPadDirection.East);
            else
                xinput.SetDPadDirection(DualShock4DPadDirection.None);
        }
        #endregion

        #region Shoulders
        private void RB(State state)
        {
            var isDown = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.RightShoulder);
            xinput.SetButtonState(DualShock4Button.ShoulderRight, isDown);
        }

        private void LB(State state)
        {
            var isDown = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.LeftShoulder);
            xinput.SetButtonState(DualShock4Button.ShoulderLeft, isDown);
        }
        #endregion

        #region Triggers
        private void RT(State state)
        {
            var value = state.Gamepad.RightTrigger;
            xinput.SetSliderValue(DualShock4Slider.RightTrigger, value);
            xinput.SetButtonState(DualShock4Button.TriggerRight, true);
        }

        private void LT(State state)
        {
            var value = state.Gamepad.LeftTrigger;
            xinput.SetSliderValue(DualShock4Slider.LeftTrigger, value);
            xinput.SetButtonState(DualShock4Button.TriggerLeft, true);
        }
        #endregion

        #region Thumbs
        private void LS(State state)
        {
            var isDown = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.LeftThumb);
            xinput.SetButtonState(DualShock4Button.ThumbLeft, isDown);
        }

        private void RS(State state)
        {
            var isDown = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.RightThumb);
            xinput.SetButtonState(DualShock4Button.ThumbRight, isDown);
        }
        #endregion

        #endregion

        private void AnalogSticks(State state)
        {
            LeftAnalogStick(state);
            RightAnalogStick(state);
        }
        #region Analog Sticks

        private void LeftAnalogStick(State state)
        {
            short x = state.Gamepad.LeftThumbX;
            short y = state.Gamepad.LeftThumbY;
            xinput.SetAxisValue(0, x);
            xinput.SetAxisValue(1, (short)(-y - 1));
        }

        private void RightAnalogStick(State state)
        {
            short x = state.Gamepad.RightThumbX;
            short y = state.Gamepad.RightThumbY;
            xinput.SetAxisValue(2, x);
            xinput.SetAxisValue(3, (short)(-y - 1));
        }
        #endregion
    }
}
