using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsEngine
{
    class PointLight : CustomEffectModel
    {
        public class LambertPointLightMaterial : Material
        {
            public Vector3 AmbientLightColor { get; set; }
            public Vector3 LightPosition { get; set; }
            public Vector3 LightColor { get; set; }

            public float LightAttenuation { get; set; }
            public float LightFalloff { get; set; }

            public Texture2D Texture { get; set; }
            public Vector3 DiffuseColor { get; set; }

            public LambertPointLightMaterial()
            {
            }

            public LambertPointLightMaterial(Color ambientColor, Color lightColor, Vector3 lightPosition, float attenuation, float falloff)
            {
                AmbientLightColor = ambientColor.ToVector3();
                LightColor = lightColor.ToVector3();
                LightPosition = lightPosition;
                LightAttenuation = attenuation;
                LightFalloff = falloff;
            }

            public override void SetEffectParameters(Effect effect)
            {
                effect.Parameters["AmbientLightColor"].SetValue(AmbientLightColor);
                effect.Parameters["LightPosition"].SetValue(LightPosition);
                effect.Parameters["DiffuseColor"].SetValue(DiffuseColor);
                effect.Parameters["LightColor"].SetValue(LightColor);
                effect.Parameters["LightFalloff"].SetValue(LightFalloff);
                effect.Parameters["LightAttenuation"].SetValue(LightAttenuation);
                effect.Parameters["Texture"].SetValue(Texture);
            }

            public override void Update()
            {
                base.Update();
            }
        }


        public PointLight(string id, string asset, Vector3 position)
            : base (id, asset, position)
        {

        }

        public override void LoadContent()
        {
            Material = new LambertPointLightMaterial()
            {
                AmbientLightColor = Color.DarkGray.ToVector3(),
                DiffuseColor = Color.Black.ToVector3(),
                LightAttenuation = 10,
                LightFalloff = 10,
                LightColor = Color.Wheat.ToVector3(),
                LightPosition = new Vector3(10, 10, 10),
                Texture = GameUtilities.Content.Load<Texture2D>("Textures\\sand")
            };

            base.LoadContent();
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
