// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Dapper;
using Movies.Application.Database;
using Movies.Application.Models;

namespace Movies.Application.Repositories;

public class MovieRepository(IDbConnectionFactory connectionFactory) : IMovieRepository {
  /// <inheritdoc />
  public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default) {
    using var connection = await connectionFactory.CreateConnectionAsync(token);
    using var transaction = connection.BeginTransaction();

    try {
      var id = await connection.ExecuteScalarAsync(new CommandDefinition(
        """
        INSERT INTO "public"."movies" ("title", "yearOfRelease", "slug") 
        VALUES (@Title, @YearOfRelease, @Slug) RETURNING "id"
        """, movie, cancellationToken: token
      ));

      if (id != null) {
        movie.Id = (long)id;

        foreach (var genre in movie.Genres) {
          await connection.ExecuteAsync(new CommandDefinition(
            """
            INSERT INTO "public"."genres" ("movieId", "name")
            VALUES (@MovieId, @Name)
            """, new Genre { MovieId = (long)id, Name = genre }, cancellationToken: token
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

  /// <inheritdoc />
  public async Task<Movie?> GetByIdAsync(long id, CancellationToken token = default) {
    using var connection = await connectionFactory.CreateConnectionAsync(token);
    var movie = await connection.QuerySingleOrDefaultAsync<Movie>(new CommandDefinition(
      """
      SELECT * FROM "public"."movies" "t" WHERE "t"."id" = @id
      """, new { id }, cancellationToken: token
    ));

    if (movie is null) return null;

    var genres = await connection.QueryAsync<string>(new CommandDefinition(
      """
      SELECT "name" FROM "public"."genres" "t" WHERE "t"."movieId" = @movieId
      """, new { movieId = movie.Id }, cancellationToken: token
    ));

    movie.Genres.AddRange(genres);

    return movie;
  }

  /// <inheritdoc />
  public async Task<Movie?> GetBySlugAsync(string slug, CancellationToken token = default) {
    using var connection = await connectionFactory.CreateConnectionAsync(token);
    var movie = await connection.QuerySingleOrDefaultAsync<Movie>(new CommandDefinition(
      """
      SELECT * FROM "public"."movies" "t" WHERE "t"."slug" = @slug
      """, new { slug }, cancellationToken: token
    ));

    if (movie is null) return null;

    var genres = await connection.QueryAsync<string>(new CommandDefinition(
      """
      SELECT "name" FROM "public"."genres" "t" WHERE "t"."movieId" = @movieId
      """, new { movieId = movie.Id }, cancellationToken: token
    ));

    movie.Genres.AddRange(genres);

    return movie;
  }

  /// <inheritdoc />
  public async Task<IEnumerable<Movie>> GetAllAsync(CancellationToken token = default) {
    using var connection = await connectionFactory.CreateConnectionAsync(token);
    var result = await connection.QueryAsync(new CommandDefinition(
      """
      SELECT "t".*, string_agg("g"."name", ',') as "genres"
      FROM "public"."movies" "t"
      LEFT JOIN "public"."genres" g on "g"."movieId" = "t"."id"
      GROUP BY "t"."id" ORDER BY "t"."id" DESC
      """, cancellationToken: token
    ));

    return result.Select(x => new Movie {
      Id = (long)x.id,
      Title = x.title,
      YearOfRelease = x.yearOfRelease,
      Genres = Enumerable.ToList(x.genres.Split(","))
    });
  }

  /// <inheritdoc />
  public async Task<bool> UpdateAsync(Movie movie, CancellationToken token = default) {
    using var connection = await connectionFactory.CreateConnectionAsync(token);
    using var transaction = connection.BeginTransaction();

    try {
      await connection.ExecuteAsync(new CommandDefinition(
        """
        DELETE FROM "public"."genres" "t" WHERE "t"."movieId" = @id
        """, new { id = movie.Id }, cancellationToken: token
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
        """, movie, cancellationToken: token
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

  /// <inheritdoc />
  public async Task<bool> DeleteByIdAsync(long id, CancellationToken token = default) {
    using var connection = await connectionFactory.CreateConnectionAsync(token);
    using var transaction = connection.BeginTransaction();

    try {
      await connection.ExecuteAsync(new CommandDefinition(
        """
        DELETE FROM "public"."genres" "t" WHERE "t"."movieId" = @id
        """, new { id }, cancellationToken: token
      ));

      var result = await connection.ExecuteAsync(new CommandDefinition(
        """
          DELETE FROM "public"."movies" "m" WHERE "m"."id" = @id
        """, new { id }, cancellationToken: token
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

  /// <inheritdoc />
  public async Task<bool> ExistsByIdAsync(long id, CancellationToken token = default) {
    using var connection = await connectionFactory.CreateConnectionAsync(token);
    var exists = await connection.ExecuteScalarAsync(new CommandDefinition(
      """
        SELECT EXISTS(SELECT 1 FROM "public"."movies" "m" WHERE "m"."id" = @Id)
      """, new { Id = id }, cancellationToken: token
    ));

    return exists is not null;
  }
}