using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sample;
using System.Collections.Generic;
using System.Diagnostics;

namespace GraphicsEngine
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;

        InputEngine input;
        DebugEngine debug;
        ImmediateShapeDrawer shapeDrawer;

        List<SimpleModel> gameObjects = new List<SimpleModel>();
        Camera mainCamera;

        SpriteBatch spriteBatch;
        SpriteFont sfont;

        int objectsDrawn;

        OcclusionQuery occQuery;
        Stopwatch timer = new Stopwatch();
        long totalTime = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1440;
            graphics.PreferredBackBufferHeight = 800;
            graphics.ApplyChanges();

            input = new InputEngine(this);
            debug = new DebugEngine();
            shapeDrawer = new ImmediateShapeDrawer();

            IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        
        protected override void Initialize()
        {
            occQuery = new OcclusionQuery(GraphicsDevice);
            GameUtilities.Content = Content;
            GameUtilities.GraphicsDevice = GraphicsDevice;
            debug.Initialize();
            shapeDrawer.Initialize();

            mainCamera = new Camera("cam", new Vector3(0, 5, 10), new Vector3(0, 0, -1));
            mainCamera.Initialize();

            base.Initialize();
        }

        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sfont = Content.Load<SpriteFont>("debug");

            AddModel(new SimpleModel("wall0", "wall", new Vector3(0, 0, -10)));
            AddModel(new SimpleModel("ball0", "ball", new Vector3(0, 2.5f, -20)));
        }

        
        protected override void UnloadContent()
        {
        }

        
        protected override void Update(GameTime gameTime)
        {
            GameUtilities.Time = gameTime;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mainCamera.Update();

            gameObjects.ForEach(go => go.Update());
            

            base.Update(gameTime);
        }

        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            objectsDrawn = 0;
            totalTime = 0;
            timer.Reset();
            foreach (var go in gameObjects)
            {
                if (FustrumContains(go) && !IsOccluded(go))
                {
                    go.Draw(mainCamera);
                    objectsDrawn++;
                }
            }
            spriteBatch.Begin();
            spriteBatch.DrawString(sfont, objectsDrawn + " objects drawn", new Vector2(20, 20), Color.Red);
            spriteBatch.DrawString(sfont, totalTime + " total time", new Vector2(20, 50), Color.Red);
            spriteBatch.End();

            GameUtilities.SetGraphicsDeviceFor3D();
            base.Draw(gameTime);
        }

        public void AddModel(SimpleModel model)
        {
            model.Initialize();
            model.LoadContent();
            gameObjects.Add(model);
        }

        public bool FustrumContains(SimpleModel model)
        {
            if (mainCamera.Frustum.Contains(model.AABB) != ContainmentType.Disjoint)
                return true;
            return false;
        }

        public bool IsOccluded(SimpleModel model)
        {
            timer.Start();
            occQuery.Begin();
            shapeDrawer.DrawBoundingBox(model.AABB, mainCamera);
            occQuery.End();
            timer.Stop();
            totalTime += timer.ElapsedMilliseconds;

            if (occQuery.IsComplete && occQuery.PixelCount>0)
            {
                return true;
            }
            return false;
        }
    }
}
