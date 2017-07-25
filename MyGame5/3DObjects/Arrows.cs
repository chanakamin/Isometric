using System;
using System.Collections.Generic;

using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;

namespace Isometric._3DObjects
{
    using SharpDX.Toolkit.Graphics;
    public class Arrows : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        private Model model;
        private Matrix world;
        private Matrix view;
        private Matrix projection;
        private MouseManager mouseManager;
        private MouseState mouseState;
        MouseState prev;
        public Arrows()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            graphicsDeviceManager.PreferredGraphicsProfile = new FeatureLevel[] { FeatureLevel.Level_11_1, };
          //  graphicsDeviceManager.DepthBufferShaderResource = true;
            Content.RootDirectory = "Content";
            mouseManager = new MouseManager(this);
        }

        protected override void LoadContent()
        {
            //קובץ שעולה צבעוני
            // model = Content.Load<Model>("Ship");
            //קובץ שקוף שעולה בלי צבע
            model = Content.Load<Model>("Rubik's Cube");
           

            BasicEffect.EnableDefaultLighting(model, true);
            //view = Matrix.LookAtRH(new Vector3(0, 0, 1), new Vector3(0, 0, 0), Vector3.UnitY);
            //projection = Matrix.PerspectiveFovRH((float)Math.PI / 4.0f, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);
           // world = Matrix.Identity;

            base.LoadContent();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }
        BasicEffect basicEffect;
        protected override void Update(GameTime gameTime)
        {
            //var time = (float)gameTime.TotalGameTime.TotalSeconds;
            //world = Matrix.RotationX(time) * Matrix.RotationY(time * 2.0f) * Matrix.RotationZ(time * .7f);
            //projection = Matrix.PerspectiveFovRH((float)Math.PI / 4.0f, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);

            // TODO: Add your update logic here
            //// Rotate the cube.
            //var time = (float)gameTime.TotalGameTime.TotalSeconds;
            //quadEffect.World = Matrix.RotationX(gameTime) * Matrix.RotationY(gameTime * 2.0f) * Matrix.RotationZ(gameTime * .7f);
            //quadEffect.Projection = Matrix.PerspectiveFovRH((float)Math.PI / 4.0f, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);

            ////mouseState = mouseManager.GetState();
            ////if (mouseState.LeftButton.Down)
            ////{
            ////    if (!mouseState.LeftButton.Pressed)
            ////    {
            ////        var time = (float)gameTime.TotalGameTime.TotalSeconds;
            ////        world *= Matrix.RotationX((mouseState.Y - prev.Y) * 3) * Matrix.RotationY((mouseState.X - prev.X) * 3);//* Matrix.RotationZ(mouseState.Z - prev.Z);
            ////        Global.World = world;
            ////    }
            ////    //  quadEffect.Projection = Matrix.PerspectiveFovRH((float)Math.PI / 4.0f, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);
            ////    prev = mouseState;
            ////}
            ////else
            ////    world = Global.World;
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.EnableDefaultLighting();
            basicEffect.View = Matrix.LookAtRH(new Vector3(0, 0, 35), new Vector3(0, 0, 0), Vector3.UnitY);
            basicEffect.Projection = Matrix.PerspectiveFovRH((float)Math.PI / 4.0f, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, 50.0f);
            basicEffect.World = Matrix.Identity;

            basicEffect.World = world;
            basicEffect.View = view;

            basicEffect.LightingEnabled = true;
            basicEffect.DiffuseColor = new Vector4(1.0f, 1.0f, 1.0f,0);
            basicEffect.SpecularColor = new Vector3(0.2f, 0.2f, 0.2f);
            basicEffect.SpecularPower = 5.0f;
            basicEffect.AmbientLightColor =
              new Vector3(0.5f, 0.5f, 0.5f);

            basicEffect.DirectionalLight0.Enabled = true;
            basicEffect.DirectionalLight0.DiffuseColor = Vector3.One;
            basicEffect.DirectionalLight0.Direction =
                      Vector3.Normalize(new Vector3(1.0f, 1.0f, -1.0f));
            basicEffect.DirectionalLight0.SpecularColor = Vector3.One;
            basicEffect.DirectionalLight1.Enabled = true;
            basicEffect.DirectionalLight1.DiffuseColor =
                      new Vector3(0.5f, 0.5f, 0.5f);
            basicEffect.DirectionalLight1.Direction =
                     Vector3.Normalize(new Vector3(-1.0f, -1.0f, 1.0f));
            basicEffect.DirectionalLight1.SpecularColor =
                      new Vector3(0.5f, 0.5f, 0.5f);
            //view = Matrix.LookAtRH(new Vector3(0, 0, 2), 
            view = Matrix.LookAtRH(new Vector3(0.0f, 0.0f, 7.0f), new Vector3(0, 0.0f, 0), Vector3.UnitY);
            projection = Matrix.PerspectiveFovRH(0.9f, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            var time = (float)gameTime.TotalGameTime.TotalSeconds;

            // Clears the screen with the Color.CornflowerBlue
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Constant used to translate 3d models
            float translateX = 0.0f;

            // ------------------------------------------------------------------------
            // Draw the 3d model
            // ------------------------------------------------------------------------
            var world = Matrix.Scaling(0.003f) *
                        Matrix.RotationY(time) *
                        Matrix.Translation(0, -1.5f, 2.0f);
       

            translateX += 3.5f;
            foreach (ModelMesh meshes in model.Meshes)
            {
                foreach (ModelMeshPart parts in meshes.MeshParts)
                    parts.Effect = basicEffect;
               
            }
            model.Draw(GraphicsDevice, world, view, projection);
            base.Draw(gameTime);
            //graphicsDeviceManager.PreferredGraphicsProfile = new FeatureLevel[] { FeatureLevel.Level_11_0, };
            //graphicsDeviceManager.DepthBufferShaderResource = true;

            //var blendStateDescription = new SharpDX.Direct3D11.BlendStateDescription();

            //blendStateDescription.AlphaToCoverageEnable = false;

            //blendStateDescription.RenderTarget[0].IsBlendEnabled = true;
            //blendStateDescription.RenderTarget[0].SourceBlend = SharpDX.Direct3D11.BlendOption.SourceAlpha;
            //blendStateDescription.RenderTarget[0].DestinationBlend = SharpDX.Direct3D11.BlendOption.InverseSourceAlpha;
            //blendStateDescription.RenderTarget[0].BlendOperation = SharpDX.Direct3D11.BlendOperation.Add;
            //blendStateDescription.RenderTarget[0].SourceAlphaBlend = SharpDX.Direct3D11.BlendOption.Zero;
            //blendStateDescription.RenderTarget[0].DestinationAlphaBlend = SharpDX.Direct3D11.BlendOption.Zero;
            //blendStateDescription.RenderTarget[0].AlphaBlendOperation = SharpDX.Direct3D11.BlendOperation.Add;
            //blendStateDescription.RenderTarget[0].RenderTargetWriteMask = SharpDX.Direct3D11.ColorWriteMaskFlags.All;

            //var blendState = SharpDX.Toolkit.Graphics.BlendState.New(this.GraphicsDevice, blendStateDescription);
            //this.GraphicsDevice.SetBlendState(blendState);
            //var depthDisabledStencilDesc = new DepthStencilStateDescription()
            //{
            //    IsDepthEnabled = false,
            //    DepthWriteMask = DepthWriteMask.All,
            //    DepthComparison = Comparison.Less,
            //    IsStencilEnabled = true,
            //    StencilReadMask = 0xFF,
            //    StencilWriteMask = 0xFF,
            //    // Stencil operation if pixel front-facing.
            //    FrontFace = new DepthStencilOperationDescription()
            //    {
            //        FailOperation = StencilOperation.Keep,
            //        DepthFailOperation = StencilOperation.Increment,
            //        PassOperation = StencilOperation.Keep,
            //        Comparison = Comparison.Always
            //    },
            //    // Stencil operation if pixel is back-facing.
            //    BackFace = new DepthStencilOperationDescription()
            //    {
            //        FailOperation = StencilOperation.Keep,
            //        DepthFailOperation = StencilOperation.Decrement,
            //        PassOperation = StencilOperation.Keep,
            //        Comparison = Comparison.Always
            //    }
            //};

            //// Create the depth stencil state.
            //var depthDisabledStencilState = SharpDX.Toolkit.Graphics.DepthStencilState.New(this.GraphicsDevice, depthDisabledStencilDesc);
            ////turn z-buffer off
            //this.GraphicsDevice.SetDepthStencilState(depthDisabledStencilState, 1);


            //    var blendStateDescription = new SharpDX.Direct3D11.BlendStateDescription();
            //    blendStateDescription.AlphaToCoverageEnable = false;

            //    blendStateDescription.RenderTarget[0].IsBlendEnabled = true;
            //    blendStateDescription.RenderTarget[0].SourceBlend = SharpDX.Direct3D11.BlendOption.SourceAlpha;
            //    blendStateDescription.RenderTarget[0].DestinationBlend = SharpDX.Direct3D11.BlendOption.InverseSourceAlpha;
            //    blendStateDescription.RenderTarget[0].BlendOperation = SharpDX.Direct3D11.BlendOperation.Add;
            //    blendStateDescription.RenderTarget[0].SourceAlphaBlend = SharpDX.Direct3D11.BlendOption.Zero;
            //    blendStateDescription.RenderTarget[0].DestinationAlphaBlend = SharpDX.Direct3D11.BlendOption.Zero;
            //    blendStateDescription.RenderTarget[0].AlphaBlendOperation = SharpDX.Direct3D11.BlendOperation.Add;
            //    blendStateDescription.RenderTarget[0].RenderTargetWriteMask = SharpDX.Direct3D11.ColorWriteMaskFlags.All;

            //    var blendState = SharpDX.Toolkit.Graphics.BlendState.New(this.GraphicsDevice, blendStateDescription);
            //    this.GraphicsDevice.SetBlendState(blendState);
            //    var depthDisabledStencilDesc = new SharpDX.Direct3D11.DepthStencilStateDescription()
            //{
            //    IsDepthEnabled = false,
            //    DepthWriteMask = SharpDX.Direct3D11.DepthWriteMask.All,
            //    DepthComparison = SharpDX.Direct3D11.Comparison.Less,
            //    IsStencilEnabled = true,
            //    StencilReadMask = 0xFF,
            //    StencilWriteMask = 0xFF,
            //    // Stencil operation if pixel front-facing.
            //    FrontFace = new SharpDX.Direct3D11.DepthStencilOperationDescription()
            //    {
            //        FailOperation = SharpDX.Direct3D11.StencilOperation.Keep,
            //        DepthFailOperation = SharpDX.Direct3D11.StencilOperation.Increment,
            //        PassOperation = SharpDX.Direct3D11.StencilOperation.Keep,
            //        Comparison = SharpDX.Direct3D11.Comparison.Always
            //    },
            //    // Stencil operation if pixel is back-facing.
            //    BackFace = new SharpDX.Direct3D11.DepthStencilOperationDescription()
            //    {
            //        FailOperation = SharpDX.Direct3D11.StencilOperation.Keep,
            //        DepthFailOperation = SharpDX.Direct3D11.StencilOperation.Decrement,
            //        PassOperation = SharpDX.Direct3D11.StencilOperation.Keep,
            //        Comparison = SharpDX.Direct3D11.Comparison.Always
            //    }
            //};

            //    // Create the depth stencil state.
            //    var depthDisabledStencilState = SharpDX.Toolkit.Graphics.DepthStencilState.New(this.GraphicsDevice, depthDisabledStencilDesc);
            //    //turn z-buffer off
            //    this.GraphicsDevice.SetDepthStencilState(depthDisabledStencilState, 1);
            //   //basicEffect.Alpha = 2f;



            //GraphicsDevice.Clear(Color.FromAbgr(4283585279));

            //model.Draw(GraphicsDevice, world, view, projection);
        }
    }
}
