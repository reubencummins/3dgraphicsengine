using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sample;

namespace GraphicsEngine
{
    class DirectionalLight : CustomEffectModel
    {
        public class LambertDirectionalLightMaterial : Material
        {
            public Vector3 AmbientColor { get; set; }
            public Vector3 DiffuseColor { get; set; }
            public Vector3 LightColor { get; set; }
            public Vector3 LightDirection { get; set; }
            public Texture2D Texture { get; set; }

            public LambertDirectionalLightMaterial()
            {
                
            }

            public override void SetEffectParameters(Effect effect)
            {
                effect.Parameters["AmbientColor"].SetValue(AmbientColor);
                effect.Parameters["DiffuseColor"].SetValue(DiffuseColor);
                effect.Parameters["LightColor"].SetValue(LightColor);
                effect.Parameters["LightDirection"].SetValue(LightDirection);
                effect.Parameters["Texture"].SetValue(Texture);
            }
            
        }

        public DirectionalLight(string id, string asset, Vector3 position)
            :base(id, asset, position)
        {

        }

        public override void LoadContent()
        {
            Material = new LambertDirectionalLightMaterial()
            {
                Texture = GameUtilities.Content.Load<Texture2D>("Textures\\sand"),
                LightColor = Color.White.ToVector3(),
                LightDirection = new Vector3(1, 1, 0),
                AmbientColor = Color.DarkGray.ToVector3()
            };

            CustomEffect = GameUtilities.Content.Load<Effect>("Effects\\EmptyEffect");

            base.LoadContent();
        }


    }
}
