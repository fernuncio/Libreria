using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Libreria
{
    public partial class Libro : Form
    {
        string[][] libros = new string[][]
{           new string[] { "Cien años de soledad", "Gabriel García Márquez", "Sudamericana", "1967", "Realismo mágico", "299.90" },
            new string[] { "1984", "George Orwell", "Secker & Warburg", "1949", "Distopía", "199.50" },
            new string[] { "El principito", "Antoine de Saint-Exupéry", "Reynal & Hitchcock", "1943", "Fábula", "150.00" },
            new string[] { "Fahrenheit 451", "Ray Bradbury", "Ballantine Books", "1953", "Ciencia ficción", "220.00" },
            new string[] { "Don Quijote de la Mancha", "Miguel de Cervantes", "Francisco de Robles", "1605", "Novela", "350.75" },
            new string[] { "Crónica de una muerte anunciada", "Gabriel García Márquez", "Oveja Negra", "1981", "Novela corta", "180.00" },
            new string[] { "La sombra del viento", "Carlos Ruiz Zafón", "Planeta", "2001", "Misterio", "275.00" },
            new string[] { "Orgullo y prejuicio", "Jane Austen", "T. Egerton", "1813", "Romance", "210.00" },
            new string[] { "El alquimista", "Paulo Coelho", "HarperTorch", "1988", "Ficción", "190.00" },
            new string[] { "It", "Stephen King", "Viking Press", "1986", "Terror", "320.00" }
        };
        bool bandera = true;
        string id;
        public Libro()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            //placeHolder();
        }

        public Libro(string id,string titulo,string autor,string editorial,
            string año,string genero,string precio,string stock)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.id = id;
            txtId.Text = id;
            txtTitulo.Text = titulo;
            txtAutor.Text = autor;
            txtEditorial.Text = editorial;
            txtAño.Text = año;
            txtGenero.Text = genero;
            txtPrecio.Text = precio;
            txtStock.Text = stock;
            bandera = false;
            buttonAgregar.Text = "ACTUALIZAR";
        }

        public void placeHolder()
        {
            txtId.Text = "Id Libro";
            txtId.ForeColor = Color.Gray;
            txtTitulo.Text = "Titulo";
            txtTitulo.ForeColor = Color.Gray;
            txtAutor.Text = "Autor";
            txtAutor.ForeColor = Color.Gray;
            txtEditorial.Text = "Editorial";
            txtEditorial.ForeColor = Color.Gray;
            txtAño.Text = "Año de Publicación";
            txtAño.ForeColor = Color.Gray;
            txtGenero.Text = "Genero";
            txtGenero.ForeColor = Color.Gray;
            txtPrecio.Text = "Precio";
            txtPrecio.ForeColor = Color.Gray;
            txtStock.Text = "Stock";
            txtStock.ForeColor = Color.Gray;
        }

        private void buttonAgregar_Click(object sender, EventArgs e)
        {
            Conexion conexion = new Conexion();
            if (bandera)
            {
                string consulta = "INSERT INTO libros (id_libro, titulo, autor, editorial, anio_publicacion, genero, precio, stock) VALUES" +
                    "(" + txtId.Text + ",'" + txtTitulo.Text + "','" + 
                    txtAutor.Text + "','" + txtEditorial.Text + "'," +
                    txtAño.Text + ",'" + txtGenero.Text + "'," + 
                    txtPrecio.Text + "," + txtStock.Text + ")";
                conexion.consulta(consulta);
                MessageBox.Show("Libro Agregado");
            }
            else
            {
                string consulta;
                //if (id.Equals(txtId.Text))
                //{
                //     consulta = "UPDATE libros SET " +
                //        "titulo = '" + txtTitulo.Text + "', " +
                //        "autor = '" + txtAutor.Text + "', " +
                //        "editorial = '" + txtEditorial.Text + "', " +
                //        "anio_publicacion = " + txtAño.Text + ", " +
                //        "genero = '" + txtGenero.Text + "', " +
                //        "precio = " + txtPrecio.Text + ", " +
                //        "stock = " + txtStock.Text +
                //        " WHERE id_libro = " + id;
                //}
                //else
                //{
                //    consulta = "UPDATE libros SET " +
                //        "id_libro=" + txtId.Text +
                //        "titulo = '" + txtTitulo.Text + "', " +
                //        "autor = '" + txtAutor.Text + "', " +
                //        "editorial = '" + txtEditorial.Text + "', " +
                //        "anio_publicacion = " + txtAño.Text + ", " +
                //        "genero = '" + txtGenero.Text + "', " +
                //        "precio = " + txtPrecio.Text + ", " +
                //        "stock = " + txtStock.Text +
                //        " WHERE id_libro = " + id;
                //}
                consulta = "UPDATE libros SET " +
                        "titulo = '" + txtTitulo.Text + "', " +
                        "autor = '" + txtAutor.Text + "', " +
                        "editorial = '" + txtEditorial.Text + "', " +
                        "anio_publicacion = " + txtAño.Text + ", " +
                        "genero = '" + txtGenero.Text + "', " +
                        "precio = " + txtPrecio.Text + ", " +
                        "stock = " + txtStock.Text +
                        " WHERE id_libro = " + id;
                conexion.consulta(consulta);
                MessageBox.Show("Libro Actualizado");
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int n = rnd.Next(0,libros.Length);
            txtId.Text = rnd.Next(100, 1001) + "";
            txtTitulo.Text = libros[n][0];
            txtAutor.Text = libros[n][1];
            txtEditorial.Text = libros[n][2];
            txtAño.Text = libros[n][3];
            txtGenero.Text = libros[n][4];
            txtPrecio.Text = libros[n][5];
            txtStock.Text = rnd.Next(1, 101) + "";
        }
    }
}
