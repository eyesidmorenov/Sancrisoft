using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sancrisoft.Models
{
	public class Aplicantes
	{
		[Key]
		public int IdAplicante { get; set; }
		[Required(ErrorMessage = "Ingrese los Nombres del Aplicante")]
		public string Nombres { get; set; }
		[Required(ErrorMessage = "Ingrese los Apellidos del Aplicante")]
		public string Apellidos { get; set; }

		[Display(Name = "Fecha Nacimiento")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
		public DateTime FechaNacimiento { get; set; }

		public string Email { get; set; }

		[Display(Name = "Cargo")]
		[Required(ErrorMessage = "Seleccione el cargo del Aplicante")]
		public int IdCargo { get; set; }
		
		public Cargos Cargos { get; set; }

		[Display(Name = "Fotografia")]
		public string UrlFoto { get; set; }
		
		[NotMapped]
		public HttpPostedFileWrapper FotoSubida { get; set; }

	}
}