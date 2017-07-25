
//יתכן וקובץ זה אינו המעודכן ביותר............
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
using System;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using System.Collections.Generic;
using Windows.Storage;
using System.IO;


namespace Isometric
{


    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        // GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private GraphicsDeviceManager graphicsDeviceManager;
        private BasicEffect basicEffect;
        private Buffer<VertexPositionColor> vertices;
        private VertexInputLayout inputLayout;
        private List<VertexPositionColor> listVertexPositionColor = new List<VertexPositionColor>();
        public Game1()
        {
            //graphics = new GraphicsDeviceManager(this);
            //Content.RootDirectory = "Content";
            // Creates a graphics manager. This is mandatory.
            graphicsDeviceManager = new GraphicsDeviceManager(this);

            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            Window.Title = "MiniRoll demo";
            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            // Creates a basic effect
            basicEffect = ToDisposeContent(new BasicEffect(GraphicsDevice)
            {
                VertexColorEnabled = true,
                View = Matrix.LookAtRH(new Vector3(0, 0, 50), new Vector3(0, 0, 0), Vector3.UnitY),
                Projection = Matrix.PerspectiveFovRH((float)Math.PI / 4.0f, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f),
                World = Matrix.Identity
            });


            //  Line x = new Line(0.5f, 0.5f, 0.5f, eAxis.x, 11);
            //  Line y = new Line(0.5f, 0.5f, 0.5f, eAxis.y, 5.5f);
            //  Corner c = new Corner(0.5f, 0.5f, 0.5f);
            //  x.Draw( listVertexPositionColor);
            // y.Draw( listVertexPositionColor);
            // c.Draw(listVertexPositionColor);
            // listVertexPositionColor= ManagerGame.matrixSystem.ToListShape();

            //foreach (var item in ManagerGame.matrixUser.ToListShape())
            //{
            //    item.Draw(listVertexPositionColor);
            //}
            //  vertices = ToDisposeContent(SharpDX.Toolkit.Graphics.Buffer.Vertex.New(
            //      GraphicsDevice, listVertexPositionColor.ToArray()));    
            //  ToDisposeContent(vertices);

            //  // Create an input layout from the vertices
            //  inputLayout = VertexInputLayout.FromBuffer(0, vertices);

            base.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            //// Rotate the cube.
            //var time = (float)gameTime.TotalGameTime.TotalSeconds;
            //basicEffect.World = Matrix.RotationX(time) * Matrix.RotationY(time * 2.0f) * Matrix.RotationZ(time * .7f);
            //basicEffect.Projection = Matrix.PerspectiveFovRH((float)Math.PI / 4.0f, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);

            // Handle base.Update
            base.Update(gameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            try
            {
                listVertexPositionColor.Clear();
              
                if (ManagerGame.matrixSystem.ToListShape().Count > 0)
                    foreach (var item in ManagerGame.matrixSystem.ToListShape())
                    {
                        if (item.Draw() != null)
                            listVertexPositionColor.AddRange(item.Draw());
                    }
                listVertexPositionColor.AddRange(ManagerGame.cursor.Draw());

                //// //בין  XלZ
                // listVertexPositionColor.AddRange((new CornerNew(0, 0, 0, 1, 1)).Draw());
                //listVertexPositionColor.AddRange((new CornerNew(11, 0, 0, 1, 2)).Draw());
                // listVertexPositionColor.AddRange((new CornerNew(11, 0f, 11, 1, 3)).Draw());
                // listVertexPositionColor.AddRange((new CornerNew(0, 0f, 11, 1, 4)).Draw());
                //// //
                //listVertexPositionColor.AddRange((new CornerNew(0, 0f, 0f, 2, 1)).Draw());
                //listVertexPositionColor.AddRange((new CornerNew(0, 11, 0f, 2, 2)).Draw());
                //listVertexPositionColor.AddRange((new CornerNew(11, 11, 0f, 2, 3)).Draw());
                //listVertexPositionColor.AddRange((new CornerNew(11, 0, 0, 2, 4)).Draw());

                //listVertexPositionColor.AddRange((new CornerNew(0, 0f, 0f, 3, 1)).Draw());
                //listVertexPositionColor.AddRange((new CornerNew(0, 0f, 11, 3, 2)).Draw());
                //listVertexPositionColor.AddRange((new CornerNew(0, 11, 11, 3, 3)).Draw());
                //listVertexPositionColor.AddRange((new CornerNew(0, 11, 0f, 3, 4)).Draw());

                //// //////////////==============
                //// listVertexPositionColor.AddRange((new CornerNew(0, 0, 0, 1, 1)).Draw());
                //// listVertexPositionColor.AddRange((new CornerNew(11, 11, 0, 1, 2)).Draw());
                //// listVertexPositionColor.AddRange((new CornerNew(11, 11, 11, 1, 3)).Draw());
                //// listVertexPositionColor.AddRange((new CornerNew(0, 11, 11, 1, 4)).Draw());
                //// //
                //// listVertexPositionColor.AddRange((new CornerNew(0, 0f, 0f, 2, 1)).Draw());
                //// listVertexPositionColor.AddRange((new CornerNew(0, 11, 11, 2, 2)).Draw());
                //// listVertexPositionColor.AddRange((new CornerNew(11, 11, 11, 2, 3)).Draw());
                //// listVertexPositionColor.AddRange((new CornerNew(11, 0, 11, 2, 4)).Draw());

                //// listVertexPositionColor.AddRange((new CornerNew(0, 0f, 0f, 3, 1)).Draw());
                //// listVertexPositionColor.AddRange((new CornerNew(11, 0f, 11, 3, 2)).Draw());
                //// listVertexPositionColor.AddRange((new CornerNew(11, 11, 11, 3, 3)).Draw());
                //// listVertexPositionColor.AddRange((new CornerNew(11, 11, 0f, 3, 4)).Draw());
                ////// listVertexPositionColor.AddRange((new CornerNew(11, 0, 11, 1,2)).Draw());
                //// //listVertexPositionColor.AddRange((new CornerNew(0, 0f, 0f, 1, 0)).Draw());
                //// //listVertexPositionColor.AddRange((new CornerNew(3, 0f, 0f)).Draw());
                //// //listVertexPositionColor.AddRange((new CornerNew(4, 0f, 0f)).Draw());
                //// //listVertexPositionColor.AddRange((new CornerNew(5, 0f, 0f)).Draw());
                //// //listVertexPositionColor.AddRange((new CornerNew(6, 0f, 0f)).Draw());
                ////
                //// listVertexPositionColor.AddRange((new Line(0, 0, 0, eAxis.y, 11)).Draw());
                //// listVertexPositionColor.AddRange((new Line(0, 0, 0, eAxis.z, 11)).Draw());

                //// listVertexPositionColor.AddRange((new Line(0,11, 0, eAxis.x, 11)).Draw());
                //// listVertexPositionColor.AddRange((new Line(11, 0, 0, eAxis.y, 11)).Draw());
                //// listVertexPositionColor.AddRange((new Line(11, 0, 0, eAxis.z, 11)).Draw());

                //// listVertexPositionColor.AddRange((new Line(0, 0, 11, eAxis.x, 11)).Draw());
                //// listVertexPositionColor.AddRange((new Line(0, 0, 11, eAxis.y, 11)).Draw());
                //// listVertexPositionColor.AddRange((new Line(0, 11, 0, eAxis.z, 11)).Draw());

                //// listVertexPositionColor.AddRange((new Line(0, 11, 11, eAxis.x, 11)).Draw());
                //// listVertexPositionColor.AddRange((new Line(11, 0, 11, eAxis.y, 11)).Draw());
                //// listVertexPositionColor.AddRange((new Line(11, 11, 0, eAxis.z, 11)).Draw());
                vertices = ToDisposeContent(SharpDX.Toolkit.Graphics.Buffer.Vertex.New(
                    GraphicsDevice, listVertexPositionColor.ToArray()));
                ToDisposeContent(vertices);

                // Create an input layout from the vertices
                inputLayout = VertexInputLayout.FromBuffer(0, vertices);
            }
            catch (SharpDX.SharpDXException ex)
            {
                if (ex.ResultCode == SharpDX.DXGI.ResultCode.DeviceRemoved
         || ex.ResultCode == SharpDX.DXGI.ResultCode.DeviceReset)
                {

                    //graphicsDeviceManager = null;
                    //graphicsDeviceManager = new GraphicsDeviceManager(this);

                    Draw(gameTime);
                }
            }     


            //GraphicsDevice.Clear(Color.CornflowerBlue);

            //// TODO: Add your drawing code here

            //base.Draw(gameTime);
            // Clears the screen with the Color.CornflowerBlue
            GraphicsDevice.Clear(Color.Add(Color.DeepPink, Color.LimeGreen));

            // Setup the vertices
            GraphicsDevice.SetVertexBuffer(vertices);
            GraphicsDevice.SetVertexInputLayout(inputLayout);

            // Apply the basic effect technique and draw the rotating cube
            basicEffect.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.Draw(PrimitiveType.TriangleList, vertices.ElementCount);

            // Handle base.Draw
            base.Draw(gameTime);
        }

        //async private void TakeScreenShot(GraphicsDevice device)
        //{

        //    Color[] screenData = new Color[device.PresentationParameters.BackBufferWidth *
        //                                   device.PresentationParameters.BackBufferHeight];
        //    GraphicsDevice
        //    RenderTarget2D screenShot = new RenderTarget2D(device,
        //        device.PresentationParameters.BackBufferWidth,
        //        device.PresentationParameters.BackBufferHeight);

        //    device.SetRenderTarget(screenShot);

        //    //Put your drawing method here.  Just copy and paste if from your game class/wherever  
        //    //you are performing drawing.  Alternatively, you can just pass in to this method whatever object is performing the drawing

        //    //In My case, I am using the GameStateManagement Sample,  
        //    //where all screens draw themselves.  So in my case this  
        //    //method is contained in the Screen Manager Class 
        //    // Draw(new GameTime());

        //    device.SetRenderTarget(null);

        //    int index = 0;
        //    string name = "Screenshot" + index + ".png";
        //    //while (File.Exists(name))
        //    //{
        //    //    index++;
        //    //    name = "Screenshot" + index + ".jpg";
        //    //}

        //    StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
        //    Stream s = await storageFolder.OpenStreamForWriteAsync(name, new CreationCollisionOption());

        //    screenShot.SaveAsPng(s, screenShot.Width, screenShot.Height);
        //    screenShot.Dispose();
        //}
//        protected override void Draw(GameTime g)
//{
//   // RealUpdate(); // Do the update once before drawing each frame.
//          RenderTarget2D  screenshot =  RenderTarget2D.New(GraphicsDevice, width, height, false, SurfaceFormat.Color, null);
//    GraphicsDevice.SetRenderTargets(screenshot); // rendering to the render target
//    //
//    // Render your scene here
//    //
//   // GraphicsDevice.SetRenderTargets(null); // finished with render target
//            Texture2D
//    using(FileStream fs = new FileStream(@"screenshot"+(i++)+@".png", FileMode.OpenOrCreate)
//    {
//        screenshot.SaveAsPng(fs, width, height); // save render target to disk
//    }

//    // Optionally you could render your render target to the screen as well so you can see the result!

//    if(done)
//        Exit();
//}
    }
}
