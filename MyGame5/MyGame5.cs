//#region File Description
////-----------------------------------------------------------------------------
//// Game1.cs
////
//// Microsoft XNA Community Game Platform
//// Copyright (C) Microsoft Corporation. All rights reserved.
////-----------------------------------------------------------------------------
//#endregion

//using System;
//using System.Collections.Generic;
//using System.Linq;

//using System;
//using System.Diagnostics;

//using SharpDX;
//using SharpDX.Direct3D11;
//using SharpDX.Toolkit;


//namespace Isometric
//{
//    // Use this namespace here in case we need to use Direct3D11 namespace as well, as this
//    // namespace will override the Direct3D11.
//    using SharpDX.Toolkit.Graphics;

//    using SharpDX.Toolkit.Content;
//    /// <summary>
//    /// This is the main type for your game
//    /// </summary>
//    public class MyGame5 : Game
//    {
//        GraphicsDeviceManager graphics;
//        SpriteBatch spriteBatch;
//       // private GraphicsDeviceManager graphicsDeviceManager;
//        private BasicEffect basicEffect;
//        private Buffer<VertexPositionNormalTexture> vertices;
//        private VertexInputLayout inputLayout;
//        private List<VertexPositionNormalTexture> listVertexPositionColor = new List<VertexPositionNormalTexture>();



//        private GraphicsDeviceManager graphicsDeviceManager;
//        private Model model;
//        private Matrix world;
//        private Matrix view;
//        private Matrix projection;
//        public MyGame5()
//        {
//            graphics = new GraphicsDeviceManager(this);
//            Content.RootDirectory = "Content";
//        }

//        Quad quad;
//        //VertexDeclaration vertexDeclaration;
//        Matrix View, Projection;
//        protected override void Initialize()
//        {
//            quad = new Quad(Vector3.Zero, Vector3.BackwardLH, Vector3.Up, 1, 1);
//            //View = Matrix.CreateLookAt(new Vector3(0, 0, 2), Vector3.Zero,
//            //    Vector3.Up);
//            //Projection = Matrix.CreatePerspectiveFieldOfView(
//            //    MathHelper.PiOver4, 4.0f / 3.0f, 1, 500);
//            View = Matrix.LookAtRH(new Vector3(0.0f, 0.0f, 7.0f), new Vector3(0, 0.0f, 0), Vector3.UnitY);
//            Projection = Matrix.PerspectiveFovRH(0.9f, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);
            
//            base.Initialize();
//        }

//        /// <summary>
//        /// LoadContent will be called once per game and is the place to load
//        /// all of your content.
//        /// </summary>
//        Texture2D texture;
//        BasicEffect quadEffect;
//        protected override void LoadContent()
//        {

//            // Create a new SpriteBatch, which can be used to draw textures.
//            spriteBatch = new SpriteBatch(GraphicsDevice);
//            //  var x = Content.Load<Effect>("file");
//            texture = Content.Load<Texture2D>("Glass");
//            quadEffect = new BasicEffect(graphics.GraphicsDevice);
//            quadEffect.EnableDefaultLighting();

//            quadEffect.World = Matrix.Identity;
//            quadEffect.View = View;
//            quadEffect.Projection = Projection;
//            quadEffect.TextureEnabled = true;
//            //בלי הטקסטורה
//           // quadEffect.TextureEnabled = false;
//            quadEffect.Texture = texture;
//            //מועתק מהגאומטריק מהזה?
//            quadEffect.PreferPerPixelLighting = true;



//            model = Content.Load<Model>("Cube");
//            BasicEffect.EnableDefaultLighting(model, true);
//            //  BasicEffect.EnableDefaultLighting();
//            // Instantiate a SpriteBatch
//            //  spriteBatch = ToDisposeContent(new SpriteBatch(GraphicsDevice));

//            view = Matrix.LookAtRH(new Vector3(0, 0, 20), new Vector3(0, 0, 0), Vector3.UnitY);
//            projection = Matrix.PerspectiveFovRH((float)Math.PI / 4.0f, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);
//            world = Matrix.Identity;
//            //basicEffect = ToDisposeContent(new BasicEffect(GraphicsDevice)
//            //{
//            //    VertexColorEnabled = true,
//            //    View = Matrix.LookAtRH(new Vector3(0, 0, 50), new Vector3(0, 0, 0), Vector3.UnitY),
//            //    Projection = Matrix.PerspectiveFovRH((float)Math.PI / 4.0f, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f),
//            //    World = Matrix.Identity
//            //});
//            //VertexElement.
//            //vertexDeclaration = new VertexDeclaration(new VertexElement[]
//            //    {
//            //        new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
//            //        new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
//            //        new VertexElement(24, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
//            //    }
//            //);

