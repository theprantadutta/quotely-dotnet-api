﻿using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using quotely_dotnet_api.Entities;

namespace quotely_dotnet_api.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Author> Authors { get; set; }
    
    public DbSet<Quote> Quotes { get; set; }
    
    public DbSet<Tag> Tags { get; set; }
}