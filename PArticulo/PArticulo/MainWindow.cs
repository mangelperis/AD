using System;
using Gtk;
using System.Data;


using SerpisAd;

public partial class MainWindow: Gtk.Window
{	
	private IDbConnection dbConnection;
	private ListStore listStore;

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
		treeviewCategoria.AppendColumn ("precio", new CellRendererText (), 
		new TreeCellDataFunc(delegate(TreeViewColumn tree_column, CellRenderer cell, 
		                              TreeModel tree_model, TreeIter iter) {
			CellRendererText cellRendererText = (CellRendererText)cell;
			object value = tree_model.GetValue(iter,0);
			cellRendererText.Text = value.ToString();

	})                       );
		listStore = new ListStore (typeof(decimal));
		object valor = new decimal (1.2);
		listStore.AppendValues (valor);
		treeviewCategoria.Model = listStore;
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}





	protected void OnRefreshCategoriaActivated (object sender, EventArgs e)
	{
		throw new NotImplementedException ();
	}
}