//        //    var blendStateDescription = new BlendStateDescription();

//        //    blendStateDescription.AlphaToCoverageEnable = false;

//        //    blendStateDescription.RenderTarget[0].IsBlendEnabled = true;
//        //    blendStateDescription.RenderTarget[0].SourceBlend = BlendOption.SourceAlpha;
//        //    blendStateDescription.RenderTarget[0].DestinationBlend = BlendOption.InverseSourceAlpha;
//        //    blendStateDescription.RenderTarget[0].BlendOperation = BlendOperation.Add;
//        //    blendStateDescription.RenderTarget[0].SourceAlphaBlend = BlendOption.Zero;
//        //    blendStateDescription.RenderTarget[0].DestinationAlphaBlend = BlendOption.Zero;
//        //    blendStateDescription.RenderTarget[0].AlphaBlendOperation = BlendOperation.Add;
//        //    blendStateDescription.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;

//        //    var blendState = SharpDX.Toolkit.Graphics.BlendState.New(this.GraphicsDevice, blendStateDescription);
//        //    this.GraphicsDevice.SetBlendState(blendState);
//        //    var depthDisabledStencilDesc = new DepthStencilStateDescription()
//        //{
//        //    IsDepthEnabled = false,
//        //    DepthWriteMask = DepthWriteMask.All,
//        //    DepthComparison = Comparison.Less,
//        //    IsStencilEnabled = true,
//        //    StencilReadMask = 0xFF,
//        //    StencilWriteMask = 0xFF,
//        //    // Stencil operation if pixel front-facing.
//        //    FrontFace = new DepthStencilOperationDescription()
//        //    {
//        //        FailOperation = StencilOperation.Keep,
//        //        DepthFailOperation = StencilOperation.Increment,
//        //        PassOperation = StencilOperation.Keep,
//        //        Comparison = Comparison.Always
//        //    },
//        //    // Stencil operation if pixel is back-facing.
//        //    BackFace = new DepthStencilOperationDescription()
//        //    {
//        //        FailOperation = StencilOperation.Keep,
//        //        DepthFailOperation = StencilOperation.Decrement,
//        //        PassOperation = StencilOperation.Keep,
//        //        Comparison = Comparison.Always
//        //    }
//        //};

//        //// Create the depth stencil state.
//        //var depthDisabledStencilState = SharpDX.Toolkit.Graphics.DepthStencilState.New(this.GraphicsDevice, depthDisabledStencilDesc);
//        ////turn z-buffer off
//        //this.GraphicsDevice.SetDepthStencilState(depthDisabledStencilState, 1);
//        ////quadEffect.Alpha = 2f;

//        }

//        /// <summary>
//        /// Allows the game to run logic such as updating the world,
//        /// checking for collisions, gathering input, and playing audio.
//        /// </summary>
//        /// <param name="gameTime">Provides a snapshot of timing values.</param>
//        protected override void Update(GameTime gameTime)
//        {
//            //if (GamePage.booly)
//            //{
//            //    // Allows the game to exit
//            //    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
//            //        this.Exit();

////#if WINDOWS
////            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
////                this.Exit();
////#endif

//            // TODO: Add your update logic here
//            // TODO: Add your update logic here
//            //// Rotate the cube.
//            var time = (float)gameTime.TotalGameTime.TotalSeconds;
//            quadEffect.World = Matrix.RotationX(time) * Matrix.RotationY(time * 2.0f) * Matrix.RotationZ(time * .7f);
//            quadEffect.Projection = Matrix.PerspectiveFovRH((float)Math.PI / 4.0f, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);

//            base.Update(gameTime);
//           // }
//        }

//        /// <summary>
//        /// This is called when the game should draw itself.
//        /// </summary>
//        /// <param name="gameTime">Provides a snapshot of timing values.</param>
//        protected override void Draw(GameTime gameTime)
//        {

