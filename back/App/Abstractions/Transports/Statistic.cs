using System.ComponentModel.DataAnnotations;
using Transport.Api.Abstractions.Models;

namespace Transport.Api.Abstractions.Transports;

public class Statistic
{
    public Statistic(string id, DateTime time, StatisticInfo data)
    {
        Time = time;
        Data = data;
        Id = id;
    }


    [Required] public DateTime Time { get; set; }

    [Required] public string Id { get; set; }

    [Required] public StatisticInfo Data { get; set; }
}