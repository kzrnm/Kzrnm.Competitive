namespace Kzrnm.Competitive.Testing
{
    public class HeavyFactAttribute : FactAttribute
    {
#if LIBRARY
        public HeavyFactAttribute()
        {
            Skip = "重いので";
        }
#endif
    }
}