//            //var screenViewport = new ViewportF(0f, 0f, GraphicsDevice.BackBuffer.Width, GraphicsDevice.BackBuffer.Height);
//            //var targetWidth = (int)(screenViewport.Width / 8f);
//            //var targetHeight = (int)(screenViewport.Height / 8f);
//            //var screenshot = ToDispose(SharpDX.Toolkit.Graphics.RenderTarget2D.New(GraphicsDevice,
//            //                                                                       targetWidth,
//            //                                                                       targetHeight,
//            //                                                                       SharpDX.Toolkit.Graphics.PixelFormat.B8G8R8A8.UNorm));

//            //RenderTarget2D screenshot = RenderTarget2D.New(GraphicsDevice, 100, 100,  new PixelFormat());
//        //    GraphicsDevice.SetRenderTargets(screenshot);
//            var x = new StreamOutputBufferBinding();
//            GraphicsDevice.SetStreamOutputTargets(x);
                
//            listVertexPositionColor.Clear();
//            listVertexPositionColor.AddRange((new Line(0, 0, 0, eDimension.X, 8)).Draw());
//            vertices = ToDisposeContent(SharpDX.Toolkit.Graphics.Buffer.Vertex.New(
//                 GraphicsDevice, listVertexPositionColor.ToArray()));
//            ToDisposeContent(vertices);
            
//            inputLayout = VertexInputLayout.FromBuffer(0, vertices);
//            GraphicsDevice.SetVertexBuffer(vertices);
//            GraphicsDevice.SetVertexInputLayout(inputLayout);
//            // Create an input layout from the vertices
//           // GraphicsDevice.SetVertexBuffer(quad.Vertices);
//            GraphicsDevice.Clear(Color.DarkOrange);

//            foreach (EffectPass pass in quadEffect.CurrentTechnique.Passes)
//            {
//                pass.Apply();

//                //GraphicsDevice.DrawIndexed(
//                //    PrimitiveType.TriangleList,vertices.ElementCount);//,
//               //    quad.Indexes.Count(), 0,4);
//         //   basicEffect.CurrentTechnique.Passes[0].Apply();
//                GraphicsDevice.Draw(PrimitiveType.TriangleList, vertices.ElementCount);
//            }

//            model.Draw(GraphicsDevice, world, view, projection);
//            GraphicsDevice.SetRenderTargets();
//       //     bsd=screenshot.GetDataAsImage();//.Save("F://fff.jpg");//(fs, width, height); // save render target to disk
//                //}
            
//            base.Draw(gameTime);
//        }
//        public Image bsd;
//        //private void DrawWithMetalEffect(Model model, Matrix world, Matrix view, Matrix proj)
//        //{

//        //    Matrix InverseWorldMatrix = Matrix.Invert(world);
//        //    Matrix ViewInverse = Matrix.Invert(view);
//        //    Effect effect = Content.Load<Effect>("file");
//        //    effect.CurrentTechnique = effect.Techniques["Simple"];
//        //    effect.Parameters["gWorldXf"].SetValue(world);
//        //    effect.Parameters["gWorldITXf"].SetValue(InverseWorldMatrix);
//        //    effect.Parameters["gWvpXf"].SetValue(world * view * proj);
//        //    effect.Parameters["gViewIXf"].SetValue(ViewInverse);

//        //    foreach (ModelMesh meshes in model.Meshes)
//        //    {
//        //        foreach (ModelMeshPart parts in meshes.MeshParts)
//        //            parts.Effect = basicEffect;
//        //        meshes.Draw();
//        //    }
//        //}

//    }
//}



////// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
////// 
////// Permission is hereby granted, free of charge, to any person obtaining a copy
////// of this software and associated documentation files (the "Software"), to deal
////// in the Software without restriction, including without limitation the rights
////// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
////// copies of the Software, and to permit persons to whom the Software is
////// furnished to do so, subject to the following conditions:
////// 
////// The above copyright notice and this permission notice shall be included in
////// all copies or substantial portions of the Software.
////// 
////// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
////// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
////// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
////// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
////// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
////// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
////// THE SOFTWARE.

////using System;
////using System.Diagnostics;

////using SharpDX;
////using SharpDX.Direct3D11;
////using SharpDX.Toolkit;

////namespace MyGame5
////{
////    // Use this namespace here in case we need to use Direct3D11 namespace as well, as this
////    // namespace will override the Direct3D11.
////    using SharpDX.Toolkit.Graphics;

