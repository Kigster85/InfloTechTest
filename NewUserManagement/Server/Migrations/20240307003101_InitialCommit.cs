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
                    emailAddress = table.Column<string>(type: "TEXT", nullable: false),
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
                values: new object[] { "e04cd014-e899-4249-aa96-62a47a92d7dd", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "Forename", "IsActive", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Password", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Surname", "TwoFactorEnabled", "UserName", "emailAddress" },
                values: new object[,]
                {
                    { "2c7eef5a-277d-4cfb-ba24-b9a52b07160e", 0, "3a77753e-fa68-45be-9ddd-fa2892a79fd2", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "bfgates@example.com", false, "Benjamin Franklin", true, false, null, null, null, "AQAAAAIAAYagAAAAENdTvAL5YMFleToExelPXyc7nhMuamGTemNs0I0/fOP+AFKToNBBMiIyOFUuZd9Dew==", "AQAAAAIAAYagAAAAEJZ78CF9dn71am7odxumOVGhJ7djt3qpwL6gdQYw8J+ZeTCzkAlQTsgGeZfyjU5dGA==", null, false, "46273d2c-309f-4b31-b247-dfc9eefecc18", "Gates", false, "bfgates@example.com", "bfgates@example.com" },
                    { "2ea068fa-6e8a-4ffb-860e-678a2728d2b2", 0, "622941b8-c914-4102-9972-9ed8a3a78604", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "cpoe@example.com", false, "Cameron", false, false, null, null, null, "AQAAAAIAAYagAAAAENdTvAL5YMFleToExelPXyc7nhMuamGTemNs0I0/fOP+AFKToNBBMiIyOFUuZd9Dew==", "AQAAAAIAAYagAAAAEK8OmF47CzbIKjRudNAEKEYoTvlvgWHb3f6gB9jhfRF/b8qIwn2GZqkXmOUsweW/Rw==", null, false, "f433bbb9-df08-4663-b3b3-ec9e37e23aa2", "Poe", false, "cpoe@example.com", "cpoe@example.com" },
                    { "347ed484-a248-411b-8328-c6ed5ac64acb", 0, "cdd65051-5694-4b4e-b99d-ed27637db9dd", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "dmacready@example.com", false, "Damon", false, false, null, null, null, "AQAAAAIAAYagAAAAENdTvAL5YMFleToExelPXyc7nhMuamGTemNs0I0/fOP+AFKToNBBMiIyOFUuZd9Dew==", "AQAAAAIAAYagAAAAEEy8HhPxRXiIFwJqnwaGCkM6EbhH46PHYxxTJCVuFd8jK9+QQpvJtKBU8ZP6Gwwq/Q==", null, false, "eb0b9a1e-fe86-42f7-a9b0-673d71ee5057", "Macready", false, "dmacready@example.com", "dmacready@example.com" },
                    { "4c9662fe-6b29-446a-ae1e-d3efd29f295a", 0, "9694348c-502a-449e-9f16-09812f199460", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "himcdunnough@example.com", false, "H.I.", true, false, null, null, null, "AQAAAAIAAYagAAAAENdTvAL5YMFleToExelPXyc7nhMuamGTemNs0I0/fOP+AFKToNBBMiIyOFUuZd9Dew==", "AQAAAAIAAYagAAAAEDg6ZGZZNyHDC4YjLWsURqnrj8LBE5LgAVEIPXDt5Ga4Mqr0WrZqO5yq9AmZAMADGA==", null, false, "a8657df1-12b7-4595-a8b1-3e9688978db8", "McDunnough", false, "himcdunnough@example.com", "himcdunnough@example.com" },
                    { "59ae1058-6375-4260-938f-0dd978f24b86", 0, "906185ac-8090-4722-a5e0-331a1602cdf7", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "rfeld@example.com", false, "Robin", true, false, null, null, null, "AQAAAAIAAYagAAAAENdTvAL5YMFleToExelPXyc7nhMuamGTemNs0I0/fOP+AFKToNBBMiIyOFUuZd9Dew==", "AQAAAAIAAYagAAAAEJL51w1IGUU/UviSGGVZXwG6pZJzo7/Bz6qVt6rPpGixfo6xmrV4UXah+IyLILv9BA==", null, false, "7f61d95c-8aec-450c-a3fa-d9da66ad3c43", "Feld", false, "rfeld@example.com", "rfeld@example.com" },
                    { "5af9f472-1c06-4469-a619-8fea3bc2c626", 0, "525d21d9-cdf4-4f86-a05c-b22ffc112616", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@example.com", false, "Admin", true, false, null, "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "", "AQAAAAIAAYagAAAAENdTvAL5YMFleToExelPXyc7nhMuamGTemNs0I0/fOP+AFKToNBBMiIyOFUuZd9Dew==", null, false, "0914bd24-7af4-4517-aee5-54f07ba4aae4", "User", false, "admin@example.com", "admin@example.com" },
                    { "7e421790-50b6-40e0-8fd0-310b884221e7", 0, "e5a942b8-9f7e-471e-a9f2-4b129532388e", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "jblaze@example.com", false, "Johnny", true, false, null, null, null, "AQAAAAIAAYagAAAAENdTvAL5YMFleToExelPXyc7nhMuamGTemNs0I0/fOP+AFKToNBBMiIyOFUuZd9Dew==", "AQAAAAIAAYagAAAAEO20eVwS6RyWLWr+atGL/b41d/ogfv8K4KPWGIloJQc/CFJhScqeXWVV6hYbfwTi4A==", null, false, "c3cce728-12f0-4683-8606-02002e815915", "Blaze", false, "jblaze@example.com", "jblaze@example.com" },
                    { "bdc38d22-618e-47ff-b765-1080f714c8fc", 0, "22fe4efd-ae8e-4bdb-bd88-5998101f48d8", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "sgodspeed@example.com", false, "Stanley", true, false, null, null, null, "AQAAAAIAAYagAAAAENdTvAL5YMFleToExelPXyc7nhMuamGTemNs0I0/fOP+AFKToNBBMiIyOFUuZd9Dew==", "AQAAAAIAAYagAAAAEO8iKuEoyVXR2dsXtgaXGJENngSdcVeggA2+117FKt8cI3dyrv+uX6a32ZkIUc+HKw==", null, false, "a10960b9-2801-4574-877a-cf32a30fe177", "Goodspeed", false, "sgodspeed@example.com", "sgodspeed@example.com" },
                    { "c09f18a1-955b-4b67-841f-176590cdef82", 0, "7485a27e-ec8a-430c-b1ad-38a43708aa32", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ctroy@example.com", false, "Castor", false, false, null, null, null, "AQAAAAIAAYagAAAAENdTvAL5YMFleToExelPXyc7nhMuamGTemNs0I0/fOP+AFKToNBBMiIyOFUuZd9Dew==", "AQAAAAIAAYagAAAAEAWbd5YGY7/92XRIQBK7nffFHztt8u0bDpXkDc4lyVPgKnn4szCzcf30Ik6MwaiVdA==", null, false, "44ff51bb-2df6-4a9a-97ac-9aeec9907e6e", "Troy", false, "ctroy@example.com", "ctroy@example.com" },
                    { "cce3ae8d-02c9-422d-9660-2bee2be19d19", 0, "4e8a38b1-aed5-4afd-b6ef-c7c1653674df", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "emalus@example.com", false, "Edward", false, false, null, null, null, "AQAAAAIAAYagAAAAENdTvAL5YMFleToExelPXyc7nhMuamGTemNs0I0/fOP+AFKToNBBMiIyOFUuZd9Dew==", "AQAAAAIAAYagAAAAEA7E0hE7Gwm7wGPqpWvQmIVGxTced3h0kJxMqi9kTX59nVdhNc5uY9cULr5z8cUohg==", null, false, "f92b2346-bbac-4d86-b462-ac9b6f7aeb6c", "Malus", false, "emalus@example.com", "emalus@example.com" },
                    { "d6d30808-8cdf-46af-a8b1-4187de5084ab", 0, "2f285324-30cc-434f-90e1-a4bb1bf61a18", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "mraines@example.com", false, "Memphis", true, false, null, null, null, "AQAAAAIAAYagAAAAENdTvAL5YMFleToExelPXyc7nhMuamGTemNs0I0/fOP+AFKToNBBMiIyOFUuZd9Dew==", "AQAAAAIAAYagAAAAEFSPlVAIGwT9qkcy3W5RG7j9pQhMwG8oMAPtaQBQ/kuozwkEn7AfW3xt1oa5qmMDDw==", null, false, "0f82424f-f6ee-4277-9883-2e9282b598c5", "Raines", false, "mraines@example.com", "mraines@example.com" },
                    { "f1529162-c9e0-4d6b-9151-47dd5cdd7c72", 0, "d104407e-477b-4d50-a1ee-f8a34869d626", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ploew@example.com", false, "Peter", true, false, null, null, null, "AQAAAAIAAYagAAAAENdTvAL5YMFleToExelPXyc7nhMuamGTemNs0I0/fOP+AFKToNBBMiIyOFUuZd9Dew==", "AQAAAAIAAYagAAAAEDdxtynIwmUemJlm4jVJQ6JZZ0g1dqXJ6B+nAu2x5LwZkr250pck4ru/auFyc7ZUMg==", null, false, "5d877e9a-0334-4774-9b14-fe432a0d3ef8", "Loew", false, "ploew@example.com", "ploew@example.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "e04cd014-e899-4249-aa96-62a47a92d7dd", "5af9f472-1c06-4469-a619-8fea3bc2c626" });

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
