using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TapsasEngine.Utilities
{
    public abstract class Renderer
    {
        public GraphicsDevice Device { get; private set; }
        public GraphicsDeviceManager DeviceManager { get; private set; }

        public Renderer(GraphicsDevice device, GraphicsDeviceManager deviceManager)
        {
            Device = device;
            DeviceManager = deviceManager;
        }
    }
    
}