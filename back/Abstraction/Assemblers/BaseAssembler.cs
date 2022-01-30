using Abstraction.Interfaces.Assemblers;

namespace Abstraction.Assemblers;

public abstract class BaseAssembler<TA, TB> : IAssembler<TA, TB>
{
    public abstract TB Convert(TA obj);

    public abstract TA Convert(TB obj);


    public IEnumerable<TB> Convert(IEnumerable<TA> objs)
    {
        return objs.Select(Convert).ToList();
    }

    public IEnumerable<TA> Convert(IEnumerable<TB> objs)
    {
        return objs.Select(Convert).ToList();
    }
}