using System;
using System.Drawing;
using System.Windows.Forms;

namespace LUNA
{
    public partial class Form1 : Form
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SetCapture(IntPtr hWnd);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ReleaseCapture();


        private Camera camera;
        private OpenGLControl openGLControl;
        private Timer renderTimer;
        private DebugConsoleForm debugConsole;
        private bool isMouseCaptured;
        public Form1()
        {
            InitializeComponent();
            InitializeOpenGLControl();
            InitializeRenderTimer();
            InitializeCamera();
            ConfigureForm();
            this.MouseMove += OnMouseMove;
            this.KeyDown += OnKeyDown;
            this.MouseDown += OnMouseDown;
            this.MouseUp += OnMouseUp;

            // Initialize the debug console
            debugConsole = new DebugConsoleForm();
            debugConsole.Show();
        }

        private void ConfigureForm()
        {

        }

        private void InitializeOpenGLControl()
        {
            // Initialize the OpenGL control
            openGLControl = new OpenGLControl();
            openGLControl.Dock = DockStyle.Fill;  // Makes the OpenGL control fill the entire form
            this.Controls.Add(openGLControl);     // Add the OpenGL control to the form
        }

        private void InitializeRenderTimer()
        {
            // Create a timer to control rendering at a fixed interval (e.g., 60 FPS)
            renderTimer = new Timer();
            renderTimer.Interval = 16; // Approximately 60 FPS (1000ms / 60 = ~16ms)
            renderTimer.Tick += RenderTimer_Tick;
            renderTimer.Start();
        }

        private void InitializeCamera()
        {
            // Initialize the camera
            camera = new Camera(new OpenTK.Vector3(0.0f, 0.0f, 3.0f));
        }

        private void RenderTimer_Tick(object sender, EventArgs e)
        {
            // Trigger the rendering of the OpenGL scene
            openGLControl.Render(camera);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!isMouseCaptured) return;

            // Calculate offsets
            float xOffset = e.X - (Width / 2.0f);
            float yOffset = (Height / 2.0f) - e.Y;

            // Process mouse movement
            camera.ProcessMouseMovement(xOffset, yOffset);

            // Recenter the mouse cursor
            Cursor.Position = this.PointToScreen(new System.Drawing.Point(Width / 2, Height / 2));

            // Debug information
            debugConsole.WriteLine($"Mouse moved: X={e.X}, Y={e.Y}");
        }
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Cursor.Hide(); // Hide the cursor
                isMouseCaptured = true;

                // Capture mouse input
                SetCapture(this.Handle);

                // Recenter the cursor
                Cursor.Position = this.PointToScreen(new System.Drawing.Point(Width / 2, Height / 2));
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Cursor.Show(); // Show the cursor
                isMouseCaptured = false;

                // Release mouse input capture
                ReleaseCapture();
            }
        }





        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            // Implement camera movement based on key presses
            float deltaTime = 0.1f; // You may want to adjust this value based on frame rate or use a time delta

            if (e.KeyCode == Keys.W)
                camera.ProcessKeyboard("FORWARD", deltaTime);
            if (e.KeyCode == Keys.S)
                camera.ProcessKeyboard("BACKWARD", deltaTime);
            if (e.KeyCode == Keys.A)
                camera.ProcessKeyboard("LEFT", deltaTime);
            if (e.KeyCode == Keys.D)
                camera.ProcessKeyboard("RIGHT", deltaTime);

            if (e.KeyCode == Keys.Space)
                camera.ProcessKeyboard("SPACE", deltaTime);
            if (e.KeyCode == Keys.ShiftKey)
                camera.ProcessKeyboard("SHIFT", deltaTime);

            // Write debug info
            debugConsole.WriteLine($"Key down: {e.KeyCode}");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
