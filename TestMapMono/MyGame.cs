using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace TestMapMono
{

    public class MyGame : Game
    {
        public static MyGame Instance;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Map map;
        List<Texture2D> textures;
        List<SpriteFont> fonts;
        KeyboardState oldState;
        Camera2D camera;
        
        public MyGame()
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 800;
            graphics.ApplyChanges();
            // TODO: Add your initialization logic here
            Random rand = new Random();
            map = new Map(rand.Next(1, 20), rand.Next(1, 20));
            camera = new Camera2D(graphics.GraphicsDevice.Viewport);
            textures = new List<Texture2D>();
            fonts = new List<SpriteFont>();
            this.IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textures.Add(this.Content.Load<Texture2D>("door0"));
            textures.Add(this.Content.Load<Texture2D>("door1"));
            textures.Add(this.Content.Load<Texture2D>("door2"));
            textures.Add(this.Content.Load<Texture2D>("door3"));
            textures.Add(this.Content.Load<Texture2D>("room"));
            fonts.Add(this.Content.Load<SpriteFont>("roomNumber_8"));
            fonts.Add(this.Content.Load<SpriteFont>("roomNumber_12"));
            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            // new maps
            KeyboardState newState = Keyboard.GetState();
            if (oldState.IsKeyUp(Keys.Space) && newState.IsKeyDown(Keys.Space)) {
                Random rand = new Random();
                map = null;
                map = new Map(rand.Next(1, 20), rand.Next(1, 20));
            }
            oldState = newState;
            if (Mouse.GetState().LeftButton == ButtonState.Pressed) {
                Random rand = new Random();
                map = null;
                map = new Map(rand.Next(1, 20), rand.Next(1, 20));
            }

            // camera
            camera.Update(new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2));
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                camera.Zoom += 0.01f;
            else if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                camera.Zoom -= 0.01f;

            // move movement
            float mapSpeed = 5;
            if (Keyboard.GetState().IsKeyDown(Keys.Z))
                map.Y -= mapSpeed;
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
                map.Y += mapSpeed;
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
                map.X -= mapSpeed;
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
                map.X += mapSpeed;
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                null, null, null, null,
                camera.Transform);
            map.Draw(spriteBatch, textures, fonts);
            spriteBatch.End();
           
            base.Draw(gameTime);
        }
    }
}