////    /// <summary>
////    /// Simple CustomEffect application using SharpDX.Toolkit.
////    /// The purpose of this application is to use a custom Effect.
////    /// </summary>
////    public class MyGame5 : Game
////    {
////        private GraphicsDeviceManager graphicsDeviceManager;
////        private Effect metaTunnelEffect;

////        /// <summary>
////        /// Initializes a new instance of the <see cref="CustomEffectGame" /> class.
////        /// </summary>
////        public MyGame5()
////        {
////            // Creates a graphics manager. This is mandatory.
////            graphicsDeviceManager = new GraphicsDeviceManager(this);
////            graphicsDeviceManager.PreferredDepthStencilFormat = DepthFormat.None;

////            // Add dynamic EffectCompilerSystem, only active on desktop and compiled in debug mode.
////            // While the program is running, you can edit the shader and save a new version
////            // The EffectCompilerSystem will recompile dynamically the effect without having to
////            // reload/recompile the whole application.
////            GameSystems.Add(new EffectCompilerSystem(this));

////            // Setup the relative directory to the executable directory
////            // for loading contents with the ContentManager
////            Content.RootDirectory = "Content";
////        }

////        protected override void Initialize()
////        {
////            Window.Title = "MetaTunnel Effect by XT95/Frequency";

////            base.Initialize();
////        }

////        protected override void LoadContent()
////        {
////            // Loads the effect
////            metaTunnelEffect = Content.Load<Effect>("metatunnel");

////            base.LoadContent();
////        }

////        protected override void Draw(GameTime gameTime)
////        {
////            // As the shader is writing to the screen, we don't need to clear it.
////            metaTunnelEffect.Parameters["w"].SetValue((float)gameTime.TotalGameTime.TotalSeconds);

////            // Draw a full screen quad using the specified effect.
////            GraphicsDevice.Quad.Draw(metaTunnelEffect, true);

////            // Handle base.Draw
////            base.Draw(gameTime);
////        }
////    }
////}

//////// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
//////// 
//////// Permission is hereby granted, free of charge, to any person obtaining a copy
//////// of this software and associated documentation files (the "Software"), to deal
//////// in the Software without restriction, including without limitation the rights
//////// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//////// copies of the Software, and to permit persons to whom the Software is
//////// furnished to do so, subject to the following conditions:
//////// 
//////// The above copyright notice and this permission notice shall be included in
//////// all copies or substantial portions of the Software.
//////// 
//////// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//////// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//////// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//////// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//////// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//////// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//////// THE SOFTWARE.

//////using System;
//////using System.Collections.Generic;

//////using SharpDX;
//////using SharpDX.Direct3D;
//////using SharpDX.Direct3D11;
//////using SharpDX.Toolkit;
//////using SharpDX.Toolkit.Input;

//////namespace MyGame5
//////{
//////    // Use this namespace here in case we need to use Direct3D11 namespace as well, as this
//////    // namespace will override the Direct3D11.
//////    using SharpDX.Toolkit.Graphics;

//////    /// <summary>
//////    /// Simple SpriteBatchAndFont application using SharpDX.Toolkit.
//////    /// The purpose of this application is to use SpriteBatch and SpriteFont.
//////    /// </summary>
//////    public class MyGame5 : Game
//////    {
//////        private GraphicsDeviceManager graphicsDeviceManager;
//////        private SpriteBatch spriteBatch;
//////        private SpriteFont arial16BMFont;

//////        private PointerManager pointer;

//////        private Model model;

//////        private List<Model> models;

//////        private BoundingSphere modelBounds;
//////        private Matrix world;
//////        private Matrix view;
//////        private Matrix projection;

//////        /// <summary>
//////        /// Initializes a new instance of the <see cref="ModelRenderingGame" /> class.
//////        /// </summary>
//////        public MyGame5()
//////        {
//////            // Creates a graphics manager. This is mandatory.
//////            graphicsDeviceManager = new GraphicsDeviceManager(this);
//////            graphicsDeviceManager.PreferredGraphicsProfile = new FeatureLevel[] { FeatureLevel.Level_9_1, };

//////            pointer = new PointerManager(this);

//////            // Setup the relative directory to the executable directory
//////            // for loading contents with the ContentManager
//////            Content.RootDirectory = "Content";
//////        }

