using System;
using Gtk;
using System.Data;


using SerpisAd;

public partial class MainWindow: Gtk.Window
{	
	private IDbConnection dbConnection;
	private ListStore listStoreArt;
	private ListStore listStoreCat;

	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();

		dbConnection = App.Instance.DbConnection;
		//OPCION STRING
		//treeview.AppendColumn ("precio", new CellRendererText (), "text", 0);

//		ListStore listStore = new ListStore (typeof(string));
//		object value = new decimal (1.2).ToString ();
//		listStore.AppendValues (value);

		//OPCION 2 (DECIMAL por DataFunc)
		/*treeviewCategoria.AppendColumn ("precio", new CellRendererText (), 
		new TreeCellDataFunc(delegate(TreeViewColumn tree_column, CellRenderer cell, 
		                              TreeModel tree_model, TreeIter iter) {
			CellRendererText cellRendererText = (CellRendererText)cell;
			object value = tree_model.GetValue(iter,0);
			cellRendererText.Text = value.ToString();

		})                       );
		listStore = new ListStore (typeof(decimal));
		object valor = new decimal (1.2);
		listStore.AppendValues (valor);
		treeviewCategoria.Model = listStore;*/


		deleteArt.Sensitive = false;
		deleteCat.Sensitive = false;
		editarArt.Sensitive = false;
		editarCat.Sensitive = false;


		//ARTICULO
		treeviewArticulo.AppendColumn ("ID", new CellRendererText (), "text", 0);
		treeviewArticulo.AppendColumn ("Nombre", new CellRendererText (), "text", 1);
		treeviewArticulo.AppendColumn ("Categoria", new CellRendererText (), "text", 2);
		treeviewArticulo.AppendColumn ("Precio", new CellRendererText (), "text", 3);

		listStoreArt = new ListStore (typeof(string), typeof(string), typeof(string), typeof(string));
		treeviewArticulo.Model = listStoreArt;

		//CATEGORIA
		treeviewCategoria.AppendColumn ("ID", new CellRendererText (), "text", 0);
		treeviewCategoria.AppendColumn ("Nombre", new CellRendererText (), "text", 1);


		listStoreCat = new ListStore (typeof(string), typeof(string));
		treeviewCategoria.Model = listStoreCat;

		//TODO SELECCION
		/*treeView.Selection.Changed += selectionChanged;
		treeView.Selection.Changed += selectionChanged;*/

	}
	//TODO SELECCION
	/*
	private void selectionChangedArt (object sender, EventArgs e) {
		Console.WriteLine ("selectionChanged");
		bool hasSelected = treeView.Selection.CountSelectedRows () > 0;
		deleteAction.Sensitive = hasSelected;
		editAction.Sensitive = hasSelected;
	}*/

	private void fillListStoreArt() {
		IDbCommand dbCommand = dbConnection.CreateCommand ();
		dbCommand.CommandText = "select * from articulo";

		IDataReader dataReader = dbCommand.ExecuteReader ();
		while (dataReader.Read()) {
			object id = dataReader ["id"].ToString();
			object nombre = dataReader ["nombre"];
			object categoria = dataReader ["categoria"].ToString();
			object precio = dataReader ["precio"].ToString();
			listStoreArt.AppendValues (id, nombre, categoria,precio);
		}
		dataReader.Close ();
	}

	private void fillListStoreCat() {
		IDbCommand dbCommand = dbConnection.CreateCommand ();
		dbCommand.CommandText = "select * from categoria";

		IDataReader dataReader = dbCommand.ExecuteReader ();
		while (dataReader.Read()) {
			object id = dataReader ["id"].ToString();
			object nombre = dataReader ["nombre"];

			listStoreCat.AppendValues (id, nombre);
		}
		dataReader.Close ();
	}


	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}


	protected void OnRefreshArticulo1Activated (object sender, EventArgs e)
	{
		listStoreArt.Clear ();
		fillListStoreArt ();
	}

	protected void OnRefreshCatActivated (object sender, EventArgs e)
	{
		listStoreCat.Clear ();
		fillListStoreCat ();
	}
	// ESTOS 3 METODOS ME TOCA DECLARARLOS VACIOS PORQUE SE VE QUE MES LOS HA GUARDADO DE ANTES A LA HORA DE CONSTRUIR
	// Y SI NO DA ERROR SI NO LOS ENCUENTRA
	protected void OnRefreshCategoriaActivated (object sender, EventArgs e)
	{
	}
	protected void OnRefreshActionActivated (object sender, EventArgs e)
	{
	}
	protected void OnRefreshAction1Activated (object sender, EventArgs e)
	{}
     //	


	protected void OnNewArticulo1Activated (object sender, EventArgs e)
	{

		/*string insertSql = string.Format(
			"insert into articulo (nombre,categoria,precio) values ('uno','dos','tres')"
			);
		Console.WriteLine ("insertSql={0}", insertSql);
		IDbCommand dbCommand = dbConnection.CreateCommand ();
		dbCommand.CommandText = insertSql;

		dbCommand.ExecuteNonQuery ();*/
		Console.WriteLine ("TODO");

	}
	protected void OnNewCategoria1Activated (object sender, EventArgs e)
	{
		Console.WriteLine ("TODO");
	}



	protected void OnEditarArtActivated (object sender, EventArgs e)
	{
		Console.WriteLine ("TODO");
	}

	protected void OnEditarCatActivated (object sender, EventArgs e)
	{
		Console.WriteLine ("TODO");
	}



	protected void OnDeleteArtActivated (object sender, EventArgs e)
	{
		Console.WriteLine ("TODO");
	}



	protected void OnDeleteCatActivated (object sender, EventArgs e)
	{
		Console.WriteLine ("TODO");
	}
}
