using Microsoft.Xna.Framework.Graphics;

namespace TapsasEngine.Models
{
    public abstract class Shader : Effect
    {
        public string Path { get; private set; }

        public Shader(string path)
            : base(Tengine.Content.Load<Effect>(path))
        {
            Path = path;
        }
        
        public virtual void Update() {}

        public override string ToString()
        {
            return Path;
        }
    }
}