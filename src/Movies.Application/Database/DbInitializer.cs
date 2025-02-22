// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Dapper;

namespace Movies.Application.Database;

public class DbInitializer(IDbConnectionFactory connectionFactory) {
  public async Task InitializeAsync() {
    using var connection = await connectionFactory.CreateConnectionAsync();
    await connection.ExecuteAsync(
      """
      CREATE TABLE IF NOT EXISTS "public"."movies" (
        "id" serial8 NOT NULL,
        "title" varchar(60) NOT NULL,
        "yearOfRelease" integer NOT NULL,
        "slug" varchar(255),
        PRIMARY KEY ("id"),
        CONSTRAINT "UNQ_Title_YoR" UNIQUE ("title", "yearOfRelease"),
        CONSTRAINT "UNQ_slug" UNIQUE ("slug")
      );

      CREATE INDEX IF NOT EXISTS "IDX_title" ON "public"."movies" USING btree (
        "title"
      );

      COMMENT ON COLUMN "public"."movies"."id" IS 'ID';
      COMMENT ON COLUMN "public"."movies"."title" IS 'Title';
      COMMENT ON COLUMN "public"."movies"."yearOfRelease" IS 'Year of Release';
      COMMENT ON COLUMN "public"."movies"."slug" IS 'Slug';
      COMMENT ON CONSTRAINT "UNQ_Title_YoR" ON "public"."movies" IS 'Title and Year must be unique';
      COMMENT ON CONSTRAINT "UNQ_slug" ON "public"."movies" IS 'Slug is always unique (alternative to ID)';
      COMMENT ON INDEX "public"."IDX_title" IS 'Index needed due to search between titles';
      """);

    await connection.ExecuteAsync(
      """
        CREATE TABLE IF NOT EXISTS "public"."genres" (
        "id" serial8 NOT NULL,
        "movieId" int8 NOT NULL,
        "name" varchar(60) NOT NULL,
        PRIMARY KEY ("id"),
        CONSTRAINT "FK_movies_id_genres_movie_id" FOREIGN KEY ("movieId") 
          REFERENCES "public"."movies" ("id") ON DELETE CASCADE ON UPDATE CASCADE,
        CONSTRAINT "UNQ_movie_name_genre" UNIQUE ("movieId", "name")
      );

      COMMENT ON COLUMN "public"."genres"."id" IS 'ID';
      COMMENT ON COLUMN "public"."genres"."movieId" IS 'Movie ID';
      COMMENT ON COLUMN "public"."genres"."name" IS 'Name';
      COMMENT ON CONSTRAINT "FK_movies_id_genres_movie_id" ON "public"."genres" IS 'Reference to Movie ID';
      COMMENT ON CONSTRAINT "UNQ_movie_name_genre" ON "public"."genres" IS 'Movie ID and Genre name must be unique';
      """);
  }
}