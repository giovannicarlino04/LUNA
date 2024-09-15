using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;
using System;
using System.Windows.Forms;

namespace LUNA
{
    public class OpenGLControl : PictureBox
    {
        private IWindowInfo windowInfo;
        private GraphicsContext context;
        private bool isInitialized = false;

        public void Render(Camera camera)
        {
            if (!isInitialized)
                return;

            context.MakeCurrent(windowInfo);

            GL.Viewport(0, 0, this.Width, this.Height);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 projection = camera.GetProjectionMatrix(Width / (float)Height);
            Matrix4 view = camera.GetViewMatrix();

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref view);

            DrawCube();

            context.SwapBuffers();
        }

        private void DrawCube()
        {
            GL.Begin(PrimitiveType.Quads);

            // Front face (red)
            GL.Color3(System.Drawing.Color.Red);
            GL.Vertex3(-0.5f, -0.5f, 0.5f);
            GL.Vertex3(0.5f, -0.5f, 0.5f);
            GL.Vertex3(0.5f, 0.5f, 0.5f);
            GL.Vertex3(-0.5f, 0.5f, 0.5f);

            // Back face (green)
            GL.Color3(System.Drawing.Color.Green);
            GL.Vertex3(-0.5f, -0.5f, -0.5f);
            GL.Vertex3(0.5f, -0.5f, -0.5f);
            GL.Vertex3(0.5f, 0.5f, -0.5f);
            GL.Vertex3(-0.5f, 0.5f, -0.5f);

            // Left face (blue)
            GL.Color3(System.Drawing.Color.Blue);
            GL.Vertex3(-0.5f, -0.5f, -0.5f);
            GL.Vertex3(-0.5f, -0.5f, 0.5f);
            GL.Vertex3(-0.5f, 0.5f, 0.5f);
            GL.Vertex3(-0.5f, 0.5f, -0.5f);

            // Right face (yellow)
            GL.Color3(System.Drawing.Color.Yellow);
            GL.Vertex3(0.5f, -0.5f, -0.5f);
            GL.Vertex3(0.5f, -0.5f, 0.5f);
            GL.Vertex3(0.5f, 0.5f, 0.5f);
            GL.Vertex3(0.5f, 0.5f, -0.5f);

            // Top face (cyan)
            GL.Color3(System.Drawing.Color.Cyan);
            GL.Vertex3(-0.5f, 0.5f, -0.5f);
            GL.Vertex3(0.5f, 0.5f, -0.5f);
            GL.Vertex3(0.5f, 0.5f, 0.5f);
            GL.Vertex3(-0.5f, 0.5f, 0.5f);

            // Bottom face (magenta)
            GL.Color3(System.Drawing.Color.Magenta);
            GL.Vertex3(-0.5f, -0.5f, -0.5f);
            GL.Vertex3(0.5f, -0.5f, -0.5f);
            GL.Vertex3(0.5f, -0.5f, 0.5f);
            GL.Vertex3(-0.5f, -0.5f, 0.5f);

            GL.End();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            windowInfo = OpenTK.Platform.Utilities.CreateWindowsWindowInfo(this.Handle);
            context = new GraphicsContext(GraphicsMode.Default, windowInfo);
            context.MakeCurrent(windowInfo);
            context.LoadAll();
            GL.ClearColor(System.Drawing.Color.CornflowerBlue);
            GL.Enable(EnableCap.DepthTest);  // Enable depth testing for 3D rendering
            isInitialized = true;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            // Prevent flickering; rendering is controlled by the Timer
        }
    }
}