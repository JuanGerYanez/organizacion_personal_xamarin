using Newtonsoft.Json;
using organizacion_personal.modelos;
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
	public partial class pagina_principal : ContentPage
	{
        public modelos.usuario Usuario;

        public pagina_principal()
        {
            InitializeComponent();
        }

        public pagina_principal (usuario usuario)
		{
			InitializeComponent ();
            Usuario = usuario;
		}

        string apiUrl = "https://organizacion-personal.onrender.com/api/v1/actividad_diaria/";

        private async void cmdInsert_Clicked(object sender, EventArgs e)
        {
            try
            {
                using (var webClient = new HttpClient())
                {
                    var ad = new actividad_diaria
                    {
                        ad_descripcion = ad_descripcion_.Text,
                        ad_hora_inicio = ad_hora_inicio_.Text,
                        ad_hora_fin = ad_hora_fin_.Text,
                        ad_dia = ad_dia_.Text,
                        ad_usuario = this.Usuario.us_cedula,
                        ad_estado = "A",
                        ad_fecha_bd = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
                    };

                    var queryParams = 
                        $"?ad_descripcion={Uri.EscapeDataString(ad.ad_descripcion)}" +
                        $"&ad_hora_inicio=${ Uri.EscapeDataString(ad.ad_hora_inicio)}" +
                        $"&ad_hora_fin={Uri.EscapeDataString(ad.ad_hora_fin)}" +
                        $"&ad_dia={Uri.EscapeDataString(ad.ad_dia)}" +
                        $"&ad_usuario={Uri.EscapeDataString(ad.ad_usuario)}" ;

                    var response = await webClient.PostAsync(apiUrl + "insert/" + queryParams, null);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        var responseObject = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);

                        lblMensaje.Text = $"{responseObject.Mensaje}, " +
                                          $"Secuencial: {responseObject.actividadDiaria.ad_secuencial}, " +
                                          $"Descripcion: {responseObject.actividadDiaria.ad_descripcion}, " +
                                          $"Hora de inicio: {responseObject.actividadDiaria.ad_hora_inicio}, " +
                                          $"Hora de fin: {responseObject.actividadDiaria.ad_hora_fin}, " +
                                          $"Estado: {responseObject.actividadDiaria.ad_estado}";
                    }
                    else
                    {
                        lblMensaje.Text = $"Error al crear el libro. Código de estado: {response.StatusCode}";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = $"Error: {ex.Message}\nDetalles: {ex.StackTrace}";
            }
        }

        private async void cmdUpdate_Clicked(object sender, EventArgs e)
        {
            try
            {
                using (var webClient = new HttpClient())
                {
                    int actividad_diaria_id = int.Parse(ad_secuencial_.Text);

                    var ad = new actividad_diaria
                    {
                        ad_descripcion = ad_descripcion_.Text,
                        ad_hora_inicio = ad_hora_inicio_.Text,
                        ad_hora_fin = ad_hora_fin_.Text,
                        ad_dia = ad_dia_.Text,
                        ad_usuario = this.Usuario.us_cedula,
                        ad_estado = "A",
                        ad_fecha_bd = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
                    };

                    var requestBody = new
                    {
                        ad_descripcion = ad.ad_descripcion,
                        ad_hora_inicio = ad.ad_hora_inicio,
                        ad_hora_fin = ad.ad_hora_fin
                    };

                    var jsonBody = JsonConvert.SerializeObject(requestBody);
                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    var response = await webClient.PutAsync(apiUrl + "update/" + actividad_diaria_id, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        var responseObject = JsonConvert.DeserializeObject<ApiResponse_>(jsonResponse);

                        lblMensaje.Text = $"{responseObject.Mensaje}, " +
                                          $"Secuencial: {responseObject.actividadDiaria.ad_secuencial}, " +
                                          $"Descripcion: {responseObject.actividadDiaria.ad_descripcion}, " +
                                          $"Hora de inicio: {responseObject.actividadDiaria.ad_hora_inicio}, " +
                                          $"Hora de fin: {responseObject.actividadDiaria.ad_hora_fin}, " +
                                          $"Estado: {responseObject.actividadDiaria.ad_estado}";
                    }
                    else
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        lblMensaje.Text = $"Error al actualizar la actividad. Código de estado: {response.StatusCode}, " +
                                          $"Detalles: {errorResponse}";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = $"Error: {ex.Message}";
            }
        }

        private async void cmdReadByUser_Clicked(object sender, EventArgs e)
        {
            try
            {
                var listaDeActividades = new pagina_lista_actividades(this.Usuario);
                await Navigation.PushAsync(listaDeActividades);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error: {ex.Message}", "Aceptar");
            }
        }

        private async void cmdDelete_Clicked(object sender, EventArgs e)
        {
            try
            {
                using (var webClient = new HttpClient())
                {
                    int actividad_diaria_id = int.Parse(ad_secuencial_.Text);

                    var response = await webClient.DeleteAsync(apiUrl + "delete/" + actividad_diaria_id);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        var responseObject = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);

                        lblMensaje.Text = $"{responseObject.Mensaje}, " +
                                          $"Secuencial: {responseObject.actividadDiaria.ad_secuencial}, " +
                                          $"Descripcion: {responseObject.actividadDiaria.ad_descripcion}, " +
                                          $"Hora de inicio: {responseObject.actividadDiaria.ad_hora_inicio}, " +
                                          $"Hora de fin: {responseObject.actividadDiaria.ad_hora_fin}, " +
                                          $"Estado: {responseObject.actividadDiaria.ad_estado}";

                        ad_secuencial_.Text = "";
                        ad_descripcion_.Text = "";
                        ad_hora_inicio_.Text = "";
                        ad_hora_fin_.Text = "";
                        ad_dia_.Text = "";
                    }
                    else
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        lblMensaje.Text = $"Error al eliminar la actividad. Código de estado: {response.StatusCode}, " +
                                          $"Detalles: {errorResponse}";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = $"Error: {ex.Message}";
            }
        }

        private async void cmdCerrarSesion_Clicked(object sender, EventArgs e)
        {
            var login = new login();
            await Navigation.PushAsync(login);
        }

    }
}