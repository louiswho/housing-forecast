using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Housing.Forecast.Context;
using Housing.Forecast.Context.Models;
using Housing.Forecast.Context.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Housing.Forecast.Service
{
    public static class Seeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context =
                new ForecastContext(serviceProvider.GetRequiredService<DbContextOptions<ForecastContext>>()))
            {
                if (context.Snapshots.Any())
                {
                    return;
                }

                context.Snapshots.AddRange(
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 4, 10),
                        RoomOccupancyCount = 2869,
                        UserCount = 1783,
                        Location = "All"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 4, 17),
                        RoomOccupancyCount = 2769,
                        UserCount = 1655,
                        Location = "All"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 4, 24),
                        RoomOccupancyCount = 2025,
                        UserCount = 1911,
                        Location = "All"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 5, 1),
                        RoomOccupancyCount = 1278,
                        UserCount = 1167,
                        Location = "All"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 5, 8),
                        RoomOccupancyCount = 2028,
                        UserCount = 1940,
                        Location = "All"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 5, 15),
                        RoomOccupancyCount = 2113,
                        UserCount = 2001,
                        Location = "All"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 5, 22),
                        RoomOccupancyCount = 2654,
                        UserCount = 1841,
                        Location = "All"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 5, 29),
                        RoomOccupancyCount = 1264,
                        UserCount = 1140,
                        Location = "All"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 6, 5),
                        RoomOccupancyCount = 2918,
                        UserCount = 2557,
                        Location = "All"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 6, 12),
                        RoomOccupancyCount = 2160,
                        UserCount = 2112,
                        Location = "All"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 4, 10),
                        RoomOccupancyCount = 743,
                        UserCount = 649,
                        Location = "New York"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 4, 17),
                        RoomOccupancyCount = 534,
                        UserCount = 451,
                        Location = "New York"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 4, 24),
                        RoomOccupancyCount = 547,
                        UserCount = 453,
                        Location = "New York"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 5, 1),
                        RoomOccupancyCount = 994,
                        UserCount = 450,
                        Location = "New York"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 5, 8),
                        RoomOccupancyCount = 632,
                        UserCount = 542,
                        Location = "New York"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 5, 15),
                        RoomOccupancyCount = 600,
                        UserCount = 417,
                        Location = "New York"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 5, 22),
                        RoomOccupancyCount = 573,
                        UserCount = 556,
                        Location = "New York"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 5, 29),
                        RoomOccupancyCount = 948,
                        UserCount = 593,
                        Location = "New York"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 6, 5),
                        RoomOccupancyCount = 476,
                        UserCount = 426,
                        Location = "New York"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 6, 12),
                        RoomOccupancyCount = 438,
                        UserCount = 372,
                        Location = "New York"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 4, 10),
                        RoomOccupancyCount = 679,
                        UserCount = 675,
                        Location = "Reston"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 4, 17),
                        RoomOccupancyCount = 962,
                        UserCount = 542,
                        Location = "Reston"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 4, 24),
                        RoomOccupancyCount = 455,
                        UserCount = 408,
                        Location = "Reston"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 5, 1),
                        RoomOccupancyCount = 991,
                        UserCount = 905,
                        Location = "Reston"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 5, 8),
                        RoomOccupancyCount = 851,
                        UserCount = 529,
                        Location = "Reston"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 5, 15),
                        RoomOccupancyCount = 727,
                        UserCount = 673,
                        Location = "Reston"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 5, 22),
                        RoomOccupancyCount = 624,
                        UserCount = 568,
                        Location = "Reston"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 5, 29),
                        RoomOccupancyCount = 560,
                        UserCount = 497,
                        Location = "Reston"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 6, 5),
                        RoomOccupancyCount = 953,
                        UserCount = 699,
                        Location = "Reston"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 6, 12),
                        RoomOccupancyCount = 864,
                        UserCount = 548,
                        Location = "Reston"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 4, 10),
                        RoomOccupancyCount = 507,
                        UserCount = 422,
                        Location = "Tampa"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 4, 17),
                        RoomOccupancyCount = 948,
                        UserCount = 544,
                        Location = "Tampa"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 4, 24),
                        RoomOccupancyCount = 490,
                        UserCount = 485,
                        Location = "Tampa"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 5, 1),
                        RoomOccupancyCount = 574,
                        UserCount = 544,
                        Location = "Tampa"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 5, 8),
                        RoomOccupancyCount = 688,
                        UserCount = 617,
                        Location = "Tampa"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 5, 15),
                        RoomOccupancyCount = 842,
                        UserCount = 753,
                        Location = "Tampa"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 5, 22),
                        RoomOccupancyCount = 435,
                        UserCount = 397,
                        Location = "Tampa"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 5, 29),
                        RoomOccupancyCount = 889,
                        UserCount = 860,
                        Location = "Tampa"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 6, 5),
                        RoomOccupancyCount = 644,
                        UserCount = 551,
                        Location = "Tampa"
                    },
                    new Snapshot
                    {
                        Id = Guid.NewGuid(),
                        Date = new DateTime(2018, 6, 12),
                        RoomOccupancyCount = 863,
                        UserCount = 606,
                        Location = "Tampa"
                    }
                );

                context.SaveChanges();
            }

        }
    }
}
