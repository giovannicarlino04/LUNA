using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Windows.Forms;
using OpenTK;

namespace LUNA
{
    [Serializable]
    public class Scene
    {
        public Camera Camera { get; set; }
        public List<SceneObject> Objects { get; set; }
    }

    [Serializable]
    public class SceneObject
    {
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }
    }

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

        public Form1()
        {
            InitializeComponent();
            InitializeOpenGLControl();
            InitializeRenderTimer();
            InitializeCamera();
            ConfigureForm();
            this.KeyDown += OnKeyDown;

            // Initialize the debug console
            debugConsole = new DebugConsoleForm();
            debugConsole.Show();
        }

        private void ConfigureForm()
        {
            // Form configuration code here (if any)
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
            camera = new Camera(new Vector3(0.0f, 0.0f, 3.0f));
        }

        private void RenderTimer_Tick(object sender, EventArgs e)
        {
            // Trigger the rendering of the OpenGL scene
            openGLControl.Render(camera);
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

        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveScene = new SaveFileDialog();
            saveScene.Filter = "Luna Scene File | *.lns";

            if (saveScene.ShowDialog() == DialogResult.OK)
            {
                // Create a scene object
                Scene scene = new Scene
                {
                    Camera = camera,
                    Objects = new List<SceneObject>() // Populate with your scene objects
                };

                // Choose between Binary or JSON serialization
                SaveSceneAsJson(saveScene.FileName, scene);
                // SaveScene(saveScene.FileName, scene); // Use binary serialization instead
            }
        }

        private void openSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openScene = new OpenFileDialog();
            openScene.Filter = "Luna Scene File | *.lns";

            if (openScene.ShowDialog() == DialogResult.OK)
            {
                // Load the scene
                Scene scene = LoadSceneFromJson(openScene.FileName);
                // Scene scene = LoadScene(openScene.FileName); // Use binary deserialization instead

                // Restore the camera and objects
                camera = scene.Camera;
                // Populate your scene objects in the OpenGLControl
                // ...
            }
        }

        private void SaveScene(string filePath, Scene scene)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, scene);
            }
        }

        private Scene LoadScene(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Scene)formatter.Deserialize(fs);
            }
        }

        private void SaveSceneAsJson(string filePath, Scene scene)
        {
            string jsonString = JsonSerializer.Serialize(scene);
            File.WriteAllText(filePath, jsonString);
        }

        private Scene LoadSceneFromJson(string filePath)
        {
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<Scene>(jsonString);
        }
    }
}
