using System;
using System.Data;
using SerpisAd;
using PArticulo;

namespace PArticulo
{

	public partial class CategoriaWindow : Gtk.Window
	{
		private object id;
		public CategoriaWindow () : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
		}

		public CategoriaWindow(object id) : this(){
			this.id=id;
			IDbCommand dbCommand =
			App.Instance.DbConnection.CreateCommand ();
			dbCommand.CommandText = String.Format (
				"SELECT * FROM categoria where id = {0}", id);

			IDataReader dataReader = dbCommand.ExecuteReader ();
			dataReader.Read ();

			entryNombre.Text = dataReader ["nombre"].ToString ();
			entryID.Text = dataReader ["id"].ToString ();


			dataReader.Close ();
		}

		protected void OnSaveActionActivated (object sender, EventArgs e)
		{
			IDbCommand dbCommand =
				App.Instance.DbConnection.CreateCommand ();
			dbCommand.CommandText = String.Format (
				"UPDATE categoria SET nombre=@nombre where id ={0}", id);


			dbCommand.AddParameter("nombre", entryNombre.Text);

			dbCommand.ExecuteNonQuery ();
			Console.WriteLine ("Cambios guardados");
			Destroy ();
		}
		//BOTON LIMPIAR
		protected void OnClearActionActivated (object sender, EventArgs e)
		{
			entryNombre.Text = "";

		}
		//BOTON DESHACER CAMBIOS
		protected void OnUndoActionActivated (object sender, EventArgs e)
		{
			IDbCommand dbCommand =	App.Instance.DbConnection.CreateCommand ();
			dbCommand.CommandText = String.Format ("SELECT * FROM categoria WHERE id = {0}", id);

			IDataReader dataReader = dbCommand.ExecuteReader ();
			dataReader.Read ();

			entryNombre.Text = dataReader ["nombre"].ToString ();


			dataReader.Close ();


			Console.WriteLine ("Cambios Realizados a los valores iniciales");
		}



	}
}

