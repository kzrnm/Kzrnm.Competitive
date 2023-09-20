namespace Kzrnm.Competitive.Testing
{
    public class HeavyFactAttribute : FactAttribute
    {
#if LIBRARY
        public override string Skip => "重いので";
#endif
    }
}