//////        protected override void LoadContent()
//////        {
//////            // Load the fonts
//////            arial16BMFont = Content.Load<SpriteFont>("Arial16");

//////            // Load the model (by default the model is loaded with a BasicEffect. Use ModelContentReaderOptions to change the behavior at loading time.
//////            models = new List<Model>();
//////            foreach (var modelName in new[] { "Ship", "Scene1", "ShipDiffuse"})
//////            {
//////                model = Content.Load<Model>(modelName);

//////                // Enable default lighting  on model.
//////                BasicEffect.EnableDefaultLighting(model, true);

//////                models.Add(model);
//////            }
//////            model = models[0];

//////            // Instantiate a SpriteBatch
//////            spriteBatch = ToDisposeContent(new SpriteBatch(GraphicsDevice));

//////            base.LoadContent();
//////        }

//////        protected override void Initialize()
//////        {
//////            Window.Title = "Model Rendering Demo";
//////            base.Initialize();
//////        }

//////        protected override void Update(GameTime gameTime)
//////        {
//////            base.Update(gameTime);

//////            var pointerState = pointer.GetState();
//////            if (pointerState.Points.Count > 0 && pointerState.Points[0].EventType == PointerEventType.Released)
//////            {
//////                // Go to next model when pressing key space
//////                model = models[(models.IndexOf(model) + 1) % models.Count];
//////            }

//////            //גבולות המודל
//////            // Calculate the bounds of this model
//////            modelBounds = model.CalculateBounds();

//////            // Calculates the world and the view based on the model size
//////            const float MaxModelSize = 10.0f;
//////            var scaling = MaxModelSize / modelBounds.Radius;
//////            view = Matrix.LookAtRH(new Vector3(0, 0, MaxModelSize * 2.5f), new Vector3(0, 0, 0), Vector3.UnitY);
//////            projection = Matrix.PerspectiveFovRH(0.9f, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, MaxModelSize * 10.0f);
//////            world = Matrix.Translation(-modelBounds.Center.X, -modelBounds.Center.Y, -modelBounds.Center.Z) * Matrix.Scaling(scaling) * Matrix.RotationY((float)gameTime.TotalGameTime.TotalSeconds);
//////        }

//////        protected override void Draw(GameTime gameTime)
//////        {
//////            // Clears the screen with the Color.CornflowerBlue
//////            GraphicsDevice.Clear(Color.CornflowerBlue);

//////            // Draw the model
//////            model.Draw(GraphicsDevice, world, view, projection);

//////            // Render the text
//////            spriteBatch.Begin();
//////            spriteBatch.DrawString(arial16BMFont, "Press the pointer to switch models...\r\nCurrent Model: " + model.Name, new Vector2(16, 16), Color.White);
//////            spriteBatch.End();

//////            // Handle base.Draw
//////            base.Draw(gameTime);
//////        }
//////    }
//////}



////////using System;
////////using System.Text;
////////using SharpDX;


////////namespace MyGame5
////////{
////////    // Use these namespaces here to override SharpDX.Direct3D11
////////    using SharpDX.Toolkit;
////////    using SharpDX.Toolkit.Graphics;
////////    using SharpDX.Toolkit.Input;

////////    /// <summary>
////////    /// Simple MyGame5 game using SharpDX.Toolkit.
////////    /// </summary>
////////    public class MyGame5 : Game
////////    {
////////        private GraphicsDeviceManager graphicsDeviceManager;
////////        private SpriteBatch spriteBatch;
////////        private SpriteFont arial16Font;

////////        private Matrix view;
////////        private Matrix projection;

////////        private Model model;

////////        private Effect bloomEffect;
////////        private RenderTarget2D renderTargetOffScreen;
////////        private RenderTarget2D[] renderTargetDownScales;
////////        private RenderTarget2D renderTargetBlurTemp;

////////        private BasicEffect basicEffect;
////////        private GeometricPrimitive primitive;

////////        private PointerManager pointer;
////////        private PointerState pointerState;

////////        /// <summary>
////////        /// Initializes a new instance of the <see cref="MyGame5" /> class.
////////        /// </summary>
////////        public MyGame5()
////////        {
////////            // Creates a graphics manager. This is mandatory.
////////            graphicsDeviceManager = new GraphicsDeviceManager(this);

