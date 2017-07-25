//יתכן וקובץ זה אינו המעודכן ביותר............
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
using System;
using SharpDX;
using SharpDX.Toolkit;
using System.Collections.Generic;
using Windows.Storage;
using System.IO;


namespace Isometric._3DObjects
{


    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;
    using System.Threading.Tasks;
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Draw3D : Game
    {
        private SpriteBatch spriteBatch;
        private GraphicsDeviceManager graphics;
        private Buffer<VertexPositionNormalTexture> vertices;
        private VertexInputLayout inputLayout;
        private List<VertexPositionNormalTexture> listVertexPositionColor = new List<VertexPositionNormalTexture>();
        //private Texture2D textureCube;
        //private Texture2D textureCursor;
        private BasicEffect basicEffectLines;
        private BasicEffect basicEffectCursor;
        private BasicEffect basicEffectCube;
        private MouseManager mouseManager;
        private MouseState mouseState;
        public Matrix world { get; set; }
        List<VertexPositionNormalTexture> listVertexCubeInternal;
        List<VertexPositionNormalTexture> listVertexCubeExterior;
        private Model model;
        private Matrix view;
        private Matrix projection;
        List<VertexPositionNormalTexture> listVertexLines = new List<VertexPositionNormalTexture>();


        public bool EndPage { get; set; }
        public Draw3D(bool EndPage = false)
        {
            this.EndPage = EndPage;
            // Creates a graphics manager. This is mandatory.
            graphics = new GraphicsDeviceManager(this);
            mouseManager = new MouseManager(this);
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
            Window.Title = "MiniRoll demo";
            base.Initialize();
        }


        protected override void LoadContent()
        {
            //אפקט לקוים
            spriteBatch = new SpriteBatch(GraphicsDevice);
            basicEffectLines = new BasicEffect(graphics.GraphicsDevice);
            basicEffectLines.TextureEnabled = true;
            basicEffectLines.EnableDefaultLighting();
            basicEffectLines.View = Matrix.LookAtRH(new Vector3(0, 0, 35), new Vector3(0, 0, 0), Vector3.UnitY);
            basicEffectLines.Projection = Matrix.PerspectiveFovRH((float)Math.PI / 4.0f, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, 50.0f);
            basicEffectLines.World = Matrix.Identity;
            basicEffectLines.Texture = Content.Load<Texture2D>("mat");//טקסטורת גליל
            basicEffectLines.LightingEnabled = true;
            basicEffectLines.Alpha = 0.9f;
            //red green blue
            //basicEffectLines.DiffuseColor = new Vector4(0, 0.601f, 0.127f, 1);
            //basicEffectLines.SpecularColor = new Vector3(0, 0.601f, 0.127f);
            //basicEffectLines.SpecularPower = 5.0f;
            //basicEffectLines.AmbientLightColor =
            //  new Vector3(0.5f, 5, 0.5f);

            //  spriteBatch = new SpriteBatch(GraphicsDevice);

            // Creates a basic effect 
            //אפקט לסמן
            basicEffectCursor = new BasicEffect(graphics.GraphicsDevice);
            basicEffectCursor.EnableDefaultLighting();
            basicEffectCursor.View = Matrix.LookAtRH(new Vector3(0, 0, 35), new Vector3(0, 0, 0), Vector3.UnitY);
            basicEffectCursor.Projection = Matrix.PerspectiveFovRH((float)Math.PI / 4.0f, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, 50.0f);
            basicEffectCursor.World = Matrix.Identity;
            //אפקט לקוביה
            basicEffectCube = new BasicEffect(graphics.GraphicsDevice);
            basicEffectCube.EnableDefaultLighting();
            basicEffectCube.Projection = Matrix.PerspectiveFovRH((float)Math.PI / 4.0f, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, 50.0f);
            basicEffectCube.World = Matrix.Identity;
            basicEffectCube.View = Matrix.LookAtRH(new Vector3(0, 0, 4), new Vector3(0, 0, 0), Vector3.UnitY);
            basicEffectCube.Texture = Content.Load<Texture2D>("bsd");
            basicEffectCube.TextureEnabled = true;
            basicEffectCube.EnableDefaultLighting();

            basicEffectCube.LightingEnabled = true;
            basicEffectCube.DiffuseColor = new Vector4(0.9f, 0.9f, 0.99f, 0.3f);
            basicEffectCube.SpecularColor = new Vector3(0.9f, 0.99f, 0.8f);
            basicEffectCube.SpecularPower = 0.3f;
            basicEffectCube.AmbientLightColor = new Vector3(0.99f, 0.9f, 0.9f);
            //basicEffectCube.LightingEnabled = true;
            //basicEffectCube.DiffuseColor = new Vector4(1.0f, 1.0f, 1.0f, 0f);
            //basicEffectCube.SpecularColor = new Vector3(0.2f, 0.2f, 0.2f);
            //basicEffectCube.SpecularPower = 5.0f;
            //basicEffectCube.AmbientLightColor =
            //  new Vector3(0.5f, 0.5f, 0.5f);

            basicEffectCube.DirectionalLight0.Enabled = true;
            basicEffectCube.DirectionalLight0.DiffuseColor = Vector3.One;
            basicEffectCube.DirectionalLight0.Direction =
                      Vector3.Normalize(new Vector3(1.0f, 1.0f, -1.0f));
            basicEffectCube.DirectionalLight0.SpecularColor = Vector3.One;
            basicEffectCube.DirectionalLight1.Enabled = true;
            basicEffectCube.DirectionalLight1.DiffuseColor =
                      new Vector3(1.1f, 0.0f, 0.1f);
            basicEffectCube.DirectionalLight1.Direction =
                     Vector3.Normalize(new Vector3(-1.0f, -1.0f, 1.0f));
            basicEffectCube.DirectionalLight1.SpecularColor =
                      new Vector3(1.0f, 0.0f, 0.0f);
            basicEffectCube.DirectionalLight2.Enabled = true;
            basicEffectCube.DirectionalLight2.DiffuseColor =
                      new Vector3(0.1f, 1.0f, 0.1f);
            basicEffectCube.DirectionalLight2.Direction =
                     Vector3.Normalize(new Vector3(1.0f, -1.0f, 0.0f));
            basicEffectCube.DirectionalLight2.SpecularColor =
                      new Vector3(0.1f, 1.0f, 0.1f);

            //float N_minus = -0.40f;
            //float N_plus = 0.86f; 
            float N_minus = -0.63f;
            float N_plus = 0.63f;
            #region cube positions
            listVertexCubeExterior = new List<VertexPositionNormalTexture>()  {
      
                        new VertexPositionNormalTexture(new Vector3(N_minus, N_minus, N_plus),new Vector3(0,0, N_plus),  new Vector2(0,1)), // Back
                        new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_plus),new Vector3(0, 0, N_plus),  new Vector2(0,0)),
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_plus),new Vector3(0, 0, N_plus),  new Vector2(1,0)),
                        new VertexPositionNormalTexture(new Vector3(N_minus, N_minus, N_plus),new Vector3(0, 0, N_plus),  new Vector2(0,1)),
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_plus),new Vector3(0, 0, N_plus),  new Vector2(1,0)),
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_minus, N_plus),new Vector3(0, 0, N_plus),  new Vector2(1,1)),
                        
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_plus),new Vector3(0, 0, N_plus),  new Vector2(1,0)), // Back
                        //new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_plus),new Vector3(0, 0, N_plus),  new Vector2(0,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_minus, N_minus, N_plus),new Vector3(0,0, N_plus),  new Vector2(0,1)),
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_minus, N_plus),new Vector3(0, 0, N_plus),  new Vector2(1,1)),
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_plus),new Vector3(0, 0, N_plus),  new Vector2(1,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_minus, N_minus, N_plus),new Vector3(0, 0, N_plus),  new Vector2(0,1)),
                        
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_minus,N_minus),new Vector3(0, 0,N_minus),  new Vector2(1,1)),// Front קדימה
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_plus,N_minus),new Vector3(0, 0, N_minus),  new Vector2(1,0)),
                        new VertexPositionNormalTexture(new Vector3(N_minus, N_minus, N_minus),new Vector3(0,0,N_minus),  new Vector2(0,1)),
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_minus),new Vector3(0,0,N_minus),  new Vector2(1,0)),
                        new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_minus),new Vector3(0, 0, N_minus),  new Vector2(0,0)),
                        new VertexPositionNormalTexture(new Vector3(N_minus, N_minus,N_minus),new Vector3(0,0,N_minus),  new Vector2(0,1)),
                        //Pנים
                        //new VertexPositionNormalTexture(new Vector3(N_minus, N_minus,N_minus),new Vector3(0,0,N_minus),  new Vector2(0,1)),// Front
                        //new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_minus),new Vector3(0, 0, N_minus),  new Vector2(0,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_minus),new Vector3(0,0,N_minus),  new Vector2(1,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_minus, N_minus, N_minus),new Vector3(0,0,N_minus),  new Vector2(0,1)),
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_plus,N_minus),new Vector3(0, 0, N_minus),  new Vector2(1,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_minus,N_minus),new Vector3(0, 0,N_minus),  new Vector2(1,1)),

                                              
                        new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_plus),new Vector3(0, N_plus, 0),  new Vector2(0,1)), // Top
                        new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_minus),new Vector3(0, N_plus,0),  new Vector2(0,0)),
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_minus),new Vector3(0, N_plus, 0),  new Vector2(1,0)),
                        new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_plus),new Vector3(0, N_plus, 0),  new Vector2(0,1)),
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_minus),new Vector3(0, N_plus, 0),  new Vector2(1,0)),
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_plus),new Vector3(0, N_plus, 0),  new Vector2(1,1)),
                                                
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_plus),new Vector3(0, N_plus, 0),  new Vector2(1,1)),
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_minus),new Vector3(0, N_plus, 0),  new Vector2(1,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_plus),new Vector3(0, N_plus, 0),  new Vector2(0,1)),
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_minus),new Vector3(0, N_plus, 0),  new Vector2(1,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_minus),new Vector3(0, N_plus,0),  new Vector2(0,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_plus),new Vector3(0, N_plus, 0),  new Vector2(0,1)), // Top
                        
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_minus, N_plus),new Vector3(0, N_minus, 0),  new Vector2(1,1)),
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_minus,N_minus),new Vector3(0, N_minus,0),  new Vector2(1,0)),
                        new VertexPositionNormalTexture(new Vector3(N_minus,N_minus, N_plus),new Vector3(0,N_minus, 0),  new Vector2(0,1)),
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_minus,N_minus),new Vector3(0, N_minus,0),  new Vector2(1,0)),
                        new VertexPositionNormalTexture(new Vector3(N_minus,N_minus,N_minus),new Vector3(0,N_minus,0),  new Vector2(0,0)),
                        new VertexPositionNormalTexture(new Vector3(N_minus,N_minus, N_plus), new Vector3(0,N_minus, 0),  new Vector2(0,1)), // Bottom
                        
                        //פנים
                        //new VertexPositionNormalTexture(new Vector3(N_minus,N_minus, N_plus), new Vector3(0,N_minus, 0),  new Vector2(0,1)), // Bottom
                        //new VertexPositionNormalTexture(new Vector3(N_minus,N_minus,N_minus),new Vector3(0,N_minus,0),  new Vector2(0,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_minus,N_minus),new Vector3(0, N_minus,0),  new Vector2(1,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_minus,N_minus, N_plus),new Vector3(0,N_minus, 0),  new Vector2(0,1)),
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_minus,N_minus),new Vector3(0, N_minus,0),  new Vector2(1,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_minus, N_plus),new Vector3(0, N_minus, 0),  new Vector2(1,1)),
                    
                        new VertexPositionNormalTexture(new Vector3(N_minus,N_minus, N_plus),new Vector3(N_minus,0, 0),  new Vector2(0,1)), // Left
                        new VertexPositionNormalTexture(new Vector3(N_minus, N_minus, N_minus),new Vector3(N_minus, 0,0),  new Vector2(0,0)),
                        new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_minus),new Vector3(N_minus, 0, 0),  new Vector2(1,0)),
                        new VertexPositionNormalTexture(new Vector3(N_minus, N_minus, N_plus),new Vector3(N_minus, 0, 0),  new Vector2(0,1)),
                        new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_minus),new Vector3(N_minus, 0, 0),  new Vector2(1,0)),
                        new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_plus),new Vector3(N_minus, 0, 0),  new Vector2(1,1)),
                        
                        //new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_plus),new Vector3(N_minus, 0, 0),  new Vector2(1,1)),
                        //new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_minus),new Vector3(N_minus, 0, 0),  new Vector2(1,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_minus, N_minus, N_plus),new Vector3(N_minus, 0, 0),  new Vector2(0,1)),
                        //new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_minus),new Vector3(N_minus, 0, 0),  new Vector2(1,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_minus, N_minus, N_minus),new Vector3(N_minus, 0,0),  new Vector2(0,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_minus,N_minus, N_plus),new Vector3(N_minus,0, 0),  new Vector2(0,1)), // Left
                        
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_plus),new Vector3(N_plus, 0, 0), new Vector2(1,1)),
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_minus),new Vector3(N_plus, 0, 0), new Vector2(1,0)),
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_minus, N_plus),new Vector3(N_plus, 0, 0), new Vector2(0,1)),
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_minus),new Vector3(N_plus, 0,0),  new Vector2(1,0)),
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_minus, N_minus),new Vector3(N_plus, 0, 0), new Vector2(0,0)),
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_minus, N_plus),new Vector3(N_plus, 0, 0),  new Vector2(0,1)), // Right

                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_minus, N_plus),new Vector3(N_plus, 0, 0),  new Vector2(0,1)), // Right
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_minus, N_minus),new Vector3(N_plus, 0, 0), new Vector2(0,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_minus),new Vector3(N_plus, 0,0),  new Vector2(1,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_minus, N_plus),new Vector3(N_plus, 0, 0), new Vector2(0,1)),
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_minus),new Vector3(N_plus, 0, 0), new Vector2(1,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_plus),new Vector3(N_plus, 0, 0), new Vector2(1,1)),
                        
                      
                   
                    };

            listVertexCubeInternal = new List<VertexPositionNormalTexture>()  {
      
                        //new VertexPositionNormalTexture(new Vector3(N_minus, N_minus, N_plus),new Vector3(0,0, N_plus),  new Vector2(0,1)), // Back
                        //new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_plus),new Vector3(0, 0, N_plus),  new Vector2(0,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_plus),new Vector3(0, 0, N_plus),  new Vector2(1,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_minus, N_minus, N_plus),new Vector3(0, 0, N_plus),  new Vector2(0,1)),
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_plus),new Vector3(0, 0, N_plus),  new Vector2(1,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_minus, N_plus),new Vector3(0, 0, N_plus),  new Vector2(1,1)),
                        
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_plus),new Vector3(0, 0, N_plus),  new Vector2(1,0)), // Back
                        new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_plus),new Vector3(0, 0, N_plus),  new Vector2(0,0)),
                        new VertexPositionNormalTexture(new Vector3(N_minus, N_minus, N_plus),new Vector3(0,0, N_plus),  new Vector2(0,1)),
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_minus, N_plus),new Vector3(0, 0, N_plus),  new Vector2(1,1)),
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_plus),new Vector3(0, 0, N_plus),  new Vector2(1,0)),
                        new VertexPositionNormalTexture(new Vector3(N_minus, N_minus, N_plus),new Vector3(0, 0, N_plus),  new Vector2(0,1)),
                        
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_minus,N_minus),new Vector3(0, 0,N_minus),  new Vector2(1,1)),// Front קדימה
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_plus,N_minus),new Vector3(0, 0, N_minus),  new Vector2(1,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_minus, N_minus, N_minus),new Vector3(0,0,N_minus),  new Vector2(0,1)),
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_minus),new Vector3(0,0,N_minus),  new Vector2(1,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_minus),new Vector3(0, 0, N_minus),  new Vector2(0,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_minus, N_minus,N_minus),new Vector3(0,0,N_minus),  new Vector2(0,1)),
                        //Pנים
                        new VertexPositionNormalTexture(new Vector3(N_minus, N_minus,N_minus),new Vector3(0,0,N_minus),  new Vector2(0,1)),// Front
                        new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_minus),new Vector3(0, 0, N_minus),  new Vector2(0,0)),
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_minus),new Vector3(0,0,N_minus),  new Vector2(1,0)),
                        new VertexPositionNormalTexture(new Vector3(N_minus, N_minus, N_minus),new Vector3(0,0,N_minus),  new Vector2(0,1)),
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_plus,N_minus),new Vector3(0, 0, N_minus),  new Vector2(1,0)),
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_minus,N_minus),new Vector3(0, 0,N_minus),  new Vector2(1,1)),

                                              
                        //new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_plus),new Vector3(0, N_plus, 0),  new Vector2(0,1)), // Top
                        //new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_minus),new Vector3(0, N_plus,0),  new Vector2(0,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_minus),new Vector3(0, N_plus, 0),  new Vector2(1,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_plus),new Vector3(0, N_plus, 0),  new Vector2(0,1)),
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_minus),new Vector3(0, N_plus, 0),  new Vector2(1,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_plus),new Vector3(0, N_plus, 0),  new Vector2(1,1)),
                                                
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_plus),new Vector3(0, N_plus, 0),  new Vector2(1,1)),
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_minus),new Vector3(0, N_plus, 0),  new Vector2(1,0)),
                        new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_plus),new Vector3(0, N_plus, 0),  new Vector2(0,1)),
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_minus),new Vector3(0, N_plus, 0),  new Vector2(1,0)),
                        new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_minus),new Vector3(0, N_plus,0),  new Vector2(0,0)),
                        new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_plus),new Vector3(0, N_plus, 0),  new Vector2(0,1)), // Top
                        
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_minus, N_plus),new Vector3(0, N_minus, 0),  new Vector2(1,1)),
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_minus,N_minus),new Vector3(0, N_minus,0),  new Vector2(1,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_minus,N_minus, N_plus),new Vector3(0,N_minus, 0),  new Vector2(0,1)),
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_minus,N_minus),new Vector3(0, N_minus,0),  new Vector2(1,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_minus,N_minus,N_minus),new Vector3(0,N_minus,0),  new Vector2(0,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_minus,N_minus, N_plus), new Vector3(0,N_minus, 0),  new Vector2(0,1)), // Bottom
                        
                        //פנים
                        new VertexPositionNormalTexture(new Vector3(N_minus,N_minus, N_plus), new Vector3(0,N_minus, 0),  new Vector2(0,1)), // Bottom
                        new VertexPositionNormalTexture(new Vector3(N_minus,N_minus,N_minus),new Vector3(0,N_minus,0),  new Vector2(0,0)),
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_minus,N_minus),new Vector3(0, N_minus,0),  new Vector2(1,0)),
                        new VertexPositionNormalTexture(new Vector3(N_minus,N_minus, N_plus),new Vector3(0,N_minus, 0),  new Vector2(0,1)),
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_minus,N_minus),new Vector3(0, N_minus,0),  new Vector2(1,0)),
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_minus, N_plus),new Vector3(0, N_minus, 0),  new Vector2(1,1)),
                    
                        //new VertexPositionNormalTexture(new Vector3(N_minus,N_minus, N_plus),new Vector3(N_minus,0, 0),  new Vector2(0,1)), // Left
                        //new VertexPositionNormalTexture(new Vector3(N_minus, N_minus, N_minus),new Vector3(N_minus, 0,0),  new Vector2(0,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_minus),new Vector3(N_minus, 0, 0),  new Vector2(1,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_minus, N_minus, N_plus),new Vector3(N_minus, 0, 0),  new Vector2(0,1)),
                        //new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_minus),new Vector3(N_minus, 0, 0),  new Vector2(1,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_plus),new Vector3(N_minus, 0, 0),  new Vector2(1,1)),
                        
                        new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_plus),new Vector3(N_minus, 0, 0),  new Vector2(1,1)),
                        new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_minus),new Vector3(N_minus, 0, 0),  new Vector2(1,0)),
                        new VertexPositionNormalTexture(new Vector3(N_minus, N_minus, N_plus),new Vector3(N_minus, 0, 0),  new Vector2(0,1)),
                        new VertexPositionNormalTexture(new Vector3(N_minus, N_plus, N_minus),new Vector3(N_minus, 0, 0),  new Vector2(1,0)),
                        new VertexPositionNormalTexture(new Vector3(N_minus, N_minus, N_minus),new Vector3(N_minus, 0,0),  new Vector2(0,0)),
                        new VertexPositionNormalTexture(new Vector3(N_minus,N_minus, N_plus),new Vector3(N_minus,0, 0),  new Vector2(0,1)), // Left
                        
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_plus),new Vector3(N_plus, 0, 0), new Vector2(1,1)),
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_minus),new Vector3(N_plus, 0, 0), new Vector2(1,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_minus, N_plus),new Vector3(N_plus, 0, 0), new Vector2(0,1)),
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_minus),new Vector3(N_plus, 0,0),  new Vector2(1,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_minus, N_minus),new Vector3(N_plus, 0, 0), new Vector2(0,0)),
                        //new VertexPositionNormalTexture(new Vector3(N_plus, N_minus, N_plus),new Vector3(N_plus, 0, 0),  new Vector2(0,1)), // Right

                        new VertexPositionNormalTexture(new Vector3(N_plus, N_minus, N_plus),new Vector3(N_plus, 0, 0),  new Vector2(0,1)), // Right
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_minus, N_minus),new Vector3(N_plus, 0, 0), new Vector2(0,0)),
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_minus),new Vector3(N_plus, 0,0),  new Vector2(1,0)),
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_minus, N_plus),new Vector3(N_plus, 0, 0), new Vector2(0,1)),
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_minus),new Vector3(N_plus, 0, 0), new Vector2(1,0)),
                        new VertexPositionNormalTexture(new Vector3(N_plus, N_plus, N_plus),new Vector3(N_plus, 0, 0), new Vector2(1,1)),
                        
                      
                   
                    };
            #endregion
            depthDisabledStencilDesc = new SharpDX.Direct3D11.DepthStencilStateDescription()
           {
               IsDepthEnabled = false,
               DepthWriteMask = SharpDX.Direct3D11.DepthWriteMask.All,
               DepthComparison = SharpDX.Direct3D11.Comparison.Less,
               IsStencilEnabled = true,
               StencilReadMask = 0xFF,
               StencilWriteMask = 0xFF,
               // Stencil operation if pixel front-facing.
               FrontFace = new SharpDX.Direct3D11.DepthStencilOperationDescription()
               {
                   FailOperation = SharpDX.Direct3D11.StencilOperation.Keep,
                   DepthFailOperation = SharpDX.Direct3D11.StencilOperation.Increment,
                   PassOperation = SharpDX.Direct3D11.StencilOperation.Keep,
                   Comparison = SharpDX.Direct3D11.Comparison.Always
               },
               // Stencil operation if pixel is back-facing.
               BackFace = new SharpDX.Direct3D11.DepthStencilOperationDescription()
               {
                   FailOperation = SharpDX.Direct3D11.StencilOperation.Keep,
                   DepthFailOperation = SharpDX.Direct3D11.StencilOperation.Decrement,
                   PassOperation = SharpDX.Direct3D11.StencilOperation.Keep,
                   Comparison = SharpDX.Direct3D11.Comparison.Always
               }
           };
            base.LoadContent();


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            base.UnloadContent();
            //GC.Collect();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        MouseState prev;
        protected override void Update(GameTime gameTime)
        {
            if (EndPage)
            {
                var time = (float)gameTime.TotalGameTime.TotalSeconds;
                world = Matrix.RotationX(time) * Matrix.RotationY(time * 2.0f) * Matrix.RotationZ(time * .7f);
                basicEffectLines.Projection = Matrix.PerspectiveFovRH((float)Math.PI / 4.0f, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);
                basicEffectCursor.Projection = Matrix.PerspectiveFovRH((float)Math.PI / 4.0f, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);
                basicEffectCube.Projection = Matrix.PerspectiveFovRH((float)Math.PI / 4.0f, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);


            }
            else
            {
                //סיבוב הקוביה ע"פ תנועת העכבר
                mouseState = mouseManager.GetState();
                if (mouseState.LeftButton.Down)
                {
                    if (!mouseState.LeftButton.Pressed)
                    {
                        //סיבוב ע"פ מיקום נוכחי- מיקום קודם
                        var time = (float)gameTime.TotalGameTime.TotalSeconds;
                        var roX = Matrix.RotationX((float)((mouseState.Y - prev.Y) * Math.PI));
                        var roY = Matrix.RotationY((float)((mouseState.X - prev.X) * Math.PI));
                        world *= Matrix.RotationX((float)((mouseState.Y - prev.Y) * Math.PI)) * Matrix.RotationY((float)((mouseState.X - prev.X) * Math.PI));//* Matrix.RotationZ(mouseState.Z - prev.Z);
                        Global.World = world;
                    }
                    prev = mouseState;
                }
                else
                    world = Global.World;
            }

            //הצבה באפקטים של כל החלקים
            basicEffectLines.World = world;
            basicEffectCursor.World = world;
            basicEffectCube.World = world;
            // עדכון צבע הסמן ע"פ הפעולה הנוכחית
            if (ManagerGame.CommandCube != null)
            {
                if (ManagerGame.CommandCube == ManagerGame.Movement)
                    basicEffectCursor.DiffuseColor = new Vector4(0.4f, 1, 0.1f, 0.1f);
                if (ManagerGame.CommandCube == ManagerGame.DeleteLine)
                    basicEffectCursor.DiffuseColor = new Vector4(0.1f, 0.1f, 0.8f, 0.01f);
                if (ManagerGame.CommandCube == ManagerGame.CreateLine)
                    basicEffectCursor.DiffuseColor = new Vector4(1, 0.2f, 0.2f, 0.5f);
                //basicEffectCursor.EnableDefaultLighting();
                //basicEffectCursor.LightingEnabled = true;
                basicEffectCursor.AmbientLightColor = new Vector3(0f, 0.0f, 0.0f);

                //basicEffectCursor.SpecularColor = new Vector3(0.7f, 0.99f, 0.8f);
                //basicEffectCursor.SpecularPower = 0.99f;
            }
            base.Update(gameTime);

        }
        public Color color = Color.SmoothStep(Color.WhiteSmoke, Color.White, 0.4f);
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //    GraphicsDevice.Clear(Color.FromAbgr(4283585279));
            //GraphicsDevice.Clear(Color.SmoothStep(Color.OrangeRed, Color.Snow, 0.8f));
          GraphicsDevice.Clear(color);/////////////////////////////////////////////////////////////
            //GraphicsDevice.Clear();



            //אוסף הנקודות המרכיבות את הקוביה
            // listVertexCube
            //   ToDisposeContent(vertices);

            var blendStateDescription = new SharpDX.Direct3D11.BlendStateDescription();
            blendStateDescription.AlphaToCoverageEnable = false;

            blendStateDescription.RenderTarget[0].IsBlendEnabled = true;
            blendStateDescription.RenderTarget[0].SourceBlend = SharpDX.Direct3D11.BlendOption.SourceAlpha;
            blendStateDescription.RenderTarget[0].DestinationBlend = SharpDX.Direct3D11.BlendOption.InverseSourceAlpha;
            blendStateDescription.RenderTarget[0].BlendOperation = SharpDX.Direct3D11.BlendOperation.Add;
            blendStateDescription.RenderTarget[0].SourceAlphaBlend = SharpDX.Direct3D11.BlendOption.Zero;
            blendStateDescription.RenderTarget[0].DestinationAlphaBlend = SharpDX.Direct3D11.BlendOption.Zero;
            blendStateDescription.RenderTarget[0].AlphaBlendOperation = SharpDX.Direct3D11.BlendOperation.Add;
            blendStateDescription.RenderTarget[0].RenderTargetWriteMask = SharpDX.Direct3D11.ColorWriteMaskFlags.All;

            var blendState = ToDisposeContent(SharpDX.Toolkit.Graphics.BlendState.New(this.GraphicsDevice, blendStateDescription));
            this.GraphicsDevice.SetBlendState(blendState);


            // Create the depth stencil state.
            var depthDisabledStencilState = ToDisposeContent(SharpDX.Toolkit.Graphics.DepthStencilState.New(this.GraphicsDevice, depthDisabledStencilDesc));
            this.GraphicsDevice.SetDepthStencilState(depthDisabledStencilState, 1);
            basicEffectCube.Alpha = 0.9f;


            DrawList(listVertexCubeInternal, basicEffectCube);

            this.GraphicsDevice.SetBlendState(null);
            this.GraphicsDevice.SetDepthStencilState(null);

            //List<VertexPositionNormalTexture>
            // listVertexLines 
            var matrixToDraw = ManagerGame.Type ? ManagerGame.matrixSystem : ManagerGame.matrixUser;

            //המרת הקוביה לאוסף של צורות
            //וממנו לאוסף של VertexPositionNormalTexture

            if (matrixToDraw.ToListShape().Count > 0)
            {
            //    if (listVertexLines.Count == 0 || matrixToDraw.change == true)
              //  {
                    listVertexLines.Clear();
                    foreach (var item in matrixToDraw.ToListShape())
                    {
                        if (item.Draw() != null)
                            listVertexLines.AddRange(item.Draw());
               }

                    matrixToDraw.change = false;
         //       }
                //ציור הנקודות ע"פ האם=פקט הרצוי המוגד למעלה
                DrawList(AddToPoints(listVertexLines), basicEffectLines);
            }
            //ציור הסמן ע"פ האפקט הרצוי
            if (!ManagerGame.Type)
            {
                List<VertexPositionNormalTexture> listVertexCursor = new List<VertexPositionNormalTexture>();
                listVertexCursor.AddRange(ManagerGame.cursor.Draw());
                DrawList(AddToPoints(listVertexCursor), basicEffectCursor);
            }


            this.GraphicsDevice.SetBlendState(blendState);

            this.GraphicsDevice.SetDepthStencilState(depthDisabledStencilState, 1);
            basicEffectCube.Alpha = 0.5f;

            DrawList(listVertexCubeExterior, basicEffectCube);
            base.Draw(gameTime);
            // מחיקת האובייקטים שכבר השתמשנו בהם ניקוי הזכרון
          //  this.UnloadContent();
        }
        /// <summary>
        /// מעדכנת את אוסף הנקודות מוסיפה ערך לכל פרמטר 
        /// </summary>
        /// <param name="listVertexLines"></param>
        /// <returns></returns>
        private List<VertexPositionNormalTexture> AddToPoints(List<VertexPositionNormalTexture> listVertexLines)
        {
            int move = -5;
            List<VertexPositionNormalTexture> temp = new List<VertexPositionNormalTexture>();

            foreach (var b in listVertexLines)
            {
                temp.Add(new VertexPositionNormalTexture(new Vector3(b.Position.X + move, b.Position.Y + move, b.Position.Z + move), new Vector3(b.Normal.X, b.Normal.Y, b.Normal.Z), new Vector2(b.TextureCoordinate.X, b.TextureCoordinate.Y)));
            }
            return temp;
        }
        /// <summary>
        /// צילום מסך של המוט התל מימדי לצורך שמירת משחק והצגת התמונה למשתמש
        /// </summary>
        /// <returns></returns>
        public Image SaveAsImage()
        {
            var screenViewport = new ViewportF(0f, 0f, GraphicsDevice.BackBuffer.Width, GraphicsDevice.BackBuffer.Height);
            var targetWidth = (int)(screenViewport.Width / 8f);
            var targetHeight = (int)(screenViewport.Height / 8f);
            var screenshot = ToDispose(SharpDX.Toolkit.Graphics.RenderTarget2D.New(GraphicsDevice,
                                                                                   targetWidth,
                                                                                   targetHeight,
                                                                                   SharpDX.Toolkit.Graphics.PixelFormat.R8G8B8A8.UNorm));

            GraphicsDevice.SetRenderTargets(screenshot);

            GraphicsDevice.Clear(Color.SmoothStep(Color.OrangeRed, Color.Snow, 0.8f));

            this.GraphicsDevice.SetBlendState(null);
            this.GraphicsDevice.SetDepthStencilState(null);
            List<VertexPositionNormalTexture> listVertexLines = new List<VertexPositionNormalTexture>();
            var matrixToDraw = ManagerGame.matrixUser;
            //המרת הקוביה לאוסף של צורות
            //וממנו לאוסף של VertexPositionNormalTexture
            if (matrixToDraw.ToListShape().Count > 0)
            {
                foreach (var item in matrixToDraw.ToListShape())
                {
                    if (item.Draw() != null)
                        listVertexLines.AddRange(item.Draw());
                }
                //ציור הנקודות ע"פ האם=פקט הרצוי המוגד למעלה
                DrawList(AddToPoints(listVertexLines), basicEffectLines);
            }
            //ציור הסמן ע"פ האפקט הרצוי
            List<VertexPositionNormalTexture> listVertexCursor = new List<VertexPositionNormalTexture>();
            listVertexCursor.AddRange(ManagerGame.cursor.Draw());
            DrawList(AddToPoints(listVertexCursor), basicEffectCursor);
            GraphicsDevice.SetRenderTargets();
            return screenshot.GetDataAsImage();
        }
        /// <summary>
        /// בונה מבנה תלת מימדי ע."פ נקודות ואפקט נתונים
        /// </summary>
        /// <param name="listVertexPositionNormalTexture">אוסך נקודות היוצרות את המבנה</param>
        /// <param name="basicEffectCurrent">אפקטשיוחל על המבנה</param>
        private async void DrawList(List<VertexPositionNormalTexture> listVertexPositionNormalTexture, BasicEffect basicEffectCurrent)
        {
            vertices = ToDisposeContent(SharpDX.Toolkit.Graphics.Buffer.Vertex.New(
                 GraphicsDevice, listVertexPositionNormalTexture.ToArray()));
            ToDisposeContent(vertices);

            // Create an input layout from the vertices
            inputLayout = VertexInputLayout.FromBuffer(0, vertices);

            GraphicsDevice.SetVertexBuffer(vertices);
            GraphicsDevice.SetVertexInputLayout(inputLayout);

            foreach (EffectPass pass in basicEffectCurrent.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.Draw(PrimitiveType.TriangleList, vertices.ElementCount);
            }
            await Task.Delay(100);
            this.UnloadContent();

        }

        public SharpDX.Direct3D11.DepthStencilStateDescription depthDisabledStencilDesc { get; set; }
    }
}
