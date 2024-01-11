using Newtonsoft.Json;
using organizacion_personal.modelos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace organizacion_personal.vistas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class pagina_lista_actividades : ContentPage
	{
        public ObservableCollection<actividad_diaria> Actividades_Diarias { get; set; }

        public pagina_lista_actividades (usuario usuario)
		{
			InitializeComponent ();
            Actividades_Diarias = new ObservableCollection<actividad_diaria>();
            CargarActividadesPorUsuario(usuario);
        }

        string apiUrl = "https://organizacion-personal.onrender.com/api/v1/actividad_diaria/";

        private async void CargarActividadesPorUsuario(usuario usuario)
        {
            try
            {
                using (var webClient = new HttpClient())
                {
                    var response = await webClient.GetAsync(apiUrl + usuario.us_cedula + "/");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        var responseObject = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);

                        if (responseObject != null)
                        {
                            if (!string.IsNullOrEmpty(responseObject.Mensaje))
                            {
                                await DisplayAlert("Mensaje", responseObject.Mensaje, "Aceptar");
                            }

                            if (responseObject.actividadesDiarias.Count > 0)
                            {
                                var tableSection = new TableSection("Lista de tus Actividades");

                                foreach (var actividad in responseObject.actividadesDiarias)
                                {
                                    var viewCell = new ViewCell
                                    {
                                        View = new StackLayout
                                        {
                                            Orientation = StackOrientation.Horizontal,
                                            Padding = new Thickness(10, 0, 10, 0),
                                            Children =
                                                {
                                                    new Label { Text = actividad.ad_secuencial+"" },
                                                    new Label { Text = actividad.ad_descripcion },
                                                    new Label { Text = actividad.ad_hora_inicio },
                                                    new Label { Text = actividad.ad_hora_fin },
                                                    new Label { Text = actividad.ad_dia },
                                                    new Label { Text = actividad.ad_estado }
                                                }
                                        }
                                    };

                                    tableSection.Add(viewCell);
                                }

                                ActividadesTableView.Root.Add(tableSection);
                            }
                            else if (responseObject.actividadDiaria != null)
                            {
                                var tableSection = new TableSection();
                                var viewCell = new ViewCell
                                {
                                    View = new StackLayout
                                    {
                                        Orientation = StackOrientation.Horizontal,
                                        Padding = new Thickness(10, 0, 10, 0),
                                        Children =
                                            {
                                                new Label { Text = responseObject.actividadDiaria.ad_secuencial+"" },
                                                new Label { Text = responseObject.actividadDiaria.ad_descripcion },
                                                new Label { Text = responseObject.actividadDiaria.ad_hora_inicio },
                                                new Label { Text = responseObject.actividadDiaria.ad_hora_fin },
                                                new Label { Text = responseObject.actividadDiaria.ad_dia },
                                                new Label { Text = responseObject.actividadDiaria.ad_estado }
                                            }
                                    }
                                };

                                tableSection.Add(viewCell);
                                ActividadesTableView.Root.Add(tableSection);
                            }
                            else
                            {
                                await DisplayAlert("Error", "No se encontraron actividades en la respuesta JSON."+responseObject.actividadesDiarias.Count+responseObject.actividadDiaria.ad_descripcion, "Aceptar");
                            }
                        }
                        else
                        {
                            await DisplayAlert("Error", "Respuesta JSON no válida.", "Aceptar");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", $"Error al cargar la lista de actividades. Código de estado: {response.StatusCode}", "Aceptar");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error: {ex.Message}", "Aceptar");
            }
        }



        private async void cmdRegresar_Clicked(object sender, EventArgs e)
        {
            try
            {
                var paginaPrincipal = new pagina_principal();
                await Navigation.PushAsync(paginaPrincipal);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error: {ex.Message}", "Aceptar");
            }
        }
    }
}