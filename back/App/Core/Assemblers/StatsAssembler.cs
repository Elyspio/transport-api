using Transport.Api.Abstractions.Assemblers;
using Transport.Api.Abstractions.Models;
using Transport.Api.Abstractions.Transports;

namespace Transport.Api.Core.Assemblers;

internal class StatsAssembler : BaseAssembler<Statistic, StatisticEntity>
{
    public override StatisticEntity Convert(Statistic obj)
    {
        throw new NotImplementedException();
    }

    public override Statistic Convert(StatisticEntity obj)
    {
        var dtMiddle = obj.Time.Start.AddDays(obj.Time.End.Day - obj.Time.Start.Day / 2);

        return new Statistic(obj.Id.ToString(), dtMiddle, obj.Statistic);
    }
}