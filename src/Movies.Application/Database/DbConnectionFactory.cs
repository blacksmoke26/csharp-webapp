// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using System.Data;
using Npgsql;

namespace Movies.Application.Database;

public interface IDbConnectionFactory {
  Task<IDbConnection> CreateConnectionAsync();
}

public class NpgsqlConnectionFactory(string connectionString) : IDbConnectionFactory {
  public async Task<IDbConnection> CreateConnectionAsync() {
    var connection = new NpgsqlConnection(connectionString);
    await connection.OpenAsync();
    return connection;
  }
}