using Microsoft.EntityFrameworkCore;
using UI.Models;

namespace UI.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
                  : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ListItemCategory> ListItemCategories { get; set; }
        public DbSet<ListItem> ListItems { get; set; }
        public DbSet<EmployeeRole> EmployeeRoles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            //for person table
            modelBuilder.Entity<Person>().HasData(new Person { PersonId = 1, FirstName = "Admin", LastName="Admin", GenderListItemId = 1 });


            //for employee Table
            modelBuilder.Entity<Employee>().HasData(new Employee { EmployeeId = 1, PersonId = 1, Email = "superadmin@gmail.com", Password = Helper.HashPassword("admin"), RoleId = 1 });


            //for role table
            modelBuilder.Entity<Role>().HasData(new Role { RoleId = 1, RoleName = "SuperAdmin" });


            //for listItem Category table
            modelBuilder.Entity<ListItemCategory>().HasData(new ListItemCategory { ListItemCategoryId = 1, ListItemCategoryName = "Gender"});

            //for ListItem table

            modelBuilder.Entity<ListItem>().HasData(
               new ListItem { ListItemId = 1, ListItemCategoryId = 1, ListItemName = "Male" },
               new ListItem { ListItemId = 2, ListItemCategoryId = 1, ListItemName = "Female" }
           );

            //for employeerole realtion Table
            modelBuilder.Entity<EmployeeRole>().HasData(new EmployeeRole { EmployeeRoleId = 1, EmployeeId = 1, RoleId = 1 });


            base.OnModelCreating(modelBuilder);
        }

    }
}

