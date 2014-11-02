using System;
using System.Data;
using SerpisAd;
using PArticulo;
namespace PArticulo
{
	public partial class ArticuloWindow : Gtk.Window
	{
		private object id;

		public ArticuloWindow () : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
		}
		//CONSTRUCTOR QUE RECIBE EL ID DEL ITEM QUE VENGO EN MAIN

		public ArticuloWindow(object id) : this(){

			this.id = id;
			//TODO ¿DESMARCAR BOTON GUARDAR  POR DEFECTO?


			IDbCommand dbCommand =	App.Instance.DbConnection.CreateCommand ();
			dbCommand.CommandText = String.Format ("SELECT * FROM articulo WHERE id = {0}", id);

			IDataReader dataReader = dbCommand.ExecuteReader ();
			dataReader.Read ();

			entryNombre.Text = dataReader ["nombre"].ToString ();
			entryPrecio.Text = dataReader ["precio"].ToString ();

			dataReader.Close ();

			FillComboboxCategoria ();
			combobox1.Active = 0;
		}

		//BOTON GUARDAR
		protected void OnSaveActionActivated (object sender, EventArgs e)
		{
			IDbCommand dbCommand = App.Instance.DbConnection.CreateCommand ();

			dbCommand.CommandText = String.Format ("UPDATE articulo SET nombre=@nombre, categoria=@categoria, precio=@precio WHERE id ={0}", id);
			dbCommand.AddParameter("nombre", entryNombre.Text);
			dbCommand.AddParameter("categoria", combobox1.ActiveText);
			dbCommand.AddParameter("precio", decimal.Parse(entryPrecio.Text));

			dbCommand.ExecuteNonQuery ();
			Console.WriteLine ("Cambios guardados ");
			Destroy ();
		}
		// COMBOBOX CATEGORIA
		protected void FillComboboxCategoria (){
			IDbCommand dbCommand =	App.Instance.DbConnection.CreateCommand ();
			dbCommand.CommandText = "SELECT id FROM categoria";
			IDataReader dataReader = dbCommand.ExecuteReader ();
			while (dataReader.Read ()) {
				combobox1.AppendText (dataReader ["id"].ToString ());
				//TODO ¿?
				//entryCategoria.Text = dataReader ["nombre"].ToString();
				//entryCategoria.Text = ""+dataReader ["nombre"];
				entryCategoria.Text = "TEST";
				//string nombre = dataReader ["nombre"].ToString();
				//entryNombre.Text = nombre;
			}
			dataReader.Close ();

		}


		protected void OnClearActionActivated (object sender, EventArgs e)
		{
			entryNombre.Text = "";
			entryPrecio.Text = "";
			//TODO CAMBIAR ESTO DE MANERA CORRECTA . CLEAR BORRA LA LISTA
			//combobox1.Clear ();
			entryCategoria.Text = "";
		}


		//BOTON DESHACER CAMBIOS
		protected void OnUndoActionActivated (object sender, EventArgs e)
		{
			IDbCommand dbCommand =	App.Instance.DbConnection.CreateCommand ();
			dbCommand.CommandText = String.Format ("SELECT * FROM articulo WHERE id = {0}", id);

			IDataReader dataReader = dbCommand.ExecuteReader ();
			dataReader.Read ();

			entryNombre.Text = dataReader ["nombre"].ToString ();
			entryPrecio.Text = dataReader ["precio"].ToString ();

			dataReader.Close ();

			combobox1.Active = 0;
			Console.WriteLine ("Cambios Realizados a los valores iniciales");
		}


	}
}

