using Transport.Api.Abstractions.Interfaces.Assemblers;

namespace Transport.Api.Abstractions.Assemblers;

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

	public async Task<TB> Convert(Task<TA> obj)
	{
		var data = await obj;
		return Convert(data);
	}

	public async Task<TA> Convert(Task<TB> obj)
	{
		var data = await obj;
		return Convert(data);
	}
}