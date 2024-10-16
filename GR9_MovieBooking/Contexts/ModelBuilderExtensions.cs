using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GR9_MovieBooking.Contexts
{
	public static class ModelBuilderExtensions
	{
		public static void Seed(this ModelBuilder builder)
		{
			var roleId = Guid.NewGuid().ToString();
			var role = new IdentityRole
			{
				Id = roleId,
				Name = "Admin",
				NormalizedName = "ADMIN"
			};
			builder.Entity<IdentityRole>().HasData(role);
			var adminId = Guid.NewGuid().ToString();
			var admin = new IdentityUser
			{
				Id = adminId,
				UserName = "nekomancer",
				NormalizedUserName = "NEKOMANCER",
				Email = "blackcatinnight0102@gmail.com",
				NormalizedEmail = "BLACKCATINNIGHT0102@GMAIL.COM",
				EmailConfirmed = true,
				LockoutEnabled = true
			};
			PasswordHasher<IdentityUser> passwordHasher = new();
			admin.PasswordHash = passwordHasher.HashPassword(admin, "viet2002");
			builder.Entity<IdentityUser>().HasData(admin);
			builder.Entity<IdentityUserRole<string>>().HasData(
				new IdentityUserRole<string>
				{
					RoleId = roleId,
					UserId = adminId
				}
			);
		}
	}
}
