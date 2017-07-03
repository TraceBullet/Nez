using Microsoft.Xna.Framework.Graphics;

namespace Nez
{
	public class PaletteCyclerMaterial : Material<PaletteCyclerEffect>
	{
		public PaletteCyclerMaterial(Effect e)
		{
			effect = new PaletteCyclerEffect(e);
		}


		public override void onPreRender( Camera camera )
		{
			effect.updateTime();
		}

	}
}

