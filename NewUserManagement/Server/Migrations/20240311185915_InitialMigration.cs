using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NewUserManagement.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
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
                values: new object[] { "97c7f19d-fc06-4403-8f6b-4f0a2e928916", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "Forename", "IsActive", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Password", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Surname", "TwoFactorEnabled", "UserName", "emailAddress" },
                values: new object[,]
                {
                    { "0050627b-f3dd-429b-a2d5-0749adc540ff", 0, "7605933c-b28f-45ea-a26b-a0561bc7a7a1", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ploew@example.com", false, "Peter", true, false, null, null, null, "AQAAAAIAAYagAAAAENXl+qwNA+GiFzN1uYqwKzV3yp5rnInqAgtqHBcUb3KI6qwU/wmCC5JAxURKg3tUvw==", "AQAAAAIAAYagAAAAELAUra/Pde63e3GyNZ6U+m57XbmXl7ANCWLpz3fjI9kPlw1E67cloyqxd3BS7+vPbg==", null, false, "afcae9b3-54ee-4b67-8841-8917aa1e7cc2", "Loew", false, "ploew@example.com", "ploew@example.com" },
                    { "0059ad64-a97b-4fc2-9767-b4b9066a7b5d", 0, "539de745-d290-4ef8-87c6-c1037f909a3d", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "jblaze@example.com", false, "Johnny", true, false, null, null, null, "AQAAAAIAAYagAAAAENXl+qwNA+GiFzN1uYqwKzV3yp5rnInqAgtqHBcUb3KI6qwU/wmCC5JAxURKg3tUvw==", "AQAAAAIAAYagAAAAEF2/fWL5SGiMefaWgaPUKSIm0Mb8tMwB/GByQfyzPL6zJBnOrbcUOTm0zYo6EVpvwA==", null, false, "a582b1ec-ce52-4e69-8b05-0f3319777ec1", "Blaze", false, "jblaze@example.com", "jblaze@example.com" },
                    { "1870a854-f2d7-45dc-a648-ae73f9a41566", 0, "a8fe94e0-4c41-42fe-86cc-448d2c480594", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "dmacready@example.com", false, "Damon", false, false, null, null, null, "AQAAAAIAAYagAAAAENXl+qwNA+GiFzN1uYqwKzV3yp5rnInqAgtqHBcUb3KI6qwU/wmCC5JAxURKg3tUvw==", "AQAAAAIAAYagAAAAEOpVzV7Qyv+j8Q4O6GPK9UtIQTpoe8Gb/Jt+hgkuEdM9RWN2kt4b4ccPGupYC4MP1g==", null, false, "445a50e3-14e7-41fb-8bf3-c65e6bcfee2b", "Macready", false, "dmacready@example.com", "dmacready@example.com" },
                    { "1dc31d76-c3ae-4bcc-803b-f8be24b7280f", 0, "0c67e562-b21a-47ec-8b5f-b2096ba8ec2d", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "emalus@example.com", false, "Edward", false, false, null, null, null, "AQAAAAIAAYagAAAAENXl+qwNA+GiFzN1uYqwKzV3yp5rnInqAgtqHBcUb3KI6qwU/wmCC5JAxURKg3tUvw==", "AQAAAAIAAYagAAAAEJybyPeNiKGEHmSm14ttI/p+JXqpj4MupVLveAA6FB8ziWVgYpDo0vLBjp1C0LP5mg==", null, false, "94c5a7bc-5b29-46cd-95a4-8256b8805a87", "Malus", false, "emalus@example.com", "emalus@example.com" },
                    { "1dd76690-6bed-4e01-8271-9f23c0a9443f", 0, "94ea24c2-fc71-442a-bcbe-93fbc4d84fa8", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "mraines@example.com", false, "Memphis", true, false, null, null, null, "AQAAAAIAAYagAAAAENXl+qwNA+GiFzN1uYqwKzV3yp5rnInqAgtqHBcUb3KI6qwU/wmCC5JAxURKg3tUvw==", "AQAAAAIAAYagAAAAEGNGprVil/UBxjM5E5sBKQW0To/YPCtNNZavPnLAif5FLyJIJBTliMHp1e+x/lBGYw==", null, false, "767b7efb-6b7f-4d64-b629-8e2f03250236", "Raines", false, "mraines@example.com", "mraines@example.com" },
                    { "518923be-6c58-4254-a2b6-4ebaffb664d6", 0, "092851ad-3952-4be2-9362-572a3ffa197d", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "cpoe@example.com", false, "Cameron", false, false, null, null, null, "AQAAAAIAAYagAAAAENXl+qwNA+GiFzN1uYqwKzV3yp5rnInqAgtqHBcUb3KI6qwU/wmCC5JAxURKg3tUvw==", "AQAAAAIAAYagAAAAEBlABBC59TsYj3hvAFzsb6UJ/kctf9GLaBccZG0xZhsb5JfUr5rdAlFLtvHzR7ZIHw==", null, false, "983d1884-8fbd-4235-b232-5d155a01d6cd", "Poe", false, "cpoe@example.com", "cpoe@example.com" },
                    { "61c39edd-4e45-4e90-ba45-5564f7c0aceb", 0, "1da0093b-f6a8-4c78-8eca-0dd18143c7d3", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ctroy@example.com", false, "Castor", false, false, null, null, null, "AQAAAAIAAYagAAAAENXl+qwNA+GiFzN1uYqwKzV3yp5rnInqAgtqHBcUb3KI6qwU/wmCC5JAxURKg3tUvw==", "AQAAAAIAAYagAAAAEJ8sB/FJXRlx86K+MndwBSxHriyfe2iOqYLx0dWGmHodPExx+gqX7siW9jGbjIso0A==", null, false, "9a539d5e-055e-46e8-a1fd-fce79f58b7c7", "Troy", false, "ctroy@example.com", "ctroy@example.com" },
                    { "6892e176-e47f-4e71-9f8a-141b658c78c2", 0, "25adfbae-45a9-4d2d-90ee-130e39e1ffb3", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "bfgates@example.com", false, "Benjamin Franklin", true, false, null, null, null, "AQAAAAIAAYagAAAAENXl+qwNA+GiFzN1uYqwKzV3yp5rnInqAgtqHBcUb3KI6qwU/wmCC5JAxURKg3tUvw==", "AQAAAAIAAYagAAAAEOsxz1/933z3ywavs9AQGVr9J2FOijXaor2R/iSNqC/pcY8Ie1RtckCtXbtJjxSCeA==", null, false, "19a7f78e-957d-4aac-a6a0-0edcb6c4c3b9", "Gates", false, "bfgates@example.com", "bfgates@example.com" },
                    { "bea53bf6-befa-46e1-9839-8cd5f803587b", 0, "3b7020b5-bd3c-41dd-9e24-d5c11b870883", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "sgodspeed@example.com", false, "Stanley", true, false, null, null, null, "AQAAAAIAAYagAAAAENXl+qwNA+GiFzN1uYqwKzV3yp5rnInqAgtqHBcUb3KI6qwU/wmCC5JAxURKg3tUvw==", "AQAAAAIAAYagAAAAEEcEgEkZijgEyisTEJIN/AsyVUilX6jTZA5iMZUYJPDSLKIHJRJyRMo1cozQbr3t0w==", null, false, "c9cd3c77-5219-4555-a403-81d103d33d49", "Goodspeed", false, "sgodspeed@example.com", "sgodspeed@example.com" },
                    { "cf86a3cf-a6c7-45f8-998b-18d1c060153d", 0, "fc812bde-4aae-47d9-abbb-9359e0c6fe7c", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "rfeld@example.com", false, "Robin", true, false, null, null, null, "AQAAAAIAAYagAAAAENXl+qwNA+GiFzN1uYqwKzV3yp5rnInqAgtqHBcUb3KI6qwU/wmCC5JAxURKg3tUvw==", "AQAAAAIAAYagAAAAED9llX7tPartp2t6meXuD6Dj6i3DsTaUzBEM3B+k7jdW05JnDcjSKMguX9gojM4VBg==", null, false, "264c9e16-bbab-428d-98b0-5ba12adfdd21", "Feld", false, "rfeld@example.com", "rfeld@example.com" },
                    { "d0d3e2c6-9df2-4563-b71b-7114a594930d", 0, "db57fa0e-5b24-4c17-bd50-db48767884d0", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@example.com", false, "Admin", true, false, null, "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "", "AQAAAAIAAYagAAAAENXl+qwNA+GiFzN1uYqwKzV3yp5rnInqAgtqHBcUb3KI6qwU/wmCC5JAxURKg3tUvw==", null, false, "f5b22ed0-2311-411d-b854-f5cc146cbfd3", "User", false, "admin@example.com", "admin@example.com" },
                    { "e2182448-7f5c-4cdd-810e-421bd316ebe3", 0, "ca43e630-a0d7-46f0-ada4-5b144838aa30", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "himcdunnough@example.com", false, "H.I.", true, false, null, null, null, "AQAAAAIAAYagAAAAENXl+qwNA+GiFzN1uYqwKzV3yp5rnInqAgtqHBcUb3KI6qwU/wmCC5JAxURKg3tUvw==", "AQAAAAIAAYagAAAAEFt8BTZCvR/6i7X4psTBT9tRhU2cTpO6vWI2mniNO+2bqynPglvyqDktjK4iKxCewQ==", null, false, "0894af26-37f6-4507-8487-7c97bc168ea5", "McDunnough", false, "himcdunnough@example.com", "himcdunnough@example.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "97c7f19d-fc06-4403-8f6b-4f0a2e928916", "d0d3e2c6-9df2-4563-b71b-7114a594930d" });

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
