using System;
using MySql.Data.MySqlClient;

namespace PHolaMySql
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");

			string connectionString =
				"Server=localhost;" +
					"Database=dbprueba;" +
					"User ID=root;" +
					"Password=sistemas;";

			MySqlConnection mySqlConnection = new MySqlConnection( connectionString); 
			mySqlConnection.Open ();

			MySqlCommand mySqlCommand = mySqlConnection.CreateCommand ();
			//mySqlCommand.CommandText = string.Format("INSERT INTO categoria (nombre) values ('{0}')", DateTime.Now );
			//mySqlCommand.ExecuteNonQuery ();

			mySqlCommand.CommandText = ("Select * from Categoria");
			MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader ();

			Console.WriteLine ("FieldCount = {0}", mySqlDataReader.FieldCount);
			for (int index = 0; index < mySqlDataReader.FieldCount; index++) {
				Console.WriteLine ("column {0}={1}", index, mySqlDataReader.GetName (index));
			
			};

			while (mySqlDataReader.Read()) {
				object id = mySqlDataReader ["id"];
				object nombre = mySqlDataReader ["nombre"];

				Console.WriteLine ("id = {0}, nombre = {1}", id,nombre);
			}
			mySqlDataReader.Close ();

			mySqlConnection.Close ();
			}
	}
}
