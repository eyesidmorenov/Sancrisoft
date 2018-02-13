using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sancrisoft.Models
{
	public class Cargos
	{
		[Key]
		public int IdCargo { get; set; }
		[Required(ErrorMessage = "Ingrese la Descripción del Cargo")]
		public string Descripcion { get; set; }

		public virtual ICollection<Aplicantes> ListaAplicantes { get; set; }
	}
}