using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace practiceFinal
{
    internal class db
    {
        private SqlConnection conex;
        private string connectionString = "Data Source=DESKTOP-1A2H4UU\\SQLEXPRESS;Initial Catalog=Usuarios;Integrated Security=True";
        

        // Método para establecer la conexión con la base de datos
        public SqlConnection EstablecerConexion()
        {
            try
            {
                conex = new SqlConnection(connectionString);
                conex.Open();
                //MessageBox.Show("Se conectó correctamente a la Base de Datos");
            }
            catch (SqlException e)
            {
                MessageBox.Show("No se logró conectar a la Base de Datos" + e.ToString());
            }

            return conex;
        }

        // Método para cerrar la conexión con la base de datos
        public void CerrarConexion()
        {
            if (conex != null && conex.State == ConnectionState.Open)
            {
                conex.Close();
                //MessageBox.Show("Se cerró la conexión con la Base de Datos");
            }
        }

        // Método para comprobar si el usuario y la contraseña son correctos
        public bool ComprobarUsuario(string nombreUsuario, string contraseña)
        {
            string consultaSql = "SELECT COUNT(*) FROM users WHERE nombre = @nombreUsuario AND pasword = @contraseña";
            // Establece una conexión a la base de datos utilizando el método EstablecerConexion
            using (SqlConnection connection = EstablecerConexion())
            {
                // Verifica si la conexión se estableció correctamente
                if (connection == null || connection.State != ConnectionState.Open)
                {
                    // Si no se pudo establecer la conexión, retorna falso
                    return false;
                }

                try
                {
                    // Crea un comando SQL con la consulta y la conexión
                    using (SqlCommand command = new SqlCommand(consultaSql, connection))
                    {
                        // Agrega los parámetros a la consulta SQL para evitar inyección de SQL
                        command.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);
                        command.Parameters.AddWithValue("@contraseña", contraseña);

                        // Ejecuta la consulta y obtiene el resultado como un entero
                        int count = (int)command.ExecuteScalar();

                        // Si el resultado es mayor a 0, el usuario y contraseña son correctos
                        if (count > 0)
                        {
                            return true;
                        }
                        else
                        {
                            // Si el resultado es 0, muestra un mensaje de error indicando que el usuario y/o contraseña son incorrectos
                            MessageBox.Show("El usuario y/o contraseña son incorrectos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    // Si ocurre algún error durante la ejecución de la consulta SQL o la conexión, muestra un mensaje de error
                    MessageBox.Show("Error de conexión: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                finally
                {
                    // Cierra la conexión utilizando el método CerrarConexion
                    CerrarConexion();
                }
            }
        }
        // Método para mostrar la información de la base de datos
        public void Mostrar(DataGridView tabla)
        {
            // Crear una instancia de la clase de conexión a la base de datos
            db objetoConexion = new db();

            try
            {
                // Establecer el origen de datos del DataGridView como nulo para limpiar cualquier contenido anterior
                tabla.DataSource = null;

                // Crear un adaptador de datos para ejecutar la consulta "Select * from users" utilizando la conexión establecida
                SqlDataAdapter adapter = new SqlDataAdapter("Select * from users", EstablecerConexion());

                // Crear una nueva tabla de datos para almacenar los resultados de la consulta
                DataTable dt = new DataTable();

                // Llenar la tabla de datos con los resultados de la consulta utilizando el adaptador de datos
                adapter.Fill(dt);

                // Establecer el origen de datos del DataGridView como la tabla de datos
                tabla.DataSource = dt;

                // Cerrar la conexión a la base de datos después de obtener los resultados
                CerrarConexion();
            }
            catch (Exception ex)
            {
                // Mostrar un mensaje de error en un cuadro de diálogo en caso de que ocurra una excepción
                MessageBox.Show("No se logró mostrar los registros, error: " + ex.ToString());
            }
        }

        // Método para guardar la información en la base de datos
        public void Guardar(string userName, string password)
        {
            // Crear una instancia de la clase de conexión a la base de datos
            db objetoConexion = new db();

            try
            {
                // Definir la consulta SQL para insertar datos en la tabla "users"
                String query = "INSERT INTO users(nombre,pasword) VALUES (@UserName, @Password)";

                // Crear un comando SQL utilizando la consulta y la conexión establecida
                SqlCommand myCommand = new SqlCommand(query, EstablecerConexion());

                // Asignar valores a los parámetros de la consulta
                myCommand.Parameters.AddWithValue("@UserName", userName);
                myCommand.Parameters.AddWithValue("@Password", password);

                // Ejecutar el comando y obtener un lector de datos para leer los resultados (si los hay)
                SqlDataReader myReader;
                myReader = myCommand.ExecuteReader();

                // Leer los resultados (si los hay) utilizando el lector de datos
                while (myReader.Read()) { }

                // Cerrar la conexión a la base de datos después de realizar la operación de guardado
                CerrarConexion();
            }
            catch (Exception ex)
            {
                // Mostrar un mensaje de error en un cuadro de diálogo en caso de que ocurra una excepción
                MessageBox.Show("No se lograrón guardar los registros, error: " + ex.ToString());
            }
        }

        // Método para selección información del datagridview
        public void SeleccionarFila(DataGridView dataGridView, MaskedTextBox maskedTextBox1, MaskedTextBox maskedTextBox2, MaskedTextBox maskedTextBox3)
        {
            // Verificar si se ha seleccionado al menos una celda en el DataGridView
            if (dataGridView.SelectedCells.Count > 0)
            {
                // Obtener el índice de la fila correspondiente a la primera celda seleccionada
                int rowIndex = dataGridView.SelectedCells[0].RowIndex;

                // Obtener los valores de las celdas en la fila seleccionada
                string valor1 = dataGridView.Rows[rowIndex].Cells[0].Value.ToString();
                string valor2 = dataGridView.Rows[rowIndex].Cells[1].Value.ToString();
                string valor3 = dataGridView.Rows[rowIndex].Cells[2].Value.ToString();

                // Asignar los valores obtenidos a los MaskedTextBox correspondientes
                maskedTextBox1.Text = valor1;
                maskedTextBox2.Text = valor2;
                maskedTextBox3.Text = valor3;
            }
        }

        // Método para modificar la información en la base de datos
        public void Modificar(int idUsuario, string userName, string password)
        {
            try
            {
                // Definir la consulta SQL para actualizar los datos del usuario en la tabla "users"
                string query = "UPDATE users SET nombre = @UserName, pasword = @Password WHERE id = @IdUsuario";

                // Crear un comando SQL utilizando la consulta y la conexión establecida
                SqlCommand myCommand = new SqlCommand(query, EstablecerConexion());

                // Asignar valores a los parámetros de la consulta
                myCommand.Parameters.AddWithValue("@UserName", userName);
                myCommand.Parameters.AddWithValue("@Password", password);
                myCommand.Parameters.AddWithValue("@IdUsuario", idUsuario);

                // Ejecutar el comando y obtener un lector de datos para leer los resultados (si los hay)
                SqlDataReader myReader;
                myReader = myCommand.ExecuteReader();

                // Leer los resultados (si los hay) utilizando el lector de datos
                while (myReader.Read()) { }

                // Cerrar la conexión a la base de datos después de realizar la operación de modificación
                CerrarConexion();
            }
            catch (Exception ex)
            {
                // Mostrar un mensaje de error en un cuadro de diálogo en caso de que ocurra una excepción
                MessageBox.Show("No se logró modificar el registro, error: " + ex.ToString());
            }
        }

        // Método para eliminar un registro de la base de datos
        public void Eliminar(int idUsuario)
        {
            try
            {
                // Definir la consulta SQL para eliminar el usuario de la tabla "users"
                string query = "DELETE FROM users WHERE id = @IdUsuario";

                // Crear un comando SQL utilizando la consulta y la conexión establecida
                SqlCommand myCommand = new SqlCommand(query, EstablecerConexion());

                // Asignar el valor del parámetro de la consulta
                myCommand.Parameters.AddWithValue("@IdUsuario", idUsuario);

                // Ejecutar el comando y obtener un lector de datos para leer los resultados (si los hay)
                SqlDataReader myReader;
                myReader = myCommand.ExecuteReader();

                // Leer los resultados (si los hay) utilizando el lector de datos
                while (myReader.Read()) { }

                // Cerrar la conexión a la base de datos después de realizar la operación de eliminación
                CerrarConexion();
            }
            catch (Exception ex)
            {
                // Mostrar un mensaje de error en un cuadro de diálogo en caso de que ocurra una excepción
                MessageBox.Show("No se logró eliminar el registro, error: " + ex.ToString());
            }
        }
    }
}
