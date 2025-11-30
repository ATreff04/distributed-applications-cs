using System;
using Microsoft.EntityFrameworkCore;
using TouristAgency.Entities;

public class TouristAgencyDbContext : DbContext
{
   

    public DbSet<Destination> Destinations { get; set; }
    public DbSet<Offer> Offers { get; set; }
    public DbSet<Trip> Trips { get; set; }


    public TouristAgencyDbContext(DbContextOptions<TouristAgencyDbContext> options) : base(options) { }
}

