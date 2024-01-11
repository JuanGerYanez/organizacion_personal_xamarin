using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace organizacion_personal.vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class login : ContentPage
    {
        public login()
        {
            InitializeComponent();
        }
        string apiUrl = "https://organizacion-personal.onrender.com/api/v1/login/";

        private async void cmdLogin_Clicked(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                try
                {
                    var usuario = new modelos.usuario
                    {
                        us_email = txtUsername.Text,
                        us_contrasenia = txtPassword.Text
                    };

                    string loginUrl = $"{apiUrl}?us_email={Uri.EscapeDataString(usuario.us_email)}&us_contrasenia={Uri.EscapeDataString(usuario.us_contrasenia)}";

                    using (var webClient = new HttpClient())
                    {
                        var response = await webClient.PostAsync(loginUrl, null);

                        if (response.IsSuccessStatusCode)
                        {
                            var jsonResponse = await response.Content.ReadAsStringAsync();
                            var usuarioBD = JsonConvert.DeserializeObject<modelos.usuario>(jsonResponse);

                            if (usuarioBD.us_cedula != null && usuarioBD.us_cedula != "")
                            {
                                modelos.usuario UsuarioEntrante = new modelos.usuario();
                                UsuarioEntrante.us_cedula = usuarioBD.us_cedula;
                                UsuarioEntrante.us_nombres = usuarioBD.us_nombres;
                                UsuarioEntrante.us_apellidos = usuarioBD.us_apellidos;
                                UsuarioEntrante.us_email = usuarioBD.us_email;
                                UsuarioEntrante.us_contrasenia = usuarioBD.us_contrasenia;
                                UsuarioEntrante.us_estado = usuarioBD.us_estado;
                                UsuarioEntrante.us_fecha_bd = usuarioBD.us_fecha_bd;

                                var paginaPrincipal = new pagina_principal(UsuarioEntrante);
                                await Navigation.PushAsync(paginaPrincipal);
                            }
                            else
                            {
                                await DisplayAlert("Inicio de sesión", "Credenciales Incorrectas", "Aceptar");
                            }
                        }
                        else
                        {
                            var errorResponse = await response.Content.ReadAsStringAsync();
                            await DisplayAlert("Error", $"Error al iniciar sesión. Detalles: {errorResponse}", "Aceptar");
                        }
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Error: {ex.Message}", "Aceptar");
                }
            }
            else
            {
                await DisplayAlert("Campos vacíos", "Por favor, ingrese nombre de usuario y contraseña.", "Aceptar");
            }
        }
    }
}