////////            // Setup the relative directory to the executable directory
////////            // for loading contents with the ContentManager
////////            Content.RootDirectory = "Content";

////////            // Initialize input pointer system
////////            pointer = new PointerManager(this);
////////        }

////////        protected override void Initialize()
////////        {
////////            // Modify the title of the window
////////            Window.Title = "MyGame5";

////////            base.Initialize();
////////        }

////////        protected override void LoadContent()
////////        {
////////            // Instantiate a SpriteBatch
////////            spriteBatch = ToDisposeContent(new SpriteBatch(GraphicsDevice));

////////            // Loads a sprite font
////////            // The [Arial16.xml] file is defined with the build action [ToolkitFont] in the project
////////            arial16Font = Content.Load<SpriteFont>("Arial16");

////////            // Load a 3D model
////////            // The [Ship.fbx] file is defined with the build action [ToolkitModel] in the project
////////            model = Content.Load<Model>("Ship");

////////            // Enable default lighting on model.
////////            BasicEffect.EnableDefaultLighting(model, true);

////////            // Bloom Effect
////////            // The [Bloom.fx] file is defined with the build action [ToolkitFxc] in the project
////////            bloomEffect = Content.Load<Effect>("Bloom");

////////            // Creates render targets for bloom effect
////////            renderTargetDownScales = new RenderTarget2D[5];
////////            var backDesc = GraphicsDevice.BackBuffer.Description;
////////            renderTargetOffScreen = ToDisposeContent(RenderTarget2D.New(GraphicsDevice, backDesc.Width, backDesc.Height, 1, backDesc.Format));
////////            for (int i = 0; i < renderTargetDownScales.Length; i++)
////////            {
////////                renderTargetDownScales[i] = ToDisposeContent(RenderTarget2D.New(GraphicsDevice, backDesc.Width >> i, backDesc.Height >> i, 1, backDesc.Format));
////////            }
////////            renderTargetBlurTemp = ToDisposeContent((RenderTarget2D)renderTargetDownScales[renderTargetDownScales.Length - 1].Clone());

////////            // Creates a basic effect
////////            basicEffect = ToDisposeContent(new BasicEffect(GraphicsDevice));
////////            basicEffect.PreferPerPixelLighting = true;
////////            basicEffect.EnableDefaultLighting();

////////            // Creates torus primitive
////////            primitive = ToDisposeContent(GeometricPrimitive.Torus.New(GraphicsDevice));

////////            base.LoadContent();
////////        }

////////        protected override void Update(GameTime gameTime)
////////        {
////////            base.Update(gameTime);

////////            // Calculates the world and the view based on the model size
////////            view = Matrix.LookAtRH(new Vector3(0.0f, 0.0f, 7.0f), new Vector3(0, 0.0f, 0), Vector3.UnitY);
////////            projection = Matrix.PerspectiveFovRH(0.9f, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);

////////            // Update basic effect for rendering the Primitive
////////            basicEffect.View = view;
////////            basicEffect.Projection = projection;

////////            // Get the current state of the pointer
////////            pointerState = pointer.GetState();
////////        }

////////        protected override void Draw(GameTime gameTime)
////////        {
////////            // Use time in seconds directly
////////            var time = (float)gameTime.TotalGameTime.TotalSeconds;

////////            // Make offline rendering
////////            GraphicsDevice.SetRenderTargets(GraphicsDevice.DepthStencilBuffer, renderTargetOffScreen);

////////            // Clears the screen with the Color.CornflowerBlue
////////            GraphicsDevice.Clear(Color.CornflowerBlue);

////////            // Constant used to translate 3d models
////////            float translateX = 0.0f;

////////            // ------------------------------------------------------------------------
////////            // Draw the 3d model
////////            // ------------------------------------------------------------------------
////////            var world = Matrix.Scaling(0.003f) *
////////                        Matrix.RotationY(time) *
////////                        Matrix.Translation(0, -1.5f, 2.0f);
////////            model.Draw(GraphicsDevice, world, view, projection);
////////            translateX += 3.5f;

////////            // ------------------------------------------------------------------------
////////            // Draw the 3d primitive using BasicEffect
////////            // ------------------------------------------------------------------------
////////            basicEffect.World = Matrix.Scaling(2.0f, 2.0f, 2.0f) *
////////                                Matrix.RotationX(0.8f * (float)Math.Sin(time * 1.45)) *
////////                                Matrix.RotationY(time * 2.0f) *
////////                                Matrix.RotationZ(0) *
////////                                Matrix.Translation(translateX, -1.0f, 0);
////////            primitive.Draw(basicEffect);

