using System;
using System.IO;
using Microsoft.Xna.Framework;


namespace Nez
{
	public static class EffectResource
	{
		// sprite effects
		internal static byte[] spriteBlinkEffectBytes { get { return getFileResourceBytes( "Content/nez/effects/SpriteBlinkEffect.mgfx" ); } }
		internal static byte[] spriteLinesEffectBytes { get { return getFileResourceBytes( "Content/nez/effects/SpriteLines.mgfx" ); } }
		internal static byte[] spriteAlphaTestBytes { get { return getFileResourceBytes( "Content/nez/effects/SpriteAlphaTest.mgfx" ); } }
		internal static byte[] crosshatchBytes { get { return getFileResourceBytes( "Content/nez/effects/Crosshatch.mgfx" ); } }
		internal static byte[] noiseBytes { get { return getFileResourceBytes( "Content/nez/effects/Noise.mgfx" ); } }
		internal static byte[] twistBytes { get { return getFileResourceBytes( "Content/nez/effects/Twist.mgfx" ); } }
		internal static byte[] dotsBytes { get { return getFileResourceBytes( "Content/nez/effects/Dots.mgfx" ); } }
		internal static byte[] dissolveBytes { get { return getFileResourceBytes( "Content/nez/effects/Dissolve.mgfx" ); } }

		// post processor effects
		internal static byte[] bloomCombineBytes { get { return getFileResourceBytes( "Content/nez/effects/BloomCombine.mgfx" ); } }
		internal static byte[] bloomExtractBytes { get { return getFileResourceBytes( "Content/nez/effects/BloomExtract.mgfx" ); } }
		internal static byte[] gaussianBlurBytes { get { return getFileResourceBytes( "Content/nez/effects/GaussianBlur.mgfx" ); } }
		internal static byte[] vignetteBytes { get { return getFileResourceBytes( "Content/nez/effects/Vignette.mgfx" ); } }
		internal static byte[] letterboxBytes { get { return getFileResourceBytes( "Content/nez/effects/Letterbox.mgfx" ); } }
		internal static byte[] heatDistortionBytes { get { return getFileResourceBytes( "Content/nez/effects/HeatDistortion.mgfx" ); } }
		internal static byte[] spriteLightMultiplyBytes { get { return getFileResourceBytes( "Content/nez/effects/SpriteLightMultiply.mgfx" ); } }
		internal static byte[] pixelGlitchBytes { get { return getFileResourceBytes( "Content/nez/effects/PixelGlitch.mgfx" ); } }

		// deferred lighting
		internal static byte[] deferredSpriteBytes { get { return getFileResourceBytes( "Content/nez/effects/DeferredSprite.mgfx" ); } }
		internal static byte[] deferredLightBytes { get { return getFileResourceBytes( "Content/nez/effects/DeferredLighting.mgfx" ); } }

		// forward lighting
		internal static byte[] forwardLightingBytes { get { return getFileResourceBytes( "Content/nez/effects/ForwardLighting.mgfx" ); } }
		internal static byte[] polygonLightBytes { get { return getFileResourceBytes( "Content/nez/effects/PolygonLight.mgfx" ); } }

		// scene transitions
		internal static byte[] squaresTransitionBytes { get { return getFileResourceBytes( "Content/nez/effects/transitions/Squares.mgfx" ); } }

		// sprite or post processor effects
		internal static byte[] spriteEffectBytes { get { return getMonoGameEmbeddedResourceBytes( "Microsoft.Xna.Framework.Graphics.Effect.Resources.SpriteEffect.ogl.mgfxo" ); } }
		internal static byte[] multiTextureOverlayBytes { get { return getFileResourceBytes( "Content/nez/effects/MultiTextureOverlay.mgfx" ); } }
		internal static byte[] scanlinesBytes { get { return getFileResourceBytes( "Content/nez/effects/Scanlines.mgfx" ); } }
		internal static byte[] reflectionBytes { get { return getFileResourceBytes( "Content/nez/effects/Reflection.mgfx" ); } }
		internal static byte[] grayscaleBytes { get { return getFileResourceBytes( "Content/nez/effects/Grayscale.mgfx" ); } }
		internal static byte[] sepiaBytes { get { return getFileResourceBytes( "Content/nez/effects/Sepia.mgfx" ); } }
		internal static byte[] paletteCyclerBytes { get { return getFileResourceBytes( "Content/nez/effects/PaletteCycler.mgfx" ); } }


		/// <summary>
		/// gets the raw byte[] from an EmbeddedResource
		/// </summary>
		/// <returns>The embedded resource bytes.</returns>
		/// <param name="name">Name.</param>
		static byte[] getEmbeddedResourceBytes( string name )
		{
			var assembly = ReflectionUtils.getAssembly( typeof( EffectResource ) );
			using( var stream = assembly.GetManifestResourceStream( name ) )
			{
				using( var ms = new MemoryStream() )
				{
					stream.CopyTo( ms );
					return ms.ToArray();
				}
			}
		}


		internal static byte[] getMonoGameEmbeddedResourceBytes( string name )
		{
			#if FNA
			name = name.Replace( ".ogl.mgfxo", ".fxb" );
			#endif

			var assembly = ReflectionUtils.getAssembly( typeof( MathHelper ) );
			using( var stream = assembly.GetManifestResourceStream( name ) )
			{
				using( var ms = new MemoryStream() )
				{
					stream.CopyTo( ms );
					return ms.ToArray();
				}
			}
		}


		/// <summary>
		/// fetches the raw byte data of a file from the Content folder. Used to keep the Effect subclass code simple and clean due to the Effect
		/// constructor requiring the byte[].
		/// </summary>
		/// <returns>The file resource bytes.</returns>
		/// <param name="path">Path.</param>
		public static byte[] getFileResourceBytes( string path )
		{
			#if FNA
			path = path.Replace( ".mgfx", ".fxb" );
			#endif

			byte[] bytes;
			try
			{
				using( var stream = TitleContainer.OpenStream( path ) )
				{
					bytes = new byte[stream.Length];
					stream.Read( bytes, 0, bytes.Length );
				}
			}
			catch( Exception e )
			{
				var txt = string.Format( "OpenStream failed to find file at path: {0}. Did you add it to the Content folder?", path );
				throw new Exception( txt, e );
			}

			return bytes;
		}
	}
}

