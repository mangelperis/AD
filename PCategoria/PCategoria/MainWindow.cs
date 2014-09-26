using MySql.Data.MySqlClient;

using System;
using Gtk;


public partial class MainWindow: Gtk.Window
{	
	private ListStore listStore;

	private MySqlConnection mySqlConnection;
	private int cont = 0;



	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();

		string connectionString =
				"Server=localhost;" +
				"Database=dbprueba;" +
				"User ID=root;" +
				"Password=sistemas;";
		mySqlConnection = new MySqlConnection( connectionString); 
		mySqlConnection.Open ();


		treeView.AppendColumn ("ID", new CellRendererText (), "text", 0);
		treeView.AppendColumn ("NOMBRE", new CellRendererText (), "text", 1);
	

		listStore = new ListStore (typeof(string), typeof(string));

		treeView.Model = listStore;

	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;

		mySqlConnection.Close ();
	}
	protected void OnAddActionActivated (object sender, EventArgs e)
	{
		if (cont > 0) {
			//NADA , quiere decir que ya he pulsado el boton antes


		} else {
			MySqlCommand mySqlCommand = mySqlConnection.CreateCommand ();

			mySqlCommand.CommandText = ("Select * from Categoria");
			MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader ();

			while (mySqlDataReader.Read()) {
				object id = mySqlDataReader ["id"];
				object nombre = mySqlDataReader ["nombre"];
				id = id.ToString ();

				listStore.AppendValues (id, nombre);
			}
			mySqlDataReader.Close ();
			cont++;
		}





	}



	protected void OnRefreshActionActivated (object sender, EventArgs e)
	{
		throw new NotImplementedException ();
	}
	protected void OnRefreshActionActivated (object sender, EventArgs e)
	{
		throw new NotImplementedException ();
	}
}