////////            // ------------------------------------------------------------------------
////////            // Draw the some 2d text
////////            // ------------------------------------------------------------------------
////////            spriteBatch.Begin();
////////            var text = new StringBuilder("This text is displayed with SpriteBatch").AppendLine();

////////            var points = pointerState.Points;
////////            if (points.Count > 0)
////////            {
////////                foreach (var point in points)
////////                {
////////                    text.AppendFormat("Pointer event: [{0}] {1} {2} ({3}, {4})", point.PointerId, point.DeviceType, point.EventType, point.Position.X, point.Position.Y).AppendLine();
////////                }
////////            }

////////            spriteBatch.DrawString(arial16Font, text.ToString(), new Vector2(16, 16), Color.White);
////////            spriteBatch.End();

////////            // ------------------------------------------------------------------------
////////            // Cheap bloom post effect
////////            // Blur applied only on latest downscale render target
////////            // ------------------------------------------------------------------------

////////            // Setup states for posteffect
////////            GraphicsDevice.SetRasterizerState(GraphicsDevice.RasterizerStates.Default);
////////            GraphicsDevice.SetBlendState(GraphicsDevice.BlendStates.Default);
////////            GraphicsDevice.SetDepthStencilState(GraphicsDevice.DepthStencilStates.None);

////////            // Apply BrightPass
////////            const float brightPassThreshold = 0.5f;
////////            GraphicsDevice.SetRenderTargets(renderTargetDownScales[0]);
////////            bloomEffect.CurrentTechnique = bloomEffect.Techniques["BrightPassTechnique"];
////////            bloomEffect.Parameters["Texture"].SetResource(renderTargetOffScreen);
////////            bloomEffect.Parameters["PointSampler"].SetResource(GraphicsDevice.SamplerStates.PointClamp);
////////            bloomEffect.Parameters["BrightPassThreshold"].SetValue(brightPassThreshold);
////////        //    GraphicsDevice.DrawQuad(bloomEffect.CurrentTechnique.Passes[0]);

////////            // Down scale passes
////////            for (int i = 1; i < renderTargetDownScales.Length; i++)
////////            {
////////                GraphicsDevice.SetRenderTargets(renderTargetDownScales[i]);
////////          //      GraphicsDevice.DrawQuad(renderTargetDownScales[0]);
////////            }

////////            // Horizontal blur pass
////////            var renderTargetBlur = renderTargetDownScales[renderTargetDownScales.Length - 1];
////////            GraphicsDevice.SetRenderTargets(renderTargetBlurTemp);
////////            bloomEffect.CurrentTechnique = bloomEffect.Techniques["BlurPassTechnique"];
////////            bloomEffect.Parameters["Texture"].SetResource(renderTargetBlur);
////////            bloomEffect.Parameters["LinearSampler"].SetResource(GraphicsDevice.SamplerStates.LinearClamp);
////////            bloomEffect.Parameters["TextureTexelSize"].SetValue(new Vector2(1.0f / renderTargetBlurTemp.Width, 1.0f / renderTargetBlurTemp.Height));
////////            //GraphicsDevice.DrawQuad(bloomEffect.CurrentTechnique.Passes[0]);

////////            // Vertical blur pass
////////            GraphicsDevice.SetRenderTargets(renderTargetBlur);
////////            bloomEffect.Parameters["Texture"].SetResource(renderTargetBlurTemp);
////////            //GraphicsDevice.DrawQuad(bloomEffect.CurrentTechnique.Passes[1]);

////////            // Render to screen
////////            GraphicsDevice.SetRenderTargets(GraphicsDevice.BackBuffer);
////////            //G/raphicsDevice.DrawQuad(renderTargetOffScreen);

////////            // Add bloom on top of it
////////            GraphicsDevice.SetBlendState(GraphicsDevice.BlendStates.Additive);
////////            //GraphicsDevice.DrawQuad(renderTargetBlur);
////////            GraphicsDevice.SetBlendState(GraphicsDevice.BlendStates.Default);

////////            base.Draw(gameTime);
////////        }
////////    }
////////}
