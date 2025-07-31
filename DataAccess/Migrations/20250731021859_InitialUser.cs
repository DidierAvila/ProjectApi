using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                SET IDENTITY_INSERT [User] ON;
                INSERT INTO [User] ([Id], [Name], [LastName], [Email], [Password], [Rol], [Phone])
                VALUES 
                (1, 'Alejo', 'Pertuz', 'alejopertuz@gmail.com', '123', 'admin', '+573218899857'),
                (2, 'Ana', 'Frank', 'anafrank@gmail.com', '321', 'user', '+578899966455');
                SET IDENTITY_INSERT [User] OFF;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM [User] WHERE [Id] IN (1, 2);
            ");
        }
    }
}
