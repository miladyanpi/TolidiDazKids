using Dto.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    public static class DbSeeder
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            //SeedUsers(modelBuilder);
            //SeedRoles(modelBuilder);
            //SeedUserRoles(modelBuilder);
        }
        //private static void SeedUsers(ModelBuilder modelBuilder)
        //{
        //    var ph = new PasswordHasher<IdentityUser>();
        //    var user1 = new IdentityUser
        //    {
        //        Id = "1",
        //        UserName = "doctor",
        //        NormalizedUserName = "DOCTOR@GMAIL.COM".ToUpper(),
        //        Email = "doctor@gmail.com",
        //        EmailConfirmed = true,

        //    };
        //    var user2 = new IdentityUser
        //    {
        //        Id = "2",
        //        UserName = "monshi",
        //        NormalizedUserName = "MONSHI@GMAIL.COM".ToUpper(),
        //        Email = "monshi@gmail.com",
        //        EmailConfirmed = true,

        //    };
        //    user1.PasswordHash = ph.HashPassword(user1, "123");
        //    user2.PasswordHash = ph.HashPassword(user2, "123");
        //    modelBuilder.Entity<IdentityUser>().HasData(user1);
        //    modelBuilder.Entity<IdentityUser>().HasData(user2);
        //}
        //private static void SeedRoles(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<IdentityRole>().HasData(
        //        new IdentityRole
        //        {
        //            Id= Scopes.doctor_role,
        //            Name= Scopes.doctor,
        //            NormalizedName= Scopes.doctor.ToUpper(),
        //        },
        //         new IdentityRole
        //         {
        //             Id = Scopes.Secretary_role,
        //             Name = Scopes.Secretary,
        //             NormalizedName = Scopes.Secretary.ToUpper(),
        //         });
        //}
        //private static void SeedUserRoles(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<IdentityUserRole<string>>().HasData(
        //        new IdentityUserRole<string>
        //        {
        //            RoleId = Scopes.doctor_role,
        //            UserId = "1",
        //        },
        //          new IdentityUserRole<string>
        //          {
        //              RoleId = Scopes.Secretary_role,
        //              UserId = "2",
        //          }
        //        );
        //}
    }
}
