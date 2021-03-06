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
	

		listStore = new ListStore (typeof(ulong), typeof(string));

		treeView.Model = listStore;

	}

	/*
	 treeView.Selection.Changed += delegate {
	deleteAction.Sensitive = treeView.Selection.CountSelectedRows() > 0;
	}
	 */

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		mySqlConnection.Close ();
		Application.Quit ();
		a.RetVal = true;


	}
	protected void OnAddActionActivated (object sender, EventArgs e)
	{
		string insertSql = string.Format (
			"INSERT INTO categoria (nombre) values ('{0}')",
			" Nuevo " + DateTime.Now
			);
		MySqlCommand mySqlCommand = mySqlConnection.CreateCommand ();
		mySqlCommand.CommandText = insertSql;

		mySqlCommand.ExecuteNonQuery ();





	}

	private void fillListStore() {
	
			MySqlCommand mySqlCommand = mySqlConnection.CreateCommand ();

			mySqlCommand.CommandText = ("Select * from Categoria");
			MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader ();

			while (mySqlDataReader.Read()) {
				object id = mySqlDataReader ["id"];
				object nombre = mySqlDataReader ["nombre"];
				//id = id.ToString ();

				listStore.AppendValues (id, nombre);
			}
			mySqlDataReader.Close ();
			
	}
	//TODO BOTON AÑADIR SOLO AÑADE 1 REGISTRO (A LA BDD TAMBIEN) 
	//TODO BOTON REFRESH VUELVE A CARGAR TODOS LOS DATOS OTRA VEZ
	//TODO POSIBILIDAD DE EDITAR

	protected void OnRefreshActionActivated (object sender, EventArgs e)
	{
		listStore.Clear ();
		fillListStore ();

			}


	protected void OnDeleteActionActivated (object sender, EventArgs e)
	{
		throw new NotImplementedException ();
	}
}
