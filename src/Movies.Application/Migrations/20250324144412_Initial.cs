﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Movies.Application.Domain.Metadata;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Movies.Application.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "ID")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, comment: "Email Address"),
                    password = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "Password"),
                    auth_key = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "Authorization Key"),
                    password_hash = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false, comment: "Password Hash"),
                    first_name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, comment: "First name"),
                    last_name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, comment: "Last name"),
                    role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, comment: "Role"),
                    status = table.Column<int>(type: "integer", nullable: false, comment: "Status"),
                    metadata = table.Column<UserMetadata>(type: "jsonb", nullable: false, defaultValueSql: "'{}'::jsonb", comment: "Metadata"),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Created"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Updated")
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "movies",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "ID")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false, comment: "User"),
                    title = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false, comment: "Title"),
                    year_of_release = table.Column<short>(type: "smallint", nullable: false, comment: "Year of Release"),
                    slug = table.Column<string>(type: "character varying(90)", maxLength: 90, nullable: false, comment: "Slug"),
                    status = table.Column<int>(type: "integer", nullable: false, comment: "Status"),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Created"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Updated")
                },
                constraints: table =>
                {
                    table.PrimaryKey("movies_pkey", x => x.id);
                    table.ForeignKey(
                        name: "FK_movies_user_id_users_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "genres",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "ID")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    movie_id = table.Column<long>(type: "bigint", nullable: false, comment: "Movie"),
                    name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false, comment: "Name")
                },
                constraints: table =>
                {
                    table.PrimaryKey("genres_pkey", x => x.id);
                    table.ForeignKey(
                        name: "FK_genres_movie_id_movies_id",
                        column: x => x.movie_id,
                        principalTable: "movies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ratings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "ID")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false, comment: "User"),
                    movie_id = table.Column<long>(type: "bigint", nullable: false, comment: "Movie"),
                    score = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Score"),
                    feedback = table.Column<string>(type: "text", nullable: true, comment: "Feedback"),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Created"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Updated")
                },
                constraints: table =>
                {
                    table.PrimaryKey("ratings_pkey", x => x.id);
                    table.ForeignKey(
                        name: "FK_ratings_movie_id_movies_id",
                        column: x => x.movie_id,
                        principalTable: "movies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ratings_user_id_users_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_genres_movie_id",
                table: "genres",
                column: "movie_id");

            migrationBuilder.CreateIndex(
                name: "ix_genres_name_movie_id",
                table: "genres",
                columns: new[] { "name", "movie_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_movies_slug",
                table: "movies",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_movies_title",
                table: "movies",
                column: "title");

            migrationBuilder.CreateIndex(
                name: "ix_movies_title_year_of_release",
                table: "movies",
                columns: new[] { "title", "year_of_release" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_movies_user_id",
                table: "movies",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_ratings_movie_id",
                table: "ratings",
                column: "movie_id");

            migrationBuilder.CreateIndex(
                name: "ix_ratings_user_id_movie_id",
                table: "ratings",
                columns: new[] { "user_id", "movie_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_auth_key",
                table: "users",
                column: "auth_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_metadata",
                table: "users",
                column: "metadata")
                .Annotation("Npgsql:IndexMethod", "gin")
                .Annotation("Npgsql:StorageParameter:gin_pending_list_limit", "2097151");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "genres");

            migrationBuilder.DropTable(
                name: "ratings");

            migrationBuilder.DropTable(
                name: "movies");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
