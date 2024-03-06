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
                    Password = table.Column<string>(type: "TEXT", nullable: false),
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
                values: new object[] { "a1df7380-21c0-4883-9f13-2779a3bb41fe", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailAddress", "EmailConfirmed", "Forename", "IsActive", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Password", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Surname", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "06b4d0b9-2fee-483f-af39-cc362deb60c0", 0, "60951d4d-675d-4f04-a845-2a51ed5ac200", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "cpoe@example.com", "cpoe@example.com", false, "Cameron", false, false, null, null, null, "AQAAAAIAAYagAAAAELMdtF0y1aS9L0QM95HCodhJIsYH3ziLkmh33r9nfYFa56qAKw4WZ1Do53CIfRVpgA==", "AQAAAAIAAYagAAAAEJZjf8OMfzzwP7LQwALdyCWQvCGiBNpvbRC/R7fXdq46N7My3p2gz9yLsRzuADBc9g==", null, false, "ef204f35-d8b0-4734-8bc8-da29d0b2849f", "Poe", false, "cpoe@example.com" },
                    { "08ff3603-95e6-426f-8a46-801a4e3834fc", 0, "1746fa08-3d35-4ab2-af79-442306b68726", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@example.com", "admin@example.com", false, "Admin", true, false, null, "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "", "AQAAAAIAAYagAAAAELMdtF0y1aS9L0QM95HCodhJIsYH3ziLkmh33r9nfYFa56qAKw4WZ1Do53CIfRVpgA==", null, false, "8657d506-4a86-4b70-9170-39b646dfc88c", "User", false, "admin@example.com" },
                    { "138cb02e-a2ec-4470-bd94-f51bfc9d5012", 0, "5acaf51c-ee1e-48d1-b7ab-b904d988e815", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "rfeld@example.com", "rfeld@example.com", false, "Robin", true, false, null, null, null, "AQAAAAIAAYagAAAAELMdtF0y1aS9L0QM95HCodhJIsYH3ziLkmh33r9nfYFa56qAKw4WZ1Do53CIfRVpgA==", "AQAAAAIAAYagAAAAEP2BaIf21zhskwSzMdkHyQ+oSx8lVtwTG4jdgN+mNFN7jAhJaNG4AlP5sE6kSbaAZA==", null, false, "bedc2f36-9114-4b4d-bc28-67179afc9739", "Feld", false, "rfeld@example.com" },
                    { "32823223-f1c8-4587-b0da-2791830f6a53", 0, "1d8eca55-e5ce-4bff-9ac4-0a29446f54bb", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "himcdunnough@example.com", "himcdunnough@example.com", false, "H.I.", true, false, null, null, null, "AQAAAAIAAYagAAAAELMdtF0y1aS9L0QM95HCodhJIsYH3ziLkmh33r9nfYFa56qAKw4WZ1Do53CIfRVpgA==", "AQAAAAIAAYagAAAAEB1xZBxt8d94lpfssZyU3SanAiIzYkF0Le6E1YJTl6XHk1WkOP02RdrNZOtE6sKddg==", null, false, "a9e1cb25-4d66-47e3-9762-f811f76df432", "McDunnough", false, "himcdunnough@example.com" },
                    { "3618777a-9b4e-41c0-b328-2e7f64ff06b7", 0, "c103890c-ed17-490d-808a-f7b8e4a83a93", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "emalus@example.com", "emalus@example.com", false, "Edward", false, false, null, null, null, "AQAAAAIAAYagAAAAELMdtF0y1aS9L0QM95HCodhJIsYH3ziLkmh33r9nfYFa56qAKw4WZ1Do53CIfRVpgA==", "AQAAAAIAAYagAAAAEFjXU795ke6IFVdhNdfSNoYyJ0M10QuaGx0PcitMzBiywk/tKjnk5SNCf9OBYyMVbg==", null, false, "5a9a11f9-be20-4e1d-91ec-6221a258e8f5", "Malus", false, "emalus@example.com" },
                    { "6eb12850-737b-4a09-a0ad-7e32bfd772b7", 0, "62b58bf1-02d3-401b-9986-55dbb99a152b", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "bfgates@example.com", "bfgates@example.com", false, "Benjamin Franklin", true, false, null, null, null, "AQAAAAIAAYagAAAAELMdtF0y1aS9L0QM95HCodhJIsYH3ziLkmh33r9nfYFa56qAKw4WZ1Do53CIfRVpgA==", "AQAAAAIAAYagAAAAECWdriAw4Jl/yiEqxbq0jygITskwjxsA16YCM5OSgZ+wRS5+XKVSAyrOGnrILbiSXg==", null, false, "34941b7a-1137-4e45-833a-24925a877f06", "Gates", false, "bfgates@example.com" },
                    { "944d4798-d321-4e0a-8c84-724309553f44", 0, "bb22a7a1-3628-4372-b29d-6ee647d22963", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "dmacready@example.com", "dmacready@example.com", false, "Damon", false, false, null, null, null, "AQAAAAIAAYagAAAAELMdtF0y1aS9L0QM95HCodhJIsYH3ziLkmh33r9nfYFa56qAKw4WZ1Do53CIfRVpgA==", "AQAAAAIAAYagAAAAEO1sLdMDBSd0ZjxWlnAX6yV0/DHtAFldF8Wt1CFcHQMNxSx6TpyEdxwnHa5CQX4dow==", null, false, "a8cbb0e8-3b91-4fcf-8d9a-b98b7208ca7b", "Macready", false, "dmacready@example.com" },
                    { "a72a38e3-072f-4434-af6e-94e16b60db56", 0, "e840608f-a318-44b0-98ad-460132c115cb", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "sgodspeed@example.com", "sgodspeed@example.com", false, "Stanley", true, false, null, null, null, "AQAAAAIAAYagAAAAELMdtF0y1aS9L0QM95HCodhJIsYH3ziLkmh33r9nfYFa56qAKw4WZ1Do53CIfRVpgA==", "AQAAAAIAAYagAAAAEAzVfhb1LTGKmHMhz5GAd+wYq5v7T+S1pO95rmA/GQt56K5IqucQnEE4uoSWsZ3HfQ==", null, false, "8b22df38-1544-4d54-8064-00bf8cbf9644", "Goodspeed", false, "sgodspeed@example.com" },
                    { "bb3a2af4-51f9-49ba-94a9-ec51bff7cbb7", 0, "bafc55f9-3e9b-45d2-a60f-0afeef253cfc", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "jblaze@example.com", "jblaze@example.com", false, "Johnny", true, false, null, null, null, "AQAAAAIAAYagAAAAELMdtF0y1aS9L0QM95HCodhJIsYH3ziLkmh33r9nfYFa56qAKw4WZ1Do53CIfRVpgA==", "AQAAAAIAAYagAAAAEI5fI4LXXLaNZHcylzNxj3+iDCKX5l69VSPmEea2FPLnvMbx0AjYQ/CSjdz0tskFCQ==", null, false, "bbb1d3b7-6de2-4f2f-82b4-1e8f58f0c9ed", "Blaze", false, "jblaze@example.com" },
                    { "c37e9c41-90e0-44d5-8864-5b49a422bd18", 0, "03d1b0d2-4107-4f69-bf26-33d93d969bfa", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ctroy@example.com", "ctroy@example.com", false, "Castor", false, false, null, null, null, "AQAAAAIAAYagAAAAELMdtF0y1aS9L0QM95HCodhJIsYH3ziLkmh33r9nfYFa56qAKw4WZ1Do53CIfRVpgA==", "AQAAAAIAAYagAAAAEG+fDCj7ikT5KrujJoUVGTE94r+FvZNBo2jar4+7hycLQjZ0JAvqbXEzuMRCJ1Becg==", null, false, "ec0e6c8b-4c12-44d7-87a6-3dbbef111d11", "Troy", false, "ctroy@example.com" },
                    { "c70a253c-1aa6-4c19-a3ff-807a7087a5e7", 0, "45bedafc-901c-442b-9feb-498f9a01428d", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ploew@example.com", "ploew@example.com", false, "Peter", true, false, null, null, null, "AQAAAAIAAYagAAAAELMdtF0y1aS9L0QM95HCodhJIsYH3ziLkmh33r9nfYFa56qAKw4WZ1Do53CIfRVpgA==", "AQAAAAIAAYagAAAAEHmzLGzU9yXURlaIrYqQK6amw+UfjLrAywPntOzDfRUqhP+OM6LgXZfuRDfnB+lSpw==", null, false, "1f0cafd3-9f34-4c83-8081-316326f82e51", "Loew", false, "ploew@example.com" },
                    { "d04896e4-d808-4fc3-9b81-9d4d9b656510", 0, "cadf661c-3533-48a1-9608-bdc26c51e3ce", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "mraines@example.com", "mraines@example.com", false, "Memphis", true, false, null, null, null, "AQAAAAIAAYagAAAAELMdtF0y1aS9L0QM95HCodhJIsYH3ziLkmh33r9nfYFa56qAKw4WZ1Do53CIfRVpgA==", "AQAAAAIAAYagAAAAEAzdfdfivn3AjTk0C3oF+WVah+ejZrJD/zsm7BNHyxHgWg90lgYCCN63tnz71RMang==", null, false, "1e518df9-835b-469b-8b27-20fcf95f21b6", "Raines", false, "mraines@example.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "a1df7380-21c0-4883-9f13-2779a3bb41fe", "08ff3603-95e6-426f-8a46-801a4e3834fc" });

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
