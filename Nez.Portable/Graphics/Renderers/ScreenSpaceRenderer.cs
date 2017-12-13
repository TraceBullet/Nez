using System;


namespace Nez
{
    /// <summary>
    /// Renderer that renders using its own Camera which doesnt move.
    /// </summary>
    public class ScreenSpaceRenderer : Renderer
    {
        public int[] renderLayers;


        public ScreenSpaceRenderer(Camera camera, int renderOrder, params int[] renderLayers) : base(renderOrder, null)
        {
            this.camera = camera;
            Array.Sort(renderLayers);
            Array.Reverse(renderLayers);
            this.renderLayers = renderLayers;
            wantsToRenderAfterPostProcessors = true;
        }

        public override void render(Scene scene)
        {
            beginRender(camera);

            for (var i = 0; i < renderLayers.Length; i++)
            {
                var renderables = scene.renderableComponents.componentsWithRenderLayer(renderLayers[i]);
                for (var j = 0; j < renderables.length; j++)
                {
                    var renderable = renderables.buffer[j];
                    if (renderable.enabled && renderable.isVisibleFromCamera(camera))
                        renderAfterStateCheck(renderable, camera);
                }
            }

            if (shouldDebugRender && Core.debugRenderEnabled)
                debugRender(scene, camera);

            endRender();
        }
    }
}
