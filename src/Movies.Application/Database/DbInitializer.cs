// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Dapper;

namespace Movies.Application.Database;

public class DbInitializer(IDbConnectionFactory connectionFactory) {
  private readonly IDbConnectionFactory _connectionFactory = connectionFactory;

  public async Task InitializeAsync() {
    using var connection = await _connectionFactory.CreateConnectionAsync();
    await connection.ExecuteAsync("""
      CREATE TABLE IF NOT EXISTS "public"."movies" (
        "id" serial8 NOT NULL,
        "title" varchar(60) NOT NULL,
        "yearOfRelease" integer NOT NULL,
        "slug" varchar(255),
        "genres" varchar[],
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

      COMMENT ON COLUMN "public"."movies"."genres" IS 'Genres';

      COMMENT ON CONSTRAINT "UNQ_Title_YoR" ON "public"."movies" IS 'Title and Year should be unique';

      COMMENT ON CONSTRAINT "UNQ_slug" ON "public"."movies" IS 'Slug is always unique (alternative to ID)';

      COMMENT ON INDEX "public"."IDX_title" IS 'Index needed due to search in titles';
      """);
  }
}