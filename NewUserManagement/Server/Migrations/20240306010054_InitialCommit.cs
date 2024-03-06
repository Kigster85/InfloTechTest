using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NewUserManagement.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCommit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    EmailAddress = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Forename = table.Column<string>(type: "TEXT", nullable: false),
                    Surname = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogEntries",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: true),
                    Action = table.Column<string>(type: "TEXT", nullable: true),
                    Details = table.Column<string>(type: "TEXT", nullable: true),
                    ViewCount = table.Column<int>(type: "INTEGER", nullable: false),
                    EditCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDeletedUserEntry = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeletedUserId = table.Column<string>(type: "TEXT", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogEntries", x => x.LogId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ef231070-2e2f-4656-9465-403153dcc55c", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailAddress", "EmailConfirmed", "Forename", "IsActive", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Password", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Surname", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "2e6f6df0-dbb5-4b0d-970d-32568547d765", 0, "c1f08fea-4424-4a30-b39f-ef72678f2ca4", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "jblaze@example.com", "jblaze@example.com", false, "Johnny", true, false, null, null, null, "AQAAAAIAAYagAAAAEDVC9KmYCrJgZBuEWidDphkSusjsH+Aqbe2r9kisH3WJ82E10jeTLE9AQj0AXOiCBQ==", "AQAAAAIAAYagAAAAEGKoNvfCYxUNc967teFVW4NcDJbhBO4j6mDiExw70K1tg3AWswTTaIx8zGng5303zQ==", null, false, "bd866ab5-2fd9-4d40-8621-197e56ac59b7", "Blaze", false, "jblaze@example.com" },
                    { "4aefaeaf-3392-4afe-bdc8-b88f642f61fb", 0, "ad702eec-9a8a-4a2c-a408-34c54a0fcf85", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "emalus@example.com", "emalus@example.com", false, "Edward", false, false, null, null, null, "AQAAAAIAAYagAAAAEDVC9KmYCrJgZBuEWidDphkSusjsH+Aqbe2r9kisH3WJ82E10jeTLE9AQj0AXOiCBQ==", "AQAAAAIAAYagAAAAEHcDv45UsLqeZDvgmirhzwsjZ0qy3zeM+v6TNmI4tNmbz5R4EfN3VYZZMPL1DGAXNw==", null, false, "a6def7e7-7cf8-4f1f-9f61-2e001214bfaf", "Malus", false, "emalus@example.com" },
                    { "585280a3-5351-4cfb-a0c6-cdd135cb7038", 0, "582e09c1-7d45-4c1c-a0d8-98fc894f0ff4", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "rfeld@example.com", "rfeld@example.com", false, "Robin", true, false, null, null, null, "AQAAAAIAAYagAAAAEDVC9KmYCrJgZBuEWidDphkSusjsH+Aqbe2r9kisH3WJ82E10jeTLE9AQj0AXOiCBQ==", "AQAAAAIAAYagAAAAEC39qefHxJsRHo0feonEgjgqK7U7qe6x7YjN4tZvLbl98N4/DF9yEMAw3XHxOXp2tw==", null, false, "bacbd181-8b49-4a4d-8522-5c7f413cea76", "Feld", false, "rfeld@example.com" },
                    { "60df61bc-eb74-4567-98b9-e4bd10ebdd92", 0, "92494087-40ce-47cf-9ba0-8fad0bbdf296", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ploew@example.com", "ploew@example.com", false, "Peter", true, false, null, null, null, "AQAAAAIAAYagAAAAEDVC9KmYCrJgZBuEWidDphkSusjsH+Aqbe2r9kisH3WJ82E10jeTLE9AQj0AXOiCBQ==", "AQAAAAIAAYagAAAAECxe6dPYQs9v69TD/+A/gw5cdauUvZ8pzqMp1Io4wKNI/BlmXT67BMsPVoAiHvYWxQ==", null, false, "e31bb621-1e93-45ba-9c22-81cabf3d179f", "Loew", false, "ploew@example.com" },
                    { "8f8f271d-c3ab-4ac6-838e-d00b5f7cc6f3", 0, "5952abd5-dd58-4553-9eeb-fea771046055", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "cpoe@example.com", "cpoe@example.com", false, "Cameron", false, false, null, null, null, "AQAAAAIAAYagAAAAEDVC9KmYCrJgZBuEWidDphkSusjsH+Aqbe2r9kisH3WJ82E10jeTLE9AQj0AXOiCBQ==", "AQAAAAIAAYagAAAAEJoj/3a2HojWKAx+Ka77vHVEbLSSKZTrgsBnTKwAD3wRBSKH3LKt3dK2ekn0R1IN8g==", null, false, "828fece5-8569-40c5-92d1-2b3744b0a49a", "Poe", false, "cpoe@example.com" },
                    { "901f0fb8-a07e-4e20-8bc1-6c5151e9ec05", 0, "9e635b10-2410-4e07-a7a3-e13b612b48f3", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "bfgates@example.com", "bfgates@example.com", false, "Benjamin Franklin", true, false, null, null, null, "AQAAAAIAAYagAAAAEDVC9KmYCrJgZBuEWidDphkSusjsH+Aqbe2r9kisH3WJ82E10jeTLE9AQj0AXOiCBQ==", "AQAAAAIAAYagAAAAEIcRTKFJE4TJu2ai85WBHwNQwl4XwGEhGRATJdYzTYkqSJ4bbXOkbZtXNGJ6V9Ttsw==", null, false, "72dad0e2-fdfd-427c-b0e4-3e2ed13ea91f", "Gates", false, "bfgates@example.com" },
                    { "b2352a1e-7ea8-4521-a776-315e06d8c3fa", 0, "01c1f48e-dee7-4ca8-9539-60214ffd60a9", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@example.com", "admin@example.com", false, "Admin", true, false, null, "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "", "AQAAAAIAAYagAAAAEDVC9KmYCrJgZBuEWidDphkSusjsH+Aqbe2r9kisH3WJ82E10jeTLE9AQj0AXOiCBQ==", null, false, "d65593e7-e0fe-490b-975a-6ce7fc321406", "User", false, "admin@example.com" },
                    { "c5515491-0cb3-45b7-aa35-9ea9459be365", 0, "1a46fdb9-eff9-4e86-8ecd-e3b4d2b61aff", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ctroy@example.com", "ctroy@example.com", false, "Castor", false, false, null, null, null, "AQAAAAIAAYagAAAAEDVC9KmYCrJgZBuEWidDphkSusjsH+Aqbe2r9kisH3WJ82E10jeTLE9AQj0AXOiCBQ==", "AQAAAAIAAYagAAAAEHV5G6i3TIvliPLcBNxfRL198blT2Q1cp4jlnx92tfpYOuAARuD1RHaV1XNZZk2LCw==", null, false, "8acc08e2-29b0-4d16-a083-09557b97569e", "Troy", false, "ctroy@example.com" },
                    { "d24e24c0-985b-4663-b768-688450997091", 0, "f80b6de2-8154-41a5-af2c-ae5c531fd2e0", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "himcdunnough@example.com", "himcdunnough@example.com", false, "H.I.", true, false, null, null, null, "AQAAAAIAAYagAAAAEDVC9KmYCrJgZBuEWidDphkSusjsH+Aqbe2r9kisH3WJ82E10jeTLE9AQj0AXOiCBQ==", "AQAAAAIAAYagAAAAEOCADHBl+rEF2hv1wBFOL/d5SpGsPbR+tyPGwUDKelsNA/7X/EalEYdSTMvJxFaroQ==", null, false, "49b253d5-0c9b-4e00-a529-5d4a895cab00", "McDunnough", false, "himcdunnough@example.com" },
                    { "df5596d7-26ad-48e2-a8b0-5e12bf94ea0f", 0, "ceb749b3-077a-4556-a726-3d33bc5e1e83", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "mraines@example.com", "mraines@example.com", false, "Memphis", true, false, null, null, null, "AQAAAAIAAYagAAAAEDVC9KmYCrJgZBuEWidDphkSusjsH+Aqbe2r9kisH3WJ82E10jeTLE9AQj0AXOiCBQ==", "AQAAAAIAAYagAAAAEJF0J/qYFjpRvCcVHtDyqjo3QL1RjNJCvmY2oAk8kMZJJnatT0u7+JtZW6mOoO97tQ==", null, false, "b656d931-39e0-4e3e-8d88-15740b993ba5", "Raines", false, "mraines@example.com" },
                    { "ea6d152f-72ea-49dd-abf4-cf2fc30411f6", 0, "6f4a2a1d-bca4-4a50-8f0d-8cdaf18734d2", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "sgodspeed@example.com", "sgodspeed@example.com", false, "Stanley", true, false, null, null, null, "AQAAAAIAAYagAAAAEDVC9KmYCrJgZBuEWidDphkSusjsH+Aqbe2r9kisH3WJ82E10jeTLE9AQj0AXOiCBQ==", "AQAAAAIAAYagAAAAEDoRAdNHh74sCi9JJ1MiQa+8YibEnSq654cQYzdHXCVN/PXGJc32rNDKWVY6q3avWw==", null, false, "d23e1753-0042-4aea-b69f-d53c2c128a36", "Goodspeed", false, "sgodspeed@example.com" },
                    { "ea8e4f3b-d83a-455b-9422-9ec2979e1384", 0, "62c91de4-c82d-4639-9df9-e58cbd4c5ea1", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "dmacready@example.com", "dmacready@example.com", false, "Damon", false, false, null, null, null, "AQAAAAIAAYagAAAAEDVC9KmYCrJgZBuEWidDphkSusjsH+Aqbe2r9kisH3WJ82E10jeTLE9AQj0AXOiCBQ==", "AQAAAAIAAYagAAAAEFOBCDoc4xg8041pHMVRnHKitqLwpilaqTJKILtymx3hINR8YKWsGBrmgv+QyJQ2Zg==", null, false, "3c365d49-e98f-4ecb-9960-f924246b798d", "Macready", false, "dmacready@example.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "ef231070-2e2f-4656-9465-403153dcc55c", "b2352a1e-7ea8-4521-a776-315e06d8c3fa" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "LogEntries");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
