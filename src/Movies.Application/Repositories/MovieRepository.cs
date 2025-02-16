// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Dapper;
using Movies.Application.Database;
using Movies.Application.Models;

namespace Movies.Application.Repositories;

public class MovieRepository(IDbConnectionFactory connectionFactory) : IMovieRepository {
  public async Task<bool> CreateAsync(Movie movie) {
    using var connection = await connectionFactory.CreateConnectionAsync();
    using var transaction = connection.BeginTransaction();

    try {
      var id = await connection.ExecuteScalarAsync(new CommandDefinition(
        """
        INSERT INTO "public"."movies" ("title", "yearOfRelease", "slug") 
        VALUES (@Title, @YearOfRelease, @Slug) RETURNING "id"
        """, movie
      ));

      if (id != null) {
        movie.Id = (long)id;

        foreach (var genre in movie.Genres) {
          await connection.ExecuteAsync(new CommandDefinition(
            """
            INSERT INTO "public"."genres" ("movieId", "name")
            VALUES (@MovieId, @Name)
            """, new Genre { MovieId = (long)id, Name = genre }
          ));
        }
      }

      transaction.Commit();
      return id != null;
    }
    catch (Exception e) {
      transaction.Rollback();
      Console.WriteLine(e);
      throw;
    }
  }

  public async Task<Movie?> GetByIdAsync(long id) {
    using var connection = await connectionFactory.CreateConnectionAsync();
    var movie = await connection.QuerySingleOrDefaultAsync<Movie>(new CommandDefinition(
      """
      SELECT * FROM "public"."movies" "t" WHERE "t"."id" = @id
      """, new { id }
    ));

    if (movie is null) return null;

    var genres = await connection.QueryAsync<string>(new CommandDefinition(
      """
      SELECT "name" FROM "public"."genres" "t" WHERE "t"."movieId" = @movieId
      """, new { movieId = movie.Id }
    ));

    movie.Genres.AddRange(genres);

    return movie;
  }

  public async Task<Movie?> GetBySlugAsync(string slug) {
    using var connection = await connectionFactory.CreateConnectionAsync();
    var movie = await connection.QuerySingleOrDefaultAsync<Movie>(new CommandDefinition(
      """
      SELECT * FROM "public"."movies" "t" WHERE "t"."slug" = @slug
      """, new { slug }
    ));

    if (movie is null) return null;

    var genres = await connection.QueryAsync<string>(new CommandDefinition(
      """
      SELECT "name" FROM "public"."genres" "t" WHERE "t"."movieId" = @movieId
      """, new { movieId = movie.Id }
    ));

    movie.Genres.AddRange(genres);

    return movie;
  }

  public async Task<IEnumerable<Movie>> GetAllAsync() {
    using var connection = await connectionFactory.CreateConnectionAsync();
    var result = await connection.QueryAsync(new CommandDefinition(
      """
      SELECT "t".*, string_agg("g"."name", ',') as "genres"
      FROM "public"."movies" "t"
      LEFT JOIN "public"."genres" g on "g"."movieId" = "t"."id"
      GROUP BY "t"."id"
      """
    ));

    return result.Select(x => new Movie {
      Id = (long)x.id,
      Title = x.title,
      YearOfRelease = x.yearOfRelease,
      Genres = Enumerable.ToList(x.genres.Split(","))
    });
  }

  public async Task<bool> UpdateAsync(Movie movie) {
    using var connection = await connectionFactory.CreateConnectionAsync();
    using var transaction = connection.BeginTransaction();

    try {
      await connection.ExecuteAsync(new CommandDefinition(
        """
        DELETE FROM "public"."genres" "t" WHERE "t"."movieId" = @id
        """, new { id = movie.Id }
      ));

      foreach (var genre in movie.Genres) {
        await connection.ExecuteAsync(new CommandDefinition(
          """
          INSERT INTO "public"."genres" ("movieId", "name")
          VALUES (@MovieId, @Name)
          """, new Genre { MovieId = (long)movie.Id!, Name = genre }
        ));
      }

      var result = await connection.ExecuteAsync(new CommandDefinition(
        """
        UPDATE "public"."movies" AS "t"
        SET "slug" = @Slug, "title" = @Title, "yearOfRelease" = @YearOfRelease
        WHERE "id" = @Id
        """, movie
      ));

      transaction.Commit();
      return result > 0;
    }
    catch (Exception e) {
      transaction.Rollback();
      Console.WriteLine(e);
      throw;
    }
  }

  public async Task<bool> DeleteByIdAsync(long id) {
    using var connection = await connectionFactory.CreateConnectionAsync();
    using var transaction = connection.BeginTransaction();

    try {
      await connection.ExecuteAsync(new CommandDefinition(
        """
        DELETE FROM "public"."genres" "t" WHERE "t"."movieId" = @id
        """, new { id }
      ));

      var result = await connection.ExecuteAsync(new CommandDefinition(
        """
          DELETE FROM "public"."movies" "m" WHERE "m"."id" = @id
        """, new { id }
      ));

      transaction.Commit();
      return result > 0;
    }
    catch (Exception e) {
      transaction.Rollback();
      Console.WriteLine(e);
      throw;
    }
  }

  public async Task<bool> ExistsByIdAsync(long id) {
    using var connection = await connectionFactory.CreateConnectionAsync();
    var count = await connection.ExecuteScalarAsync(new CommandDefinition(
      """
        SELECT EXISTS(SELECT 1 FROM "public"."movies" "m" WHERE "m"."id" = @Id)
      """, new { Id = id }
    ));

    Console.WriteLine(count);
    return false;
  }
}