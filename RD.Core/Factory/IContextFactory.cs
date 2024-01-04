using RD.Core.EFContext;

namespace RD.Core.Factory
{
    public interface IContextFactory
    {
        IDatabaseContext DbContext { get; }
    }
}