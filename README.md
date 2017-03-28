# Entity Framework Core bug when setting the default value for a nullable int field
This is a reproduction for a bug in Entity Framework Core.

## Environment
* Windows 10 Pro
* Visual Studio 2015 Update 3
* Entity Framework Core 1.1.1
* Entity Framework Core Sql Server 1.1.1

## Repoduce using this example
To reproduce the bug using this example, clone the repository, add a connection string to the appsettings.json file (or add it as an environment variable), and run the console application. It will attempt to construct a working DBContext class and a broken one. The only difference between these DBContext classes is that the working one uses `.HasDefaultValueSql( "null" )` on when building a nullable int field, and the broken one uses `.HasDefaultValue( null )` for the same field. Both classes use `.HasDefaultValue( null )` for nullable boolean and string fields.

## Expected Results
Both methods should produce the same result, and allow the creation of the nullable int field with a default value of null.

## Actual Results
The DBContext that uses `.HasDefaultValueSql( "null" )` works as expected. The DBContext that uses `.HasDefaultValue( null )` produces an exception:
```
Multiple identity columns specified for table 'Test'. Only one identity column per table is allowed.
   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlCommand.RunExecuteNonQueryTds(String methodName, Boolean async, Int32 timeout, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.InternalExecuteNonQuery(TaskCompletionSource`1 completion, Boolean sendToPipe, Int32 timeout, Boolean asyncWrite, String methodName)
   at System.Data.SqlClient.SqlCommand.ExecuteNonQuery()
   at Microsoft.EntityFrameworkCore.Storage.Internal.RelationalCommand.Execute(IRelationalConnection connection, String executeMethod, IReadOnlyDictionary`2 parameterValues, Boolean closeConnection)
   at Microsoft.EntityFrameworkCore.Storage.Internal.RelationalCommand.ExecuteNonQuery(IRelationalConnection connection, IReadOnlyDictionary`2 parameterValues)
   at Microsoft.EntityFrameworkCore.Migrations.Internal.MigrationCommandExecutor.ExecuteNonQuery(IEnumerable`1 migrationCommands, IRelationalConnection connection)
   at Microsoft.EntityFrameworkCore.Storage.RelationalDatabaseCreator.EnsureCreated()
   at efcore_default_null_bug.Program.Main(String[] args) in A:\ghp\default-null-bug\efcore-default-null-bug\src\efcore-default-null-bug\Program.cs:line 46
```

In addition to failing, the exception message is confusing and potentially inaccurate, suggesting that two identity columns have been defined as opposed to suggesting that there is an issue with the default value. This might be because Entity Framework Core is assuming that any int field with a null default value must be an identity or auto-incrementing column.

## Reproduce without example
0. Create a new `netcore` or `netstandard` project and add Entity Framework Core 1.1.1 as a dependency.
0. Create a Model with at a nullable int field `int?`
0. Create a `DBContext` class with the `OnModelCreating()` method overloaded
0. In the `OnModelCreating()` method, set the default value of the nullable int field to be null using `HasDefaultValue( null )`
0. Attempt to run the DBContext Database `EnsureCreated()` method. An exception will be thrown (see above).
0. Change the default value to be `HasDefaultValueSql( "null" )`.
0. The `EnsureCreated()` method will now run successfully.
