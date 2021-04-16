using GameBauCua.Web.Client.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameBauCua.Web.Client.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<RoundPlayDetail>()
                .HasKey(hk => new { hk.PlayerId, hk.RoundPlayId });

            builder.Entity<RoomDetail>()
                .HasKey(hk => new { hk.PlayerId, hk.RoomId });

            builder.Entity<RoundPlay>()
                .HasOne(ho => ho.Room)
                .WithMany(wm => wm.RoundPlays)
                .HasForeignKey(fk => fk.RoomId);

            base.OnModelCreating(builder);
        }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<RoomDetail> RoomDetails { get; set; }

        public DbSet<RoundPlay> RoundPlays { get; set; }

        public DbSet<RoundPlayDetail> RoundPlayDetails { get; set; }
    }
}
