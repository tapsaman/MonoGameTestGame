using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTestGame
{
    public class Dialog
    {
        public string Title;
        public string[] Messages;

        public Dialog(params string[] messages)
        {
            Messages = messages;
        }
    }
}