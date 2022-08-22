namespace ZA6.UI
{
    public abstract class FocusableComponent : UIComponent
    {
        public bool Disabled { get; set; }
        public virtual bool IsFocused { get; set; }
    }
}