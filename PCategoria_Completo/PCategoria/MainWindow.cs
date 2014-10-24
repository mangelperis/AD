using MySql.Data.MySqlClient;

using System;
using Gtk;


public partial class MainWindow: Gtk.Window
{	
	private ListStore listStore;

	private MySqlConnection mySqlConnection;


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
		fillListStore ();

		treeView.Selection.Changed += selectionChanged;

	}

	void selectionChanged (object sender, EventArgs e)
	{
		Console.WriteLine ("selectionChanged");
		bool hasSelected  = treeView.Selection.CountSelectedRows () > 0;
		deleteAction.Sensitive = hasSelected;
		editAction.Sensitive = hasSelected;

	}


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
		Console.WriteLine ("insertSql={0}", insertSql);
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


	protected void OnRefreshActionActivated (object sender, EventArgs e)
	{
		listStore.Clear ();
		fillListStore ();

	}


	protected void OnDeleteActionActivated (object sender, EventArgs e)
	{
		//TODO confirmar el delete

		MessageDialog messageDialog = new MessageDialog (
			this,
			DialogFlags.Modal,
			MessageType.Question,
			ButtonsType.YesNo,
			"¿Quieres eliminar el registro?"

		);

		ResponseType response = (ResponseType) messageDialog.Run ();
		messageDialog.Destroy ();

		if (response != ResponseType.Yes)
			return;

		//

		TreeIter treeIter;
		treeView.Selection.GetSelected (out treeIter);
		object id = listStore.GetValue (treeIter, 0);

		string deleteSql = string.Format ("delete from categoria where id = {0}", id);
		MySqlCommand mySqlCommand = mySqlConnection.CreateCommand ();
		mySqlCommand.CommandText = deleteSql;

		mySqlCommand.ExecuteNonQuery ();

	}
	protected void OnEditActionActivated (object sender, EventArgs e)
	{
		throw new NotImplementedException ();
	}



}
