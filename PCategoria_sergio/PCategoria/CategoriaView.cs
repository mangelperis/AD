using System;

namespace PCategoria
{
	public partial class CategoriaView : Gtk.Window
	{
		public CategoriaView () : base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
		}

		public CategoriaView(object id) : this(){
			string pregunta = "Editando fila con id : '{0}'";
			pregunta = string.Format(pregunta, id); 
			entryNombre.Text = pregunta;
		}
	}
}

