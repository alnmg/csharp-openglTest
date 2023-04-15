using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace openglTest
{
    public class Window : GameWindow
    {

        private int VBO;
        private int shaderProgram;
        private int VAO;

        public Window(int width, int height, string title) : base(new GameWindowSettings()
        {
            IsMultiThreaded = true,
            RenderFrequency = 60
        },
        new NativeWindowSettings()
        {
            StartVisible = false,
            Size = new Vector2i(width, height),
            Title = title,
            API = ContextAPI.OpenGL,
            Profile = ContextProfile.Any,
            APIVersion = new Version(3, 1)
        })
        {
            this.CenterWindow();
        }

        protected override void OnLoad()
        {
            IsVisible = true;

            GL.ClearColor(0.2f, 0.3f, 0.4f, 1.0f);

            float[] triangle = {
                0.0f, 0.5f, 0.0f,   1f, 0f, 0f, 1f,
                0.5f, -0.5f, 0.0f,  0f, 1f, 0f, 1f,
                -0.5f, -0.5f, 0.0f, 0f, 0f, 1f, 1f
            };

            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, triangle.Length * sizeof(float), triangle, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 7 * sizeof(float), 0);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 7 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            GL.BindVertexArray(0);

            string vertexShaderCode =
                @"
                #version 330 core

                layout (location = 0) in vec3 apos;
                layout (location = 1) in vec4 acolor;

                out vec4 vcolor;

                void main()
                {
                    vcolor = acolor;
                    gl_Position = vec4(apos, 1);
                }   
                ";

            string fragmentShaderCode =
                @"
                #version 330 core
                
                in vec4 vcolor;

                out vec4 fragColor;

                void main()
                {
                    fragColor = vcolor;
                }   
                ";

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderCode);
            GL.CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderCode);
            GL.CompileShader(fragmentShader);

            shaderProgram = GL.CreateProgram();

            GL.AttachShader(shaderProgram, vertexShader);
            GL.AttachShader(shaderProgram, fragmentShader);
            GL.LinkProgram(shaderProgram);

            GL.DetachShader(shaderProgram, vertexShader);
            GL.DetachShader(shaderProgram, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            

            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.UseProgram(shaderProgram);
            GL.BindVertexArray(VAO);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            SwapBuffers();

            base.OnRenderFrame(args);
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            KeyboardState input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

    }
}
