﻿using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using System;
using System.IO;
using System.IO.Compression;


namespace Nez.ParticleDesignerImporter
{
    [ContentProcessor( DisplayName = "Particle Designer Processor" )]
	public class ParticleDesignerProcessor : ContentProcessor<ParticleDesignerContent, ParticleDesignerProcessorResult>
	{
		public static ContentBuildLogger logger;


		public override ParticleDesignerProcessorResult Process( ParticleDesignerContent input, ContentProcessorContext context )
		{
			logger = context.Logger;
			var result = new ParticleDesignerProcessorResult();

			// check for an embedded tiff texture
			if( input.emitterConfig.texture.data != null )
			{
				context.Logger.LogMessage( "pex file has an embedded tiff. Extracting now." );
				using( var memoryStream = new MemoryStream( Convert.FromBase64String( input.emitterConfig.texture.data ), writable: false ) )
				{
					using( var stream = new GZipStream( memoryStream, CompressionMode.Decompress ) )
					{
						const int size = 4096;
						byte[] buffer = new byte[size];
						using( var memory = new MemoryStream() )
						{
							int count = 0;
							do
							{
								count = stream.Read( buffer, 0, size );
								if( count > 0 )
									memory.Write( buffer, 0, count );

							} while( count > 0 );

							result.textureTiffData = memory.ToArray();
						}
					}
				}

				var tempFile = Path.Combine( Path.GetTempPath(), "tempParticleTexture.tif" );
				File.WriteAllBytes( tempFile, result.textureTiffData );
				context.Logger.LogMessage( "writing tiff to temp file: {0}", tempFile );

				context.Logger.LogMessage( "running TextureImportor on tiff" );
				var textureImporter = new TextureImporter();
				result.texture = textureImporter.Import( tempFile, input.context ) as Texture2DContent;
				result.texture.Name = input.emitterConfig.texture.name;

				context.Logger.LogMessage( "deleting temp file" );
				File.Delete( tempFile );

				// process
				context.Logger.LogMessage( "processing TextureContent" );
				var textureProcessor = new TextureProcessor
				{
					GenerateMipmaps = false,
					TextureFormat = TextureProcessorOutputFormat.Color
				};
				result.texture = (Texture2DContent)textureProcessor.Process( result.texture, context );
				context.Logger.LogMessage( "TextureContent processed" );
			}
			else // no tiff data, so let's try loading the texture with the texture name, from the same directory as the particle file
			{
				string fileDirectory = Path.GetDirectoryName( input.path );
				string fullPath = Path.Combine( fileDirectory, input.emitterConfig.texture.name );
				context.Logger.LogMessage( "Looking for texture file at {0}", fullPath );
				result.texture = context.BuildAndLoadAsset<string, Texture2DContent>( new ExternalReference<string>( fullPath ), "TextureProcessor" );
				context.Logger.LogMessage( "Texture file found" );
			}

			result.particleEmitterConfig = input.emitterConfig;

			return result;
		}

	}
}
