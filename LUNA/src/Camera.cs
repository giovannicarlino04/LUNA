using OpenTK;
using OpenTK.Input;
using System;

namespace LUNA
{
    public class Camera
    {
        public Vector3 Position { get; private set; }
        public Vector3 Front { get; private set; }
        public Vector3 Up { get; private set; }
        public Vector3 Right { get; private set; }
        public float Yaw { get; private set; }
        public float Pitch { get; private set; }
        public float MovementSpeed { get; set; }
        public float MouseSensitivity { get; set; }

        private float zoom;

        public Camera(Vector3 position)
        {
            Position = position;
            Front = new Vector3(0.0f, 0.0f, -1.0f);
            Up = new Vector3(0.0f, 1.0f, 0.0f);
            Right = Vector3.Cross(Front, Up);
            Yaw = -90.0f;
            Pitch = 0.0f;
            MovementSpeed = 2.5f;
            MouseSensitivity = 0.1f;
            zoom = 45.0f;
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Position, Position + Front, Up);
        }

        public void ProcessKeyboard(string direction, float deltaTime)
        {
            float velocity = MovementSpeed * deltaTime;
            if (direction == "FORWARD")
                Position += Front * velocity;
            if (direction == "BACKWARD")
                Position -= Front * velocity;
            if (direction == "LEFT")
                Position -= Right * velocity;
            if (direction == "RIGHT")
                Position += Right * velocity;
            if (direction == "SPACE")
                Position += Up * velocity;
            if (direction == "SHIFT")
                Position -= Up * velocity;
        }

        public void ProcessMouseMovement(float xOffset, float yOffset)
        {
            float sensitivity = 0.1f; // Adjust sensitivity as needed
            xOffset *= sensitivity;
            yOffset *= sensitivity;

            // Update the camera's orientation based on the mouse movement
            Yaw += xOffset;
            Pitch -= yOffset;

            // Clamp the pitch value to prevent flipping
            if (Pitch > 89.0f)
                Pitch = 89.0f;
            if (Pitch < -89.0f)
                Pitch = -89.0f;

            // Recalculate front vector
            Vector3 front;
            front.X = (float)Math.Cos(MathHelper.DegreesToRadians(Yaw)) * (float)Math.Cos(MathHelper.DegreesToRadians(Pitch));
            front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(Pitch));
            front.Z = (float)Math.Sin(MathHelper.DegreesToRadians(Yaw)) * (float)Math.Cos(MathHelper.DegreesToRadians(Pitch));
            Front = Vector3.Normalize(front);
        }


        private void UpdateCameraVectors()
        {
            Vector3 front;
            front.X = (float)Math.Cos(MathHelper.DegreesToRadians(Yaw)) * (float)Math.Cos(MathHelper.DegreesToRadians(Pitch));
            front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(Pitch));
            front.Z = (float)Math.Sin(MathHelper.DegreesToRadians(Yaw)) * (float)Math.Cos(MathHelper.DegreesToRadians(Pitch));
            Front = Vector3.Normalize(front);
            Right = Vector3.Normalize(Vector3.Cross(Front, Up));
            Up = Vector3.Normalize(Vector3.Cross(Right, Front));
        }

        public Matrix4 GetProjectionMatrix(float aspectRatio)
        {
            return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(zoom), aspectRatio, 0.1f, 100.0f);
        }
    }
}
