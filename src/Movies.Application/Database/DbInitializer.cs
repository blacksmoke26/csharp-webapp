// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Dapper;

namespace Movies.Application.Database;

public class DbInitializer(IDbConnectionFactory connectionFactory) {
  public async Task InitializeAsync() {
    using var connection = await connectionFactory.CreateConnectionAsync();
    Console.WriteLine("Running migrations");
    
    await connection.ExecuteAsync(
      """
      CREATE TABLE IF NOT EXISTS "public"."users" (
        "id" serial8 NOT NULL,
        "first_name" varchar(15) NOT NULL,
        "last_name" varchar(15) NOT NULL,
        "password" varchar(64) NOT NULL,
        "email" varchar(255),
        "auth_key" varchar(64) NOT NULL,
        "role" varchar(20) NOT NULL,
        "status" int2,
        "metadata" jsonb DEFAULT '{}',
        "created_at" varchar(255),
        "updated_at" date,
        PRIMARY KEY ("id"),
        CONSTRAINT "UNQ_users_email" UNIQUE ("email")
      );

      CREATE INDEX IF NOT EXISTS "IDX_users_metadata" ON "public"."users" USING gin (
        "metadata"
      ) WITH (GIN_PENDING_LIST_LIMIT = 2097151);

      COMMENT ON COLUMN "public"."users"."id" IS 'ID';

      COMMENT ON COLUMN "public"."users"."first_name" IS 'First name';

      COMMENT ON COLUMN "public"."users"."last_name" IS 'Last name';

      COMMENT ON COLUMN "public"."users"."password" IS 'Password';

      COMMENT ON COLUMN "public"."users"."email" IS 'Email address';

      COMMENT ON COLUMN "public"."users"."auth_key" IS 'Authorization Key';

      COMMENT ON COLUMN "public"."users"."role" IS 'Role';

      COMMENT ON COLUMN "public"."users"."status" IS 'Status';

      COMMENT ON COLUMN "public"."users"."metadata" IS 'Metadata';

      COMMENT ON COLUMN "public"."users"."created_at" IS 'Created';

      COMMENT ON COLUMN "public"."users"."updated_at" IS 'Updated';
      """);
    
    await connection.ExecuteAsync(
      """
      CREATE TABLE IF NOT EXISTS "public"."movies" (
        "id" serial8 NOT NULL,
        "user_id" int8 NOT NULL,
        "title" varchar(60) COLLATE "pg_catalog"."default" NOT NULL,
        "year_of_release" int4 NOT NULL,
        "slug" varchar(255) COLLATE "pg_catalog"."default",
        CONSTRAINT "movies_pkey" PRIMARY KEY ("id"),
        CONSTRAINT "FK_movies_user_id_users_id" FOREIGN KEY ("user_id") REFERENCES "public"."users" ("id") ON DELETE CASCADE ON UPDATE CASCADE,
        CONSTRAINT "UNQ_Title_YoR" UNIQUE ("title", "year_of_release"),
        CONSTRAINT "UNQ_slug" UNIQUE ("slug")
      );

      CREATE INDEX IF NOT EXISTS "IDX_movies_title" ON "public"."movies" USING btree (
        "title" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
      );

      COMMENT ON COLUMN "public"."movies"."id" IS 'ID';

      COMMENT ON COLUMN "public"."movies"."user_id" IS 'User';

      COMMENT ON COLUMN "public"."movies"."title" IS 'Title';

      COMMENT ON COLUMN "public"."movies"."year_of_release" IS 'Year of Release';

      COMMENT ON COLUMN "public"."movies"."slug" IS 'Slug';

      COMMENT ON CONSTRAINT "UNQ_Title_YoR" ON "public"."movies" IS 'Title and Year must be unique';

      COMMENT ON CONSTRAINT "UNQ_slug" ON "public"."movies" IS 'Slug is always unique (alternative to ID)';

      COMMENT ON INDEX "public"."IDX_movies_title" IS 'Index needed due to search between titles';
      """);
    
    await connection.ExecuteAsync(
      """
      CREATE TABLE IF NOT EXISTS "public"."genres" (
        "id" serial8 NOT NULL,
        "movie_id" int8 NOT NULL,
        "name" varchar(60) NOT NULL,
        CONSTRAINT "genres_pkey" PRIMARY KEY ("id"),
        CONSTRAINT "FK_genres_movie_id_movies_id" FOREIGN KEY ("movie_id") REFERENCES "public"."movies" ("id") ON DELETE CASCADE ON UPDATE CASCADE,
        CONSTRAINT "UNQ_movie_name_genre" UNIQUE ("name", "movie_id")
      );

      COMMENT ON COLUMN "public"."genres"."id" IS 'ID';

      COMMENT ON COLUMN "public"."genres"."movie_id" IS 'Movie';

      COMMENT ON COLUMN "public"."genres"."name" IS 'Name';

      COMMENT ON CONSTRAINT "FK_genres_movie_id_movies_id" ON "public"."genres" IS 'Reference to Movie ID';

      COMMENT ON CONSTRAINT "UNQ_movie_name_genre" ON "public"."genres" IS 'Movie ID and Genre name must be unique';
      """);

    await connection.ExecuteAsync(
      """
      CREATE TABLE IF NOT EXISTS "public"."ratings" (
        "id" serial8 NOT NULL,
        "user_id" int8 NOT NULL,
        "movie_id" int8 NOT NULL,
        "rating" int2 NOT NULL DEFAULT 1,
        "feedback" text,
        "created_at" timestamp,
        "updated_at" timestamp,
        PRIMARY KEY ("id"),
        CONSTRAINT "FK_ratings_movie_id_movies_id" FOREIGN KEY ("movie_id") REFERENCES "public"."movies" ("id") ON DELETE CASCADE ON UPDATE CASCADE,
        CONSTRAINT "FK_ratings_user_id_users_id" FOREIGN KEY ("user_id") REFERENCES "public"."users" ("id") ON DELETE CASCADE ON UPDATE CASCADE,
        CONSTRAINT "IDX_ratings_user_id_movie_id" UNIQUE ("user_id", "movie_id")
      );

      COMMENT ON COLUMN "public"."ratings"."id" IS 'ID';

      COMMENT ON COLUMN "public"."ratings"."user_id" IS 'User';

      COMMENT ON COLUMN "public"."ratings"."movie_id" IS 'Movie';

      COMMENT ON COLUMN "public"."ratings"."rating" IS 'Rating';

      COMMENT ON COLUMN "public"."ratings"."feedback" IS 'Feedback';

      COMMENT ON COLUMN "public"."ratings"."created_at" IS 'Created';

      COMMENT ON COLUMN "public"."ratings"."updated_at" IS 'Updated';

      COMMENT ON CONSTRAINT "IDX_ratings_user_id_movie_id" ON "public"."ratings" IS 'Movie and User should be unique';
      """);
  }
}