using System;
using Gtk;
using System.Data;

using PArticulo;
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

		//BOTONES EDITAR Y ELIMINAR
		deleteArt.Sensitive = false;
		deleteCat.Sensitive = false;
		editarArt.Sensitive = false;
		editarCat.Sensitive = false;


		//ARTICULO
		treeviewArticulo.AppendColumn ("ID", new CellRendererText (), "text", 0);
		treeviewArticulo.AppendColumn ("Nombre", new CellRendererText (), "text", 1);
		treeviewArticulo.AppendColumn ("Categoria", new CellRendererText (), "text", 2);
		//treeviewArticulo.AppendColumn ("Precio", new CellRendererText (), "text", 3);
		treeviewArticulo.AppendColumn ("precio", new CellRendererText (), 
		                               new TreeCellDataFunc (delegate(TreeViewColumn tree_column, CellRenderer cell, 
					                   TreeModel tree_model , TreeIter iter) {
			CellRendererText cellRendererText = (CellRendererText)cell;
			object value = tree_model.GetValue (iter, 3);
			cellRendererText.Text = value != DBNull.Value ? value.ToString () : "null";
		}));

		listStoreArt = new ListStore (typeof(string), typeof(string), typeof(string), typeof(decimal));//typeof(string) -> precio
		treeviewArticulo.Model = listStoreArt;

			//SELECCION 
			treeviewArticulo.Selection.Changed += delegate {
				bool hasSelected = treeviewArticulo.Selection.CountSelectedRows() > 0;
				deleteArt.Sensitive = hasSelected;
				editarArt.Sensitive = hasSelected;
			};



		//CATEGORIA
		treeviewCategoria.AppendColumn ("ID", new CellRendererText (), "text", 0);
		treeviewCategoria.AppendColumn ("Nombre", new CellRendererText (), "text", 1);


		listStoreCat = new ListStore (typeof(string), typeof(string));
		treeviewCategoria.Model = listStoreCat;

			//SELECCION
			treeviewCategoria.Selection.Changed += delegate {
				bool hasSelected = treeviewCategoria.Selection.CountSelectedRows() > 0;
				deleteCat.Sensitive = hasSelected;
				editarCat.Sensitive = hasSelected;
			};


	}
	//BOTON REFRESH

	private void fillListStoreArt() {
		IDbCommand dbCommand = dbConnection.CreateCommand ();
		dbCommand.CommandText = "select * from articulo";

		IDataReader dataReader = dbCommand.ExecuteReader ();
		while (dataReader.Read()) {
			object id = dataReader ["id"].ToString();
			object nombre = dataReader ["nombre"];
			object categoria = dataReader ["categoria"].ToString();
			object precio = dataReader ["precio"];//QUITAR TO STRING para solucion 2
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

	//BOTON BORRAR 
	protected void OnDeleteArtActivated (object sender, EventArgs e)
	{
		TreeIter treeIter;
		treeviewArticulo.Selection.GetSelected (out treeIter);
		object id=listStoreArt.GetValue (treeIter, 0);

		string pregunta = "¿Borrar el registro '{0}'?";
		pregunta = string.Format(pregunta, id); 


		if (Confirm(pregunta)) {

			IDbCommand mySqlCommand = dbConnection.CreateCommand ();
			string deleteSql = "DELETE FROM articulo WHERE id = '{0}'";
			deleteSql= string.Format(deleteSql, id);
			mySqlCommand.CommandText = deleteSql;
			mySqlCommand.ExecuteNonQuery ();

		} 
	}

	protected void OnDeleteCatActivated (object sender, EventArgs e)
	{
		TreeIter treeIter;
		treeviewCategoria.Selection.GetSelected (out treeIter);
		object id=listStoreCat.GetValue (treeIter, 0);

		string pregunta = "¿Borrar  el registro '{0}'?";
		pregunta = string.Format(pregunta, id); 


		if (Confirm(pregunta)) {

			IDbCommand mySqlCommand = dbConnection.CreateCommand ();
			string deleteSql = "DELETE FROM categoria WHERE id = '{0}'";
			deleteSql= string.Format(deleteSql, id);
			mySqlCommand.CommandText = deleteSql;
			mySqlCommand.ExecuteNonQuery ();

		} 
	}
	//PARTE DEL DIALOGO DE 'CONFIRMAR'  PARA BORRAR
	public bool Confirm(string text){
		MessageDialog messageDialog = new MessageDialog (
			this,
			DialogFlags.Modal,
			MessageType.Question,
			ButtonsType.YesNo,
			text
			);
		messageDialog.Title = Title;
		ResponseType result = (ResponseType)messageDialog.Run ();
		messageDialog.Destroy ();
		return result == ResponseType.Yes;
	}



	//BOTON AÑADIR

	protected void OnNewArticulo1Activated (object sender, EventArgs e)
	{

		string insertSql = string.Format(
			"INSERT INTO articulo (nombre) VALUES ('{0}')", "Nuevo ART: " + DateTime.Now
			);

		IDbCommand dbCommand = dbConnection.CreateCommand ();
		dbCommand.CommandText = insertSql;

		dbCommand.ExecuteNonQuery ();
		Console.WriteLine ("Registro Añadido. SQL :: {0}", insertSql);

	}
	protected void OnNewCategoria1Activated (object sender, EventArgs e)
	{
		string insertSql = string.Format(
			"INSERT INTO categoria (nombre) VALUES ('{0}')", "Nueva CAT: " + DateTime.Now
			);

		IDbCommand dbCommand = dbConnection.CreateCommand ();
		dbCommand.CommandText = insertSql;

		dbCommand.ExecuteNonQuery ();
		Console.WriteLine ("Registro Añadido. SQL :: {0}", insertSql);
	}

	// BOTON REFRESH


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
	// Y SI NO DA ERROR SI NO LOS ENCUENTRA. NO LOS PUEDO BORRAR
	protected void OnRefreshCategoriaActivated (object sender, EventArgs e)
	{
	}
	protected void OnRefreshActionActivated (object sender, EventArgs e)
	{
	}
	protected void OnRefreshAction1Activated (object sender, EventArgs e)
	{}
     //	

	// BOTON EDITAR
	protected void OnEditarArtActivated (object sender, EventArgs e)
	{
		TreeIter treeIter;
		treeviewArticulo.Selection.GetSelected (out treeIter);
		object id = listStoreArt.GetValue (treeIter, 0);
		new ArticuloWindow (id);
	}

	protected void OnEditarCatActivated (object sender, EventArgs e)
	{
		TreeIter treeIter;
		treeviewCategoria.Selection.GetSelected (out treeIter);
		object id = listStoreCat.GetValue (treeIter, 0);
		new CategoriaWindow (id);
	}

	//CLOSE APP
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{		//CERRAR CONEXION CON BDD
			dbConnection.Close ();
		Application.Quit ();
		a.RetVal = true;
	}



}
