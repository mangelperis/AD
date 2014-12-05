using System;
using Gtk;
using System.Collections.Generic;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();

		List<Categoria> categorias = new List<Categoria> ();
		categorias.Add (new Categoria (1, "Uno"));
		categorias.Add (new Categoria (2, "Dos"));
		categorias.Add (new Categoria (3, "Tres"));
		categorias.Add (new Categoria (4, "Cuatro"));
		
	

		int categoriaId = -1;

//		CellRendererText cellRendererText = new CellRendererText ();
//		comboBox.PackStart (cellRendererText, false);
//		comboBox.AddAttribute (cellRendererText, "text", 0);

		CellRendererText cellRendererText2 = new CellRendererText ();
		comboBox.PackStart (cellRendererText2, false);
		comboBox.AddAttribute (cellRendererText2, "text", 1);

		ListStore listStore = new ListStore (typeof(int), typeof(string));
		TreeIter treeIterInicial = listStore.AppendValues (0, "<sin asignar>");

		foreach (Categoria categoria in categorias) {
			TreeIter currentIter = listStore.AppendValues (categoria.Id, categoria.Nombre);
			if (categoria.Id == categoriaId)
				treeIterInicial = currentIter;
		}
//		listStore.AppendValues (1, "Uno");
//		listStore.AppendValues (2, "Dos");


		comboBox.Model = listStore;

		comboBox.SetActiveIter (treeIterInicial);

		TreeIter currentTreeIter;
		listStore.GetIterFirst (out currentTreeIter);
		listStore.GetIterFirst (out currentTreeIter);
		do {
			if() { 
				break;
			}
			while (listStore.IterNext (ref currentTreeIter));
		//listStore.GetValue (currentTreeIter, 0);



		propertiesAction.Activated += delegate {
			TreeIter treeIter;
			bool activeIter = comboBox.GetActiveIter (out treeIter);
			object id = activeIter ? listStore.GetValue (treeIter, 0) : null;
			// ? significa el boolean si tiene algo dentro ejecuta la primera orden y sino, es null.
			Console.WriteLine ("id={0}", id);

		};
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
}

public class Categoria {
	public Categoria (int id, string nombre){
		Id = id;
		Nombre = nombre;
	}
	public int Id { get; set; }
	public string Nombre { get; set; }

